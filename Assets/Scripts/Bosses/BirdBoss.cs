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

    private BounceBetweenTwoPoints bouncy;
    private Rotate r;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        _outlineMaterial = new Material(outlineMaterial);
        _deathMaterial = new Material(outlineMaterial);
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
        _outlineMaterial.SetColor("_Color", currColors[currColorEquipped]);

        if(currHealth <= 0.0f)
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
        }
    }

    void FixedUpdate()
    {
        //decrease outline size constantly
        if (outlineLength > 0.0f)
        {
            outlineLength -= Time.deltaTime * outlineDecreaseSpeed;
        }
        else
        {
            outlineLength = 0.0f;
        }
    }

    public override void SetBossColors(int i1, Color c1, int i2, Color c2)
    {
        base.SetBossColors(i1, c1, i2, c2);
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
}
