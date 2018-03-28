using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrinterBoss : MonoBehaviour 
{
	private Animator ani;

	[SerializeField]
	private float maxHealth;
	private float currHealth;

	[SerializeField]
	private GameObject healthBar;
	private RectTransform healthBarRect;

	private int bossPrimaryColor;
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
	private int[] currColorIndexes = new int[2];

	[SerializeField]
	private SpriteRenderer[] renderersToColor;
	[SerializeField]
	private Image[] imagesToColor;
	[SerializeField]
	private float switchDuration;
	private float t = 0.0f;
	private bool isSwitchingColors = false;

	// Use this for initialization
	void Start() 
	{
		ani = GetComponent<Animator>();
		if (healthBar != null)
		{
			healthBarRect = healthBar.GetComponent<RectTransform>();
		}

		currHealth = maxHealth;
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
}
