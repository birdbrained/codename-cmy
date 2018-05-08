using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefresherBoss : Boss
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
    private Transform bulletSpawnPos;
    [SerializeField]
    private GameObject bulletObj;
    public float normalDamage;
    [SerializeField]
    private float attackDelayTime = 1.0f;
    private float attackTimer = 0.0f;
    [SerializeField]
    private int numBulletsInSwipe = 5;
    [SerializeField]
    private float totalConeAngle = 70.0f;

    private BounceBetweenTwoPoints bouncy;
    private Rotate rotator;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        bouncy = GetComponent<BounceBetweenTwoPoints>();
        rotator = GetComponent<Rotate>();

        //ani = GetComponent<Animator>();
        if (healthBar != null)
        {
            healthBarRect = healthBar.GetComponent<RectTransform>();
        }

        currHealth = maxHealth;
        Attacking = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (currHealth <= 0.0f)
        {
            if (canSetDieTrigger)
            {
                MyAnimator.SetTrigger("die");
                Instantiate(ParticleManager.Instance.DyingParticles, transform.position, Quaternion.identity);
                GameManager.Instance.DestroyAllBulletsAndSpawns();
                if (bouncy != null)
                {
                    bouncy.CanBounce = false;
                }
                if (rotator != null)
                {
                    rotator.resetRotationWhenStopped = true;
                    rotator.canRotate = false;
                }
                canSetDieTrigger = false;
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
            if (!IsDead)
            {
                if (bouncy != null)
                {
                    bouncy.CanBounce = true;
                }
                if (rotator != null)
                    rotator.canRotate = true;
            }
            if (attackTimer >= attackDelayTime)
            {
                attackTimer = 0.0f;
                //attack function here
                MyAnimator.SetTrigger("attack");
                if (bouncy != null)
                {
                    bouncy.CanBounce = false;
                }
                if (rotator != null)
                    rotator.canRotate = false;
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
                particle = Instantiate(ParticleManager.Instance.CritParticle, other.gameObject.transform.position, gameObject.transform.rotation);
            else if (damageMod == 0.5f)
                particle = Instantiate(ParticleManager.Instance.ResistParticle, other.gameObject.transform.position, gameObject.transform.rotation);
            else
                particle = Instantiate(ParticleManager.Instance.WhiffParticle, other.gameObject.transform.position, gameObject.transform.rotation);

            particle.GetComponent<SpriteRenderer>().material.color = GameManager.Instance.PlayerColors[_bullet.colorIndex];

            if (other.gameObject.tag != "laser")
                Destroy(other.gameObject);
        }
    }

    public void Attack()
    {
        Vector3 startRotation = new Vector3(0, 0, (-1.0f * (float)(totalConeAngle / 2.0f)) - 90.0f);
        for (int i = 0; i < numBulletsInSwipe; i++)
        {
            float angleOffset = (float)((totalConeAngle / numBulletsInSwipe) * (float)i);
            GameObject _bullet = Instantiate(bulletObj, bulletSpawnPos.position, Quaternion.Euler(new Vector3(startRotation.x, startRotation.y, transform.rotation.eulerAngles.z + startRotation.z + angleOffset)));
            Bullet _bulletComponent = _bullet.GetComponent<Bullet>();
            _bullet.tag = "enemy_bullet";
            _bullet.transform.localScale *= 1.5f;
            _bulletComponent.SetBulletAttributes(gameObject, GetCurrentColor(), GetCurrentColorIndex(), normalDamage);
        }
    }
}
