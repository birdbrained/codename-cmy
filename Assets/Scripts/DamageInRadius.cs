using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInRadius : MonoBehaviour
{
    public float radius;
    public float damageToDeal;
    public int colorIndex;
    private Color color;
    private GameObject owner;
    private ParticleSystem ps;
    private ParticleSystem.MainModule mainPS;
    private bool canExplode = false;
    [SerializeField]
    private LayerMask mask;

    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            mainPS = ps.main;
        }
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canExplode)
        {
            Explode();
            canExplode = false;
        }
	}

    public void Explode()
    {
        ps.Play();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && colliders[i].gameObject != owner)
            {
                PlayerController player = colliders[i].gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    //this object is another player, damage them
                    float damageMod = GameManager.Instance.DamageModifier(colorIndex, player.GetCurrentColorIndex());
                    //StartCoroutine(player.TakeDamage(damageToDeal * damageMod, "other player"));
                    player.TakeDamageWrapper(damageToDeal, damageMod, "other player");

                    GameObject parts;

                    if (damageMod == 2.0f)
                        parts = Instantiate(ParticleManager.Instance.CritParticle, colliders[i].gameObject.transform.position, transform.rotation);
                    else if (damageMod == 0.5f)
                        parts = Instantiate(ParticleManager.Instance.ResistParticle, colliders[i].gameObject.transform.position, transform.rotation);
                    else
                        parts = Instantiate(ParticleManager.Instance.WhiffParticle, colliders[i].gameObject.transform.position, transform.rotation);

                    parts.GetComponent<SpriteRenderer>().material.color = player.GetCurrentColor();
                }
                else
                {
                    Boss boss = colliders[i].gameObject.GetComponent<Boss>();
                    if (boss != null)
                    {
                        //this object is a boss, damage it
                        float damageMod = GameManager.Instance.DamageModifier(colorIndex, boss.GetCurrentColorIndex());
                        boss.DealDamage(damageToDeal, damageMod);
                        if (boss.GetCurrentHealth() <= 0.0f)
                        {
                            AchievementManager.Instance.AwardAchievement(1);
                        }

                        GameObject parts;

                        if (damageMod == 2.0f)
                            parts = Instantiate(ParticleManager.Instance.CritParticle, colliders[i].gameObject.transform.position, transform.rotation);
                        else if (damageMod == 0.5f)
                            parts = Instantiate(ParticleManager.Instance.ResistParticle, colliders[i].gameObject.transform.position, transform.rotation);
                        else
                            parts = Instantiate(ParticleManager.Instance.WhiffParticle, colliders[i].gameObject.transform.position, transform.rotation);

                        parts.GetComponent<SpriteRenderer>().material.color = boss.GetCurrentColor();
                    }
                }
            }
        }
    }

    /**
     * @brief Sets up the properites of the explosion
     * @param Owner The owner of the explosion
     * @param Radius The radius of the explosion (negative number to use default)
     * @param Damage The damage of the explosion (negative number to use deafult)
     * @param ColorIndex The index of the color to use for the attack
     * @param C The color to color the particles with
     */ 
    public void SetupExplosion(GameObject Owner, float Radius, float Damage, int ColorIndex, Color C)
    {
        owner = Owner;
        if (Radius >= 0.0f)
            radius = Radius;
        if (Damage >= 0.0f)
            damageToDeal = Damage;
        colorIndex = ColorIndex;
        color = C;
        canExplode = true;

        if (ps != null)
        {
            Debug.Log("Changing particle color");
            mainPS.startColor = C;
        }
    }

    public void SetupExplosionColor(Color C)
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = ps.main;
        ma.startColor = new Color(C.r, C.g, C.b, C.a);
    }
}
