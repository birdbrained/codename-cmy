using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelephoneBoss : Boss
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
    private Transform[] bulletSpawnPositions;
    [SerializeField]
    private LineRenderer cordLine;
    [SerializeField]
    private Transform cordAttachPoint;

    [SerializeField]
    [Range(1, 4)]
    private int numOfAttacks = 1;
    [SerializeField]
    private float attackDelayTime = 1.0f;
    private float attackTimer = 0.0f;
    [SerializeField]
    private GameObject bulletObj;
    public float normalDamage;
    [SerializeField]
    private int numBulletsInRing = 8;

    private int[] xSpawnPos = { -15, 15 };
    private float ySpawnMax = 5.0f;
    [SerializeField]
    private float maxSpeed;
    private float currSpeed;
    private float flipMovement;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (healthBar != null)
        {
            healthBarRect = healthBar.GetComponent<RectTransform>();
        }

        currHealth = maxHealth;
        Attacking = false;

        currSpeed = maxSpeed;
        DecideCycleStart();
    }
	
	// Update is called once per frame
	public override void Update()
    {
        //if (healthBar != null && healthBarRect != null)
        //{
        //healthBarRect.sizeDelta = new Vector2(100.0f, ((float)currHealth / (float)maxHealth) * 145.0f);
        //}
        base.Update();

        if (currHealth <= 0.0f)
        {
            //change to play death animation
            //Destroy(gameObject);
            MyAnimator.SetTrigger("die");
            canSetDieTrigger = false;
        }

        transform.Translate(Vector3.right * Time.deltaTime * currSpeed * flipMovement, Space.World);
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
                //Attacking = true;
                MyAnimator.SetTrigger("attack");
            }
        }

        if (currSpeed > -maxSpeed)
        {
            currSpeed -= Time.deltaTime * 3.0f;
        }
        else
        {
            currSpeed = maxSpeed;
            DecideCycleStart();
        }

        if (IsDead)
        {
            currSpeed = 0.0f;
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
        if (cordLine != null)
        {
            cordLine.SetPosition(1, cordAttachPoint.position);
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

    public void Attack(int side)
    {
        if (IsDead)
            return;

        if (side < 0)
            side = 0;
        if (side > 1)
            side = 1;

        for (int i = 0; i < numBulletsInRing; i++)
        {
            GameObject _bullet;
            Bullet _bulletComponent;

            _bullet = Instantiate(bulletObj, bulletSpawnPositions[side].position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0.0f, 360.0f))));
            _bulletComponent = _bullet.GetComponent<Bullet>();
            _bullet.tag = "enemy_bullet";
            _bullet.transform.localScale *= 1.5f;
            _bulletComponent.SetBulletAttributes(gameObject, currColors[currColorEquipped], currColorIndexes[currColorEquipped], normalDamage);
        }
    }

    void DecideCycleStart()
    {
        int randomRange = Random.Range(0, xSpawnPos.Length);
        float randomY = Random.Range(-ySpawnMax, ySpawnMax);
        if (randomRange > 0)
        {
            Vector3 scale = new Vector3(-1, 1, 1);
            transform.localScale = scale;
            flipMovement = -1.0f;
        }
        else
        {
            Vector3 scale = new Vector3(1, 1, 1);
            transform.localScale = scale;
            flipMovement = 1.0f;
        }
        gameObject.transform.position = new Vector3(xSpawnPos[randomRange], randomY, 0);
        if (cordLine != null)
        {
            cordLine.SetPosition(0, new Vector3(xSpawnPos[randomRange], randomY, 0));
        }
    }
}
