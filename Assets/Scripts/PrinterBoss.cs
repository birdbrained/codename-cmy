using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrinterBoss : Boss
{
	//private Animator ani;

	//[SerializeField]
	//private float maxHealth;
	//private float currHealth;

	//[SerializeField]
	//private GameObject healthBar;
	//private RectTransform healthBarRect;

	/*private int bossPrimaryColor;
	public int BossPrimaryColor
	{
		get
		{
			return bossPrimaryColor;
		}
		set
		{
			bossPrimaryColor = value;
		}
	}
	private int bossSecondaryColor;
	public int BossSecondaryColor
	{
		get
		{
			return bossSecondaryColor;
		}
		set
		{
			bossSecondaryColor = value;
		}
	}
	private int currColorEquipped = 0;
	public int CurrColorEquippped
	{
		get
		{
			return currColorEquipped;
		}
	}
	private Color[] currColors = new Color[2];
	private int[] currColorIndexes = new int[2];*/

	[SerializeField]
	private SpriteRenderer[] renderersToColor;
	[SerializeField]
	private Image[] imagesToColor;
	[SerializeField]
	private float switchDuration;
	private float t = 0.0f;
	private bool isSwitchingColors = false;

	[SerializeField]
	private GameObject leftFireSpawn;
	[SerializeField]
	private GameObject rightFireSpawn;
	[SerializeField]
	private GameObject centerFireSpawn;

	[SerializeField][Range (1, 4)]
	private int numOfAttacks = 1;
	[SerializeField]
	private float attackDelayTime = 1.0f;
	private float attackTimer = 0.0f;
	private bool Attacking { get; set; }
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

		//ani = GetComponent<Animator>();
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
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		if (isSwitchingColors)
		{
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
				isSwitchingColors = false;
			}
		}

		if (!Attacking)
		{
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackDelayTime)
			{
				attackTimer = 0.0f;
				Attacking = true;
				MyAnimator.SetTrigger("attack");
			}
		}
	}

	public void SetBossColors(int i1, Color c1, int i2, Color c2)
	{
		bossPrimaryColor = i1;
		bossSecondaryColor = i2;
		currColorIndexes[0] = i1;
		currColorIndexes[1] = i2;
		currColors[0] = c1;
		currColors[1] = c2;
		currColorEquipped = 0;

		for (int i = 0; i < renderersToColor.Length; i++)
			renderersToColor[i].material.color = currColors[0];
		for (int i = 0; i < imagesToColor.Length; i++)
			imagesToColor[i].color = currColors[0];
	}

	public void SwapColors()
	{
		if (currColorEquipped == 0)
			currColorEquipped = 1;
		else
			currColorEquipped = 0;

		isSwitchingColors = true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "player_bullet")
		{
			Bullet _bullet = other.gameObject.GetComponent<Bullet>();
			float damageMod = GameManager.Instance.DamageModifier(_bullet.colorIndex, currColorIndexes[currColorEquipped]);
			DealDamage(_bullet.damageAmount, damageMod);

			GameObject particle;

			if (damageMod == 2.0f)
				particle = Instantiate(GameManager.Instance.CritParticle, other.gameObject.transform.position, gameObject.transform.rotation);
			else if (damageMod == 0.5f)
				particle = Instantiate(GameManager.Instance.ResistParticle, other.gameObject.transform.position, gameObject.transform.rotation);
			else
				particle = Instantiate(GameManager.Instance.WhiffParticle, other.gameObject.transform.position, gameObject.transform.rotation);

			particle.GetComponent<SpriteRenderer>().material.color = GameManager.Instance.PlayerColors[_bullet.colorIndex];

			Destroy(other.gameObject);
		}
	}

	public void DealDamage(float damage, float damageMod)
	{
		currHealth -= (damage * damageMod);
	}

	public int GetCurrentColorIndex()
	{
		return currColorIndexes[currColorEquipped];
	}

	/**
	 * @brief The printer's main attack
	 * @param side 0 for left attack, non-zero for right attack
	 */
	public void Attack(int side)
	{
		for (int i = 0; i < numBulletsInSwipe; i++)
		{
			GameObject _bullet;
			Bullet _bulletComponent;

			if (side == 0)
			{
				_bullet = Instantiate(bulletObj, leftFireSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, -45 + Random.Range(-angleOffset, angleOffset))));
				_bulletComponent = _bullet.GetComponent<Bullet>();
				_bullet.tag = "enemy_bullet";
				_bullet.transform.localScale *= 2;
				_bulletComponent.FireSprite.material.color = currColors[currColorEquipped];
				_bulletComponent.colorIndex = currColorIndexes[currColorEquipped];
				_bulletComponent.damageAmount = normalDamage;
			}
			else
			{
				_bullet = Instantiate(bulletObj, rightFireSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 45 + Random.Range(-angleOffset, angleOffset))));
				_bulletComponent = _bullet.GetComponent<Bullet>();
				_bullet.tag = "enemy_bullet";
				_bullet.transform.localScale *= 2;
				_bulletComponent.FireSprite.material.color = currColors[currColorEquipped];
				_bulletComponent.colorIndex = currColorIndexes[currColorEquipped];
				_bulletComponent.damageAmount = normalDamage;
			}
		}
	}

	public void ChargeAttack()
	{
		GameObject _bullet;
		Bullet _bulletComponent;

		_bullet = Instantiate(chargeBulletObj, centerFireSpawn.transform.position, transform.rotation);
		_bulletComponent = _bullet.GetComponent<Bullet>();
		_bullet.tag = "enemy_bullet";
		_bulletComponent.FireSprite.material.color = currColors[currColorEquipped];
		_bulletComponent.colorIndex = currColorIndexes[currColorEquipped];
		_bulletComponent.damageAmount = chargeDamage;
	}
}
