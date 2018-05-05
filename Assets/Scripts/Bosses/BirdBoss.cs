using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdBoss : Boss
{
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private Material outlineMaterial;
    private Material _outlineMaterial;
    private float outlineLength = 0.0f;
    [SerializeField][Range(0,1)]
    private float outlineDecreaseSpeed;
    [SerializeField]
    private Material deathMaterial;
    private Material _deathMaterial;

    [SerializeField]
    private Image[] imagesToColor;
    [SerializeField]
    private float switchDuration;
    private float t = 0.0f;
    private bool isSwitchingColors = false;

    //stuff for attacking
    [SerializeField]
    private Transform[] bulletSpawnPositions;
    [SerializeField]
    [Range(1, 4)]
    private int numOfAttacks = 1;
    [SerializeField]
    private float attackDelayTime = 0.5f;
    private float attackTimer = 0.0f;
    [SerializeField]
    private float chargeDelayTime = 2.0f;
    private float chargeTimer = 0.0f;
    [SerializeField]
    private GameObject bulletObj;
    public float normalDamage;
    [SerializeField]
    private GameObject chargeBulletObj;
    public float chargeDamage;
    [SerializeField]
    private int numBulletsPerAttack = 3;

    //for shader
    public float fractureIntensity = 0.0f; //dont touch this one
    private bool fracturing = false;
    private bool canSwapToDeathMaterial = true;

    private BounceBetweenTwoPoints bouncy;
    private Rotate r;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        _outlineMaterial = new Material(outlineMaterial);
        _deathMaterial = new Material(deathMaterial);
        if (body != null)
        {
            body.GetComponent<Renderer>().material = _outlineMaterial;
        }
        bouncy = GetComponent<BounceBetweenTwoPoints>();
        r = GetComponentInChildren<Rotate>();

        if (healthBar != null)
        {
            healthBarRect = healthBar.GetComponent<RectTransform>();
        }

        currHealth = maxHealth;
        Attacking = false;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        _outlineMaterial.SetFloat("_Outline", outlineLength);
        //_outlineMaterial.SetColor("_Color", currColors[currColorEquipped]);

        if (currHealth <= 0.0f)
        {
            //change to play death animation
            //Destroy(gameObject);
            if (canSetDieTrigger)
            {
                MyAnimator.SetTrigger("die");
                if (bouncy != null)
                {
                    bouncy.CanBounce = false;
                }
                if (r != null)
                {
                    r.canRotate = false;
                }
                canSetDieTrigger = false;
            }
            /*if (canSwapToDeathMaterial)
            {
                SwapToDeathMaterial();
                canSwapToDeathMaterial = false;
            }*/
        }
    }

    void FixedUpdate()
    {
        if (isSwitchingColors)
        {
            attackTimer = 0.0f;
            if (currColorEquipped == 0)
                _outlineMaterial.SetColor("_Color", Color.Lerp(currColors[0], currColors[1], t));
            else
                _outlineMaterial.SetColor("_Color", Color.Lerp(currColors[1], currColors[0], t));
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
                //MyAnimator.SetTrigger("attack");
                Attack(Random.Range(0, 2));
            }

            chargeTimer += Time.deltaTime;
            if (chargeTimer >= chargeDelayTime)
            {
                chargeTimer = 0.0f;
                ChargeAttack();
            }
        }

        //decrease outline size constantly
        if (outlineLength > 0.0f)
        {
            outlineLength -= Time.deltaTime * outlineDecreaseSpeed;
        }
        else
        {
            outlineLength = 0.0f;
        }

        //if dying, increase shader stuffs
        if (fracturing)
        {
            _deathMaterial.SetFloat("_Outline", fractureIntensity);
        }

        //death handler
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
        _outlineMaterial.SetColor("_Color", c1);
    }

    public override void DealDamage(float damage, float damageMod)
    {
        base.DealDamage(damage, damageMod);
        outlineLength += 0.1f;
        if (outlineLength > 1.0f)
            outlineLength = 1.0f;
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
    }

    public void SwapToDeathMaterial()
    {
        body.GetComponent<Renderer>().material = _deathMaterial;
        fracturing = true;
    }

    public void Attack(int side)
    {
        if (IsDead)
            return;
        if (side < 0)
            side = 0;
        if (side > 1)
            side = 1;

        for (int i = 0; i < numBulletsPerAttack; i++)
        {
            GameObject _bullet;
            Bullet _bulletComponent;

            _bullet = Instantiate(bulletObj, bulletSpawnPositions[side].position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-60.0f, 60.0f))));
            _bulletComponent = _bullet.GetComponent<Bullet>();
            _bullet.tag = "enemy_bullet";
            _bullet.transform.localScale *= 1.5f;
            int index = Random.Range(0, 3);
            _bulletComponent.SetBulletAttributes(gameObject, GameManager.Instance.PlayerColors[index], index, normalDamage);
        }
    }

    public void ChargeAttack()
    {
        if (IsDead)
            return;

        GameObject _bullet;
        Bullet _bulletComponent;

        _bullet = Instantiate(chargeBulletObj, bulletSpawnPositions[2].position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-60.0f, 60.0f))));
        _bulletComponent = _bullet.GetComponent<Bullet>();
        _bullet.tag = "enemy_bullet";
        _bullet.transform.localScale *= 3.0f;
        _bulletComponent.SetBulletAttributes(gameObject, currColors[currColorEquipped], currColorIndexes[currColorEquipped], chargeDamage);
    }
}
