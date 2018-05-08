using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SissorsBoss : Boss
{
    [SerializeField]
    private SpriteRenderer[] renderersToColor;
    [SerializeField]
    private Image[] imagesToColor;
    [SerializeField]
    private float switchDuration;
    private float t = 0.0f;
    private bool isSwitchingColors = false;

    [SerializeField]
    private GameObject meleeObj;
    private Collider2D meleeCollider;
    [SerializeField]
    private float attackDelayTime = 1.0f;
    private float attackTimer = 0.0f;
    public float normalDamage;

    private Rigidbody2D rb2d;
    [SerializeField]
    private float pushForce = 10.0f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
        if (healthBar != null)
        {
            healthBarRect = healthBar.GetComponent<RectTransform>();
        }
        if (meleeObj != null)
        {
            meleeCollider = meleeObj.GetComponent<Collider2D>();
            meleeObj.SetActive(false);
        }

        currHealth = maxHealth;
        Attacking = false;

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController p in players)
        {
            Physics2D.IgnoreCollision(p.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
	
	// Update is called once per frame
	public override void Update()
    {
        base.Update();

        if (currHealth <= 0.0f)
        {
            //change to play death animation
            //Destroy(gameObject);
            if (canSetDieTrigger)
            {
                MyAnimator.SetTrigger("die");
                Instantiate(ParticleManager.Instance.DyingParticles, transform.position, Quaternion.identity);
                canSetDieTrigger = false;
                GameManager.Instance.DestroyAllBulletsAndSpawns();
            }
        }
    }

    void FixedUpdate()
    {
        if (isSwitchingColors)
        {
            attackTimer = 0.0f;
            for (int i = 0; i < renderersToColor.Length; i++)
            {
                if (currColorEquipped == 0)
                    renderersToColor[i].material.color = Color.Lerp(currColors[0], currColors[1], t);
                else
                    renderersToColor[i].material.color = Color.Lerp(currColors[1], currColors[0], t);
            }
            for (int i = 0; i < imagesToColor.Length; i++)
            {
                if (currColorEquipped == 0)
                    imagesToColor[i].color = Color.Lerp(currColors[0], currColors[1], t);
                else
                    imagesToColor[i].color = Color.Lerp(currColors[1], currColors[0], t);
            }

            if (t < 1.0f)
            {
                t += Time.deltaTime / switchDuration;
            }
            else
            {
                SwapColors();
                isSwitchingColors = false;
                t = 0.0f;
            }
        }

        if (tSwitch < timeBeforeSwitching)
        {
            if (!Attacking)
                tSwitch += Time.deltaTime;
        }
        else
        {
            isSwitchingColors = true;
            tSwitch = 0.0f;
        }

        if (!Attacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelayTime)
            {
                attackTimer = 0.0f;
                //attack function here
                //Attack();
                MyAnimator.SetTrigger("attack");
            }
        }

        if (IsDead)
        {
            if (deathTimer > 0.0f)
            {
                deathTimer -= Time.deltaTime;
            }
            else if (deathTimer <= 0.0f && canTransitionToSecondDeathPhase)
            {
                canTransitionToSecondDeathPhase = false;
                MyAnimator.SetTrigger("die2");
            }
        }
    }

    public override void SetBossColors(int i1, Color c1, int i2, Color c2)
    {
        base.SetBossColors(i1, c1, i2, c2);

        for (int i = 0; i < renderersToColor.Length; i++)
            renderersToColor[i].material.color = currColors[0];
        for (int i = 0; i < imagesToColor.Length; i++)
            imagesToColor[i].color = currColors[0];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player_bullet" || other.gameObject.tag == "laser")
        {
            Bullet _bullet = other.gameObject.GetComponent<Bullet>();
            float damageMod = GameManager.Instance.DamageModifier(_bullet.colorIndex, currColorIndexes[currColorEquipped]);
            DealDamage(_bullet.damageAmount, damageMod);

            GameObject particle;

            if (damageMod == 2.0f)
                particle = Instantiate(ParticleManager.Instance.CritParticle, other.gameObject.transform.position, Quaternion.identity);
            else if (damageMod == 0.5f)
                particle = Instantiate(ParticleManager.Instance.ResistParticle, other.gameObject.transform.position, Quaternion.identity);
            else
                particle = Instantiate(ParticleManager.Instance.WhiffParticle, other.gameObject.transform.position, Quaternion.identity);

            particle.GetComponent<SpriteRenderer>().material.color = GameManager.Instance.PlayerColors[_bullet.colorIndex];

            if (other.gameObject.tag != "laser")
                Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                if (!player.IsDead)
                {
                    float damageMod;
                    if (player.playerNum == 1)
                    {
                        if (player.CurrentColorEqupped == 0)
                            damageMod = GameManager.Instance.DamageModifier(GetCurrentColorIndex(), GameManager.Instance.playerOnePrimaryColorIndex);
                        else
                            damageMod = GameManager.Instance.DamageModifier(GetCurrentColorIndex(), GameManager.Instance.playerOneSecondaryColorIndex);
                    }
                    else
                    {
                        if (player.CurrentColorEqupped == 0)
                            damageMod = GameManager.Instance.DamageModifier(GetCurrentColorIndex(), GameManager.Instance.playerTwoPrimaryColorIndex);
                        else
                            damageMod = GameManager.Instance.DamageModifier(GetCurrentColorIndex(), GameManager.Instance.playerTwoSecondaryColorIndex);
                    }

                    StartCoroutine(player.TakeDamage(normalDamage * damageMod, gameObject.name));

                    GameObject parts;

                    if (damageMod == 2.0f)
                        parts = Instantiate(ParticleManager.Instance.CritParticle, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
                    else if (damageMod == 0.5f)
                        parts = Instantiate(ParticleManager.Instance.ResistParticle, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
                    else
                        parts = Instantiate(ParticleManager.Instance.WhiffParticle, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);

                    parts.GetComponent<SpriteRenderer>().material.color = GetCurrentColor();
                }
            }
        }
    }

    void AddRotation()
    {
        transform.rotation *= Quaternion.Euler(0, 0, 90);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        if (other.gameObject.tag == "right_wall")
        {
            //rb2d.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
            AddRotation();
            //rb2d.velocity = new Vector2(rb2d.velocity.x * -1.0f, rb2d.velocity.y);
        }
        if (other.gameObject.tag == "left_wall")
        {
            //rb2d.AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);
            AddRotation();
            //rb2d.velocity = new Vector2(rb2d.velocity.x * -1.0f, rb2d.velocity.y);
        }
        if (other.gameObject.tag == "top_wall")
        {
            //rb2d.AddForce(Vector2.down * pushForce, ForceMode2D.Impulse);
            AddRotation();
            //rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * -1.0f);
        }
        if (other.gameObject.tag == "bottom_wall")
        {
            AddRotation();
            //rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * -1.0f);
            //rb2d.AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
        }
    }

    public void Attack()
    {
        rb2d.AddForce(transform.up * -1.0f * pushForce, ForceMode2D.Impulse);
    }

    public void AttackWarmup()
    {
        rb2d.velocity = Vector2.zero;
        transform.rotation *= Quaternion.Euler(0, 0, 0);
    }
}
