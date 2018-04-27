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
    [Range(1, 4)]
    private int numOfAttacks = 1;
    [SerializeField]
    private float attackDelayTime = 1.0f;
    private float attackTimer = 0.0f;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private GameObject chargeBulletObj;
    public float normalDamage;
    public float chargeDamage;
    [SerializeField]
    private int numBulletsInSwipe = 5;
    [SerializeField]
    private int angleOffset = 5;

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
    }
	
	// Update is called once per frame
	void Update()
    {
        if (healthBar != null && healthBarRect != null)
        {
            healthBarRect.sizeDelta = new Vector2(100.0f, ((float)currHealth / (float)maxHealth) * 145.0f);
        }

        if (currHealth <= 0.0f)
        {
            //change to play death animation
            //Destroy(gameObject);
            MyAnimator.SetTrigger("die");
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
                //Attacking = true;
                MyAnimator.SetTrigger("attack");
            }
        }
    }
}
