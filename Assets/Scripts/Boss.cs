using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour 
{
	[SerializeField]
	protected float maxHealth;
	protected float currHealth;

	[SerializeField]
	protected GameObject healthBar;
	protected RectTransform healthBarRect;

	protected int bossPrimaryColor;
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
	protected int bossSecondaryColor;
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
	protected int currColorEquipped = 0;
	public int CurrColorEquippped
	{
		get
		{
			return currColorEquipped;
		}
	}
	protected Color[] currColors = new Color[2];
	protected int[] currColorIndexes = new int[2];
	public Animator MyAnimator { get; private set; }
	public bool IsDead
	{
		get
		{
			return currHealth <= 0.0f;
		}
	}
    public bool Attacking { get; set; }

    // Use this for initialization
    public virtual void Start () 
	{
		MyAnimator = GetComponent<Animator>();

		currHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator TakeDamage(float damage, string dealer)
	{
		currHealth -= damage;

		if (!IsDead)
		{
			//not dead

		}
		else
		{
			//dead
		}
		yield return null;
	}

}
