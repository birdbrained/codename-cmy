using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour 
{
	[SerializeField]
	protected float maxHealth;
	protected float currHealth;

	[SerializeField]
	protected GameObject healthBar;
	protected RectTransform healthBarRect;

    [SerializeField]
    protected float timeBeforeSwitching;
    protected float tSwitch;

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
    public int[] CurrColorIndexes
    {
        get
        {
            return currColorIndexes;
        }
    }
	public Animator MyAnimator { get; private set; }
	public bool IsDead
	{
		get
		{
			return currHealth <= 0.0f;
		}
	}
    public bool Attacking { get; set; }
    protected bool canSetDieTrigger = true;
    [SerializeField]
    protected float deathTimer;
    protected bool canTransitionToSecondDeathPhase = true;
    protected bool canAwardUphillBattleAchievement = true;

    // Use this for initialization
    public virtual void Start() 
	{
		MyAnimator = GetComponent<Animator>();

		currHealth = maxHealth;
	}
	
	// Update is called once per frame
	public virtual void Update()
    {
        if (healthBar != null && healthBarRect != null)
        {
            healthBarRect.sizeDelta = new Vector2(100.0f, ((float)currHealth / (float)maxHealth) * 145.0f);
        }
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

    public void SwapColors()
    {
        if (currColorEquipped == 0)
            currColorEquipped = 1;
        else
            currColorEquipped = 0;

        //isSwitchingColors = true;
    }

    public virtual void SetBossColors(int i1, Color c1, int i2, Color c2)
    {
        bossPrimaryColor = i1;
        bossSecondaryColor = i2;
        currColorIndexes[0] = i1;
        currColorIndexes[1] = i2;
        currColors[0] = c1;
        currColors[1] = c2;
        currColorEquipped = 0;
    }

    public virtual void DealDamage(float damage, float damageMod)
    {
        currHealth -= (damage * damageMod);
        if (damageMod > 1.0f)
        {
            canAwardUphillBattleAchievement = false;
        }
    }

    public int GetCurrentColorIndex()
    {
        return currColorIndexes[currColorEquipped];
    }

    public void DestroySelf()
    {
        AchievementManager.Instance.AwardAchievement(0);
        if (canAwardUphillBattleAchievement)
        {
            AchievementManager.Instance.AwardAchievement(2);
        }
        GameManager.Instance.IncrementWinStreak();
        //Destroy(gameObject);
        StartCoroutine(TransitionToIntermission(2.0f));
    }

    protected IEnumerator TransitionToIntermission(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("intermission");
    }

    public Color GetCurrentColor()
    {
        return currColors[currColorEquipped];
    }

    public float GetCurrentHealth()
    {
        return currHealth;
    }
}
