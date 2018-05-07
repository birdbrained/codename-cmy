using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	//components needed
	private Rigidbody2D rb;
	private Animator ani;
	[SerializeField]
	private SpriteRenderer bodySprite;

	//basic player variables
	[Range (1,2)]
	public int playerNum = 1;

	[SerializeField]
	private float speed;
	[SerializeField]
	private float maxHealth = 100;
	private float currHealth;
	private bool facingRight;
	public float horizontalInvert = 1.0f;
	public float verticalInvert = 1.0f;
	public float horizontal2Invert = 1.0f;
	public float vertical2Invert = 1.0f;
	public float fireAndDefendInvert = 1.0f;

	//health bar data
	[SerializeField]
	private GameObject healthBar;
	private RectTransform healthBarRect;

	//[SerializeField]
	//private Texture2D cursorTexture;
	//public CursorMode cursorMode = CursorMode.Auto;
	//[SerializeField]
	//private Vector2 hotspot = Vector2.zero;

	//cursor object
	[SerializeField]
	private GameObject cursorObj;
	private SpriteRenderer cursorSr;
	[SerializeField]
	private float cursorDistance;

	//arm object
	[SerializeField]
	private GameObject armObj;
	private PointAtObject armPointer;

	//bullet data
	[SerializeField]
	private GameObject bullet;
	[SerializeField]
	private float fireDelay;
	private float currDelay = 0.0f;

	//shield object
	[SerializeField]
	private GameObject shieldObj;
	[SerializeField]
	private float shieldDistance;
	private bool isDefending;

	//data about color
	[SerializeField]
	private Color[] totalColors;
	//private int currentColorIndex;
	private Color[] currentColors = new Color[2];
	private bool primaryColorEquipped = true;
	private short currentColorEquipped = 0;
    public short CurrentColorEqupped
    {
        get
        {
            return currentColorEquipped;
        }
    }

	//weapon details
	[SerializeField]
	public Weapon[] totalWeapons;
	private Weapon[] currentWeapons = new Weapon[2];
	private Weapon currentWeapon;

	//switching colors
	[SerializeField]
	private SpriteRenderer[] renderersToColor;
    [SerializeField]
    private SpriteRenderer[] baseSpriteRenderers;
	[SerializeField]
	private Image[] imagesToColor;
	[SerializeField]
	private Image[] imagesToColorSecondary;
	[SerializeField]
	private float switchDuration;
	private float t = 0.0f;
	private bool isSwitchingColors = false;

    //immortal variables
	private bool immortal = false;
	[SerializeField]
	private float immortalTime;
    private float flashColorTime;
    [SerializeField]
    private float flashSpeed;
    private bool increaseColor = true;

	//sound effect manager
	/*[SerializeField]
	private AudioClip fireClip;
	private AudioSource fireAudio;*/

	[SerializeField]
	private Image delayRing;

	public bool controllerConnected;

	//controls!!!!!
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public string horizontalAxis2 = "Horizontal2";
	public string verticalAxis2 = "Vertical2";
	public string fireAxis = "XboxOneTrigger";
	public string switchColorAxis = "SwitchColor";
	public int mouseFireButton = 0;
	public int mouseDefendButton = 1;
    public bool SwapFireAndDefend = false;
	[SerializeField]
	private ControllerInfo controllerInfo;
	private Dictionary<string, string> keyBindings = new Dictionary<string, string>();

    //particle systems
    [SerializeField]
    private ParticleSystem partsSwitchColor;
    ParticleSystem.MainModule partsSwitchColorMM;

    //sound systems
    [SerializeField]
    private AudioSource[] audioSources;

	public bool IsDead
	{
		get
		{
			return currHealth <= 0.0f;
		}
	}
    private bool isRevivingPlayer = false;
    public bool IsRevivingPlayer
    {
        get
        {
            return isRevivingPlayer;
        }
        set
        {
            isRevivingPlayer = value;
        }
    }

	/*void Awake()
	{
		if (fireClip != null)
			fireAudio = AddAudio(fireClip, false, false, 1);
	}*/

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		ani = GetComponent<Animator>();

		if (armObj != null)
		{
			armPointer = armObj.GetComponent<PointAtObject>();
		}
		if (controllerInfo != null)
		{
			LoadDefaultControls();
		}

		currHealth = maxHealth;
		facingRight = true;
		//Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
		if (cursorObj != null)
		{
			cursorSr = cursorObj.gameObject.GetComponent<SpriteRenderer>();
		}
		if (shieldObj != null)
		{
			shieldObj.SetActive(false);
		}
		if (healthBar != null)
		{
			healthBarRect = healthBar.GetComponent<RectTransform>();
		}
		isDefending = false;

		//color setting
		/*currentColors[0] = totalColors[0];
		currentColors[1] = totalColors[1];
		currentWeapons[0] = totalWeapons[0];
		currentWeapons[1] = totalWeapons[1];*/

		/*if (playerNum == 1)
		{
			currentColors[0] = totalColors[GameManager.Instance.playerOnePrimaryColorIndex];
			currentColors[1] = totalColors[GameManager.Instance.playerOneSecondaryColorIndex];
			currentWeapons[0] = totalWeapons[GameManager.Instance.playerOnePrimaryWeaponIndex];
			currentWeapons[1] = totalWeapons[GameManager.Instance.playerOneSecondaryWeaponIndex];
			Debug.Log("Player 1 received: " + GameManager.Instance.playerOnePrimaryColorIndex.ToString() + " " +
			GameManager.Instance.playerOneSecondaryColorIndex.ToString() + " " +
			GameManager.Instance.playerOnePrimaryWeaponIndex.ToString() + " " +
			GameManager.Instance.playerOneSecondaryWeaponIndex.ToString());
		} 
		else if (playerNum == 2)
		{
			currentColors[0] = totalColors[GameManager.Instance.playerTwoPrimaryColorIndex];
			currentColors[1] = totalColors[GameManager.Instance.playerTwoSecondaryColorIndex];
			currentWeapons[0] = totalWeapons[GameManager.Instance.playerTwoPrimaryWeaponIndex];
			currentWeapons[1] = totalWeapons[GameManager.Instance.playerTwoSecondaryWeaponIndex];
		}*/

		//currentWeapon = currentWeapons[0];
		//AssignWeaponColorIndexes();

		foreach (SpriteRenderer r in renderersToColor)
		{
			r.material.color = currentColors[0];
		}
		foreach (Image im in imagesToColor)
		{
			//im.color = new Color(255, 255, 255, 255);
			im.color = currentColors[0];
		}
		//foreach (Weapon w in currentWeapons)
		for (int i = 0; i < totalWeapons.Length; i++)
		{
			//w.ControllerConnected = controllerConnected;
			totalWeapons[i].BulletColor = totalColors[i];
		}

		//add input axises to dictionary
		keyBindings.Add("horizontalAxis", horizontalAxis);
		keyBindings.Add("verticalAxis", verticalAxis);
		keyBindings.Add("horizontalAxis2", horizontalAxis2);
		keyBindings.Add("verticalAxis2", verticalAxis2);
		keyBindings.Add("fireAxis", fireAxis);
		keyBindings.Add("switchColorAxis", switchColorAxis);

        if (partsSwitchColor != null)
            partsSwitchColorMM = partsSwitchColor.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (delayRing != null && currentWeapon != null)
		{
			delayRing.fillAmount = ((float)currentWeapon.CurrChargeTime / (float)currentWeapon.ChargeTime);
		}
		if (healthBar != null && healthBarRect != null)
		{
			healthBarRect.sizeDelta = new Vector2(100.0f, ((float)currHealth / (float)maxHealth) * 95.0f);
			//healthBarRect.rect.height = ((float)currHealth / (float)maxHealth) * 100.0f;
			//Debug.Log("Derp!");
		}

		//currHealth--;
	}

	void FixedUpdate()
	{
		float hor = Input.GetAxis(horizontalAxis);
		float ver = Input.GetAxis(verticalAxis);
		float hor2 = Input.GetAxis(horizontalAxis2);
		float ver2 = Input.GetAxis(verticalAxis2);
        float fire = Input.GetAxis(fireAxis);
        if (SwapFireAndDefend)
            fire *= -1.0f;

        if (IsDead || isRevivingPlayer)
        {
            hor = ver = hor2 = ver2 = 0;
        }

		if (ani != null)
		{
			HandleAnimations(hor, ver);
		}

		rb.velocity = new Vector2(
			Mathf.Lerp(0, hor * speed * horizontalInvert, 0.8f),
			Mathf.Lerp(0, ver * speed * verticalInvert, 0.8f)
		);

		if (cursorObj != null)
		{
			if (controllerConnected)
			{
				if (hor2 == 0.0f && ver2 == 0.0f)
				{
					cursorSr.enabled = false;
				} 
				else
				{
					cursorSr.enabled = true;
				}
					
				cursorObj.transform.localPosition = new Vector3(
					hor2 * horizontal2Invert * cursorDistance + transform.localPosition.x, 
					ver2 * vertical2Invert * cursorDistance + transform.localPosition.y, 0);
			}
			else
			{
				cursorObj.transform.position = Vector2.Lerp(cursorObj.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1.0f);
				//cursorObj.transform.localPosition = Input.mousePosition;
			}
		}

		if (shieldObj != null)
		{
            //right click or left trigger to defend
            if (!IsDead)
            {
                if ((!controllerConnected && Input.GetMouseButton(mouseDefendButton)) || controllerConnected && fire > 0.5)
                {
                    shieldObj.SetActive(true);
                    isDefending = true;
                    currentWeapon.CurrChargeTime = 0.0f;

                    //use the inputs from the right stick if you are using a controller
                    if (controllerConnected)
                    {
                        if (Input.GetAxis(horizontalAxis2) == 0 && Input.GetAxis(verticalAxis2) == 0)
                        {
                            shieldObj.SetActive(false);
                            isDefending = false;
                        }
                        else
                        {
                            shieldObj.transform.localPosition = new Vector3(
                                hor2 * horizontal2Invert * shieldDistance + transform.localPosition.x,
                                ver2 * vertical2Invert * shieldDistance + transform.localPosition.y, 0);
                        }
                    }
                    //otherwise need to figure the angle from how the cursor relates to the player
                    else
                    {
                        float curX = cursorObj.transform.localPosition.x - transform.localPosition.x;
                        float curY = cursorObj.transform.localPosition.y - transform.localPosition.y;
                        float c = Mathf.Sqrt(curX * curX + curY * curY);
                        //float c = curX * curX + curY * curY;
                        float shieldX = curX / c;
                        float shieldY = curY / c;

                        shieldObj.transform.localPosition = new Vector3(
                            shieldX * shieldDistance + transform.localPosition.x * horizontal2Invert,
                            shieldY * shieldDistance + transform.localPosition.y * vertical2Invert, 0);
                    }
                }
                else
                {
                    shieldObj.SetActive(false);
                    isDefending = false;
                }
            }
		}

		ChangeDirection(hor);
		if (armPointer != null)
		{
			armPointer.FacingRight = facingRight;
		}

		//fire a projectile
		if (!isSwitchingColors)
		{
			//firing, please change controller input future matt, fire should not be change color <--done!
			if ((!controllerConnected && Input.GetMouseButton(mouseFireButton)) || (controllerConnected && fire < 0))
			{
				//Fire();
				if (!isDefending && !IsDead)
				{
					FireWeapon();
				}
			}
			//if ((!controllerConnected && Input.GetKeyDown(KeyCode.F)) || (controllerConnected && Input.GetAxis(switchColorAxis) == 1))
			if (Input.GetAxis(switchColorAxis) == 1 && !IsDead)
			{
				isSwitchingColors = true;
			}
		}
		//or else if you are currently switching colors, switch colors
		else
		{
			for (int i = 0; i < renderersToColor.Length; i++)
			{
				if (currentColorEquipped == 0)
				{
					renderersToColor[i].material.color = Color.Lerp(currentColors[0], currentColors[1], t);
				}
				else
				{
					renderersToColor[i].material.color = Color.Lerp(currentColors[1], currentColors[0], t);
				}
			}
			for (int i = 0; i < imagesToColor.Length; i++)
			{
				if (currentColorEquipped == 0)
				{
					imagesToColor[i].color = Color.Lerp(currentColors[0], currentColors[1], t);
				}
				else
				{
					imagesToColor[i].color = Color.Lerp(currentColors[1], currentColors[0], t);
				}
			}
			for (int i = 0; i < imagesToColorSecondary.Length; i++)
			{
				if (currentColorEquipped == 0)
				{
					imagesToColorSecondary[i].color = Color.Lerp(currentColors[1], currentColors[0], t);
				}
				else
				{
					imagesToColorSecondary[i].color = Color.Lerp(currentColors[0], currentColors[1], t);
				}
			}


			if (t < 1.0f)
			{
				t += Time.deltaTime / switchDuration;
			}
			else
			{
				isSwitchingColors = false;
				t = 0.0f;
				if (currentColorEquipped == 0)
					currentColorEquipped = 1;
				else
					currentColorEquipped = 0;

                if (partsSwitchColor != null)
                {
                    partsSwitchColorMM.startColor = new Color(
                        currentColors[currentColorEquipped].r,
                        currentColors[currentColorEquipped].g,
                        currentColors[currentColorEquipped].b,
                        0.5f);
                    partsSwitchColor.Play();
                }
			}
		}

		if ((!controllerConnected && Input.GetMouseButtonUp(0)) || (controllerConnected && fire >= 0))
		{
			//Debug.Log("mouse button up");
			//unleash the charge move!
			//if (currentWeapons[currentColorEquipped].CurrChargeTime >= currentWeapons[currentColorEquipped].ChargeTime)
			if (currentWeapon.CurrChargeTime >= currentWeapon.ChargeTime && !isDefending)
			{
				//currentWeapons[currentColorEquipped].ChargeFire();
				currentWeapon.ChargeFire("player_bullet");
			}
			currentWeapon.CurrChargeTime = 0.0f;
		}

		if (currDelay > 0)
		{
			currDelay -= Time.deltaTime;
		}
	}

	void FireWeapon()
	{
		currentWeapon = currentWeapons[currentColorEquipped];
		//currentWeapons[currentColorEquipped].BulletColor = currentColors[currentColorEquipped];
		//currentWeapons[currentColorEquipped].Fire();
		//currentWeapons[currentColorEquipped].CurrChargeTime += Time.deltaTime;
		currentWeapon.BulletColor = currentColors[currentColorEquipped];
		currentWeapon.Fire("player_bullet");

		/*if (fireAudio != null)
			fireAudio.Play();*/
		currentWeapon.CurrChargeTime += Time.deltaTime;
	}

	void AssignWeaponColorIndexes()
	{
		if (playerNum == 1)
		{
			currentWeapons[0].bulletColorIndex = GameManager.Instance.playerOnePrimaryColorIndex;
			currentWeapons[1].bulletColorIndex = GameManager.Instance.playerOneSecondaryColorIndex;
		}
		else if (playerNum == 2)
		{
			currentWeapons[0].bulletColorIndex = GameManager.Instance.playerTwoPrimaryColorIndex;
			currentWeapons[1].bulletColorIndex = GameManager.Instance.playerTwoSecondaryColorIndex;
		}
	}

	void HandleAnimations(float hor, float ver)
	{
		ani.SetFloat("walkSpeed", Mathf.Abs(hor) + Mathf.Abs(ver));

		if (ver > 0)
		{
			ani.SetLayerWeight(1, 1);
		}
		else
		{
			ani.SetLayerWeight(1, 0);
		}
	}

	public void LoadDefaultControls()
	{
		/*if (playerNum == 1)
			controllerInfo = GameManager.Instance.Player1ControllerInfo;
		else
			controllerInfo = GameManager.Instance.Player2ControllerInfo;*/
		
		horizontalAxis = controllerInfo.horAxis;
		verticalAxis = controllerInfo.verAxis;
		horizontalAxis2 = controllerInfo.horAxis2;
		verticalAxis2 = controllerInfo.verAxis2;
		fireAxis = controllerInfo.fireAxis;
		switchColorAxis = controllerInfo.switchColor;
		mouseFireButton = controllerInfo.mouseFireButton;
		mouseDefendButton = controllerInfo.mouseDefendButton;
		controllerConnected = controllerInfo.isController;
		horizontalInvert = controllerInfo.horizontalInvert;
		verticalInvert = controllerInfo.verticalInvert;
		horizontal2Invert = controllerInfo.horizontal2Invert;
		vertical2Invert = controllerInfo.vertical2Invert;
		fireAndDefendInvert = controllerInfo.fireAndDefendInvert;
	}

	public void ChangeDirection(float hor)
	{
		if (hor > 0 && !facingRight || hor < 0 && facingRight)
		{
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}

	public void SetCurrentColors(int primary, int secondary)
	{
		currentColors[0] = totalColors[primary];
		currentColors[1] = totalColors[secondary];
		Debug.Log("Player (" + playerNum.ToString() + ") colors are (" + primary.ToString() + secondary.ToString());
		currentColorEquipped = 0;
		for (int i = 0; i < renderersToColor.Length; i++)
		{
			renderersToColor[i].material.color = currentColors[0];
		}
		for (int i = 0; i < imagesToColor.Length; i++)
		{
			imagesToColor[i].color = currentColors[0];
		}
		for (int i = 0; i < imagesToColorSecondary.Length; i++)
		{
			imagesToColorSecondary[i].color = currentColors[1];
		}
	}

	public void SetCurrentWeapons(int primary, int secondary)
	{
		currentWeapons[0] = totalWeapons[primary];
		currentWeapons[1] = totalWeapons[secondary];
		currentWeapon = currentWeapons[0];
		AssignWeaponColorIndexes();
	}

	private IEnumerator IndicateImmortality()
	{
		while (immortal)
		{
			//Debug.Log("immortal");
			//bodySprite.enabled = false;
			//yield return new WaitForSeconds(0.1f);
			//bodySprite.enabled = true;
			//yield return new WaitForSeconds(0.1f);
            if (increaseColor)
            {
                flashColorTime += flashSpeed;
                for (int i = 0; i < baseSpriteRenderers.Length; i++)
                {
                    baseSpriteRenderers[i].material.SetFloat("_Override", flashColorTime);
                }
                yield return new WaitForSeconds(0.1f);
                if (flashColorTime >= 1.0f)
                {
                    flashColorTime = 1.0f;
                    //yield return new WaitForSeconds(0.1f);
                    increaseColor = false;
                }
            }
            else
            {
                flashColorTime -= flashSpeed;
                for (int i = 0; i < baseSpriteRenderers.Length; i++)
                {
                    baseSpriteRenderers[i].material.SetFloat("_Override", flashColorTime);
                }
                yield return new WaitForSeconds(0.1f);
                if (flashColorTime <= 0.0f)
                {
                    flashColorTime = 0.0f;
                    //yield return new WaitForSeconds(0.1f);
                    increaseColor = true;
                }
            }
		}
        if (!immortal)
        {
            increaseColor = true;
            flashColorTime = 0.0f;
            for (int i = 0; i < baseSpriteRenderers.Length; i++)
            {
                baseSpriteRenderers[i].material.SetFloat("_Override", flashColorTime);
            }
        }
	}

	public IEnumerator TakeDamage(float damage, string dealer)
	{
		if (!immortal && damage != 0.0f)
		{
			currHealth -= damage;

			if (!IsDead)
			{
				//not dead
				ani.SetTrigger("damage");
				immortal = true;
				StartCoroutine(IndicateImmortality());
				yield return new WaitForSeconds(immortalTime);
				immortal = false;
			}
			else
			{
                //ya dead
                Instantiate(ParticleManager.Instance.DownParticle, transform.position, transform.rotation);
                ani.SetBool("dead", true);
                ani.SetTrigger("die");

                //GameManager.Instance.rmInstance.gameObject.SetActive(true);

                if (GameManager.Instance.rmInstance.DeadPlayer == null)
                {
                    GameManager.Instance.rmInstance.DeadPlayer = this;
                    GameManager.Instance.rmInstance.transform.position = new Vector3
                    (
                        transform.position.x,
                        transform.position.y,
                        GameManager.Instance.rmInstance.transform.position.z
                    );
                    if (playerNum == 1)
                    {
                        GameManager.Instance.rmInstance.SubmitAxis = "Submit_P2";
                    }
                    else
                    {
                        GameManager.Instance.rmInstance.SubmitAxis = "Submit";
                    }
                }
                else if (GameManager.Instance.rmInstance.DeadPlayer != this)
                {
                    //both players are dead, game over!
                    Debug.Log("Game over!");
                    //ParticleManager.Instance.GameOverParticle.SetActive(true);
                    Instantiate(ParticleManager.Instance.GameOverParticle, new Vector3(0, 9, 0), Quaternion.identity);
                }
			}
		}
	}

    public void TakeDamageWrapper(float damage, float damageMod, string dealer)
    {
        StartCoroutine(TakeDamage(damage * damageMod, dealer));
    }

    public void RevivePlayer(float reviveAmount)
    {
        currHealth = reviveAmount;
        ani.SetBool("dead", false);
        //immortal = true;
        //StartCoroutine(IndicateImmortality());
        //yield return new WaitForSeconds(immortalTime);
        //immortal = false;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "enemy_bullet" || other.gameObject.tag == "player_bullet" || other.gameObject.tag == "laser" || other.gameObject.tag == "enemy_laser")
		{
			Bullet _bullet = other.gameObject.GetComponent<Bullet>();
            if (_bullet != null && _bullet.owner != gameObject)
            {
                if (!IsDead)
                {
                    float damageMod;
                    if (playerNum == 1)
                    {
                        if (currentColorEquipped == 0)
                            damageMod = GameManager.Instance.DamageModifier(_bullet.colorIndex, GameManager.Instance.playerOnePrimaryColorIndex);
                        else
                            damageMod = GameManager.Instance.DamageModifier(_bullet.colorIndex, GameManager.Instance.playerOneSecondaryColorIndex);
                    }
                    else
                    {
                        if (currentColorEquipped == 0)
                            damageMod = GameManager.Instance.DamageModifier(_bullet.colorIndex, GameManager.Instance.playerTwoPrimaryColorIndex);
                        else
                            damageMod = GameManager.Instance.DamageModifier(_bullet.colorIndex, GameManager.Instance.playerTwoSecondaryColorIndex);
                    }

                    //explosion-friendly-fire attack
                    //Debug.Log("tag: (" + other.gameObject.tag + ") immortal: (" + immortal.ToString() + ") damageMod: (" + damageMod.ToString() + ")");
                    if ((other.gameObject.tag == "player_bullet" || other.gameObject.tag == "laser") && !immortal && damageMod > 1.0f)
                    {
                        Debug.Log("Same player hurts! A Splode!");
                        GameObject explosionObj = Instantiate(GameManager.Instance.ExplosionObj, transform.position, transform.rotation);
                        DamageInRadius explosion = explosionObj.GetComponent<DamageInRadius>();
                        explosion.SetupExplosion(gameObject, -1.0f, -1.0f, GetCurrentColorIndex(), currentColors[currentColorEquipped]);
                        explosion.SetupExplosionColor(GetCurrentColor());
                    }

                    StartCoroutine(TakeDamage(_bullet.damageAmount * damageMod, other.gameObject.name));

                    GameObject parts;

                    if (damageMod == 2.0f)
                        parts = Instantiate(ParticleManager.Instance.CritParticle, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
                    else if (damageMod == 0.5f)
                        parts = Instantiate(ParticleManager.Instance.ResistParticle, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
                    else
                        parts = Instantiate(ParticleManager.Instance.WhiffParticle, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);

                    parts.GetComponent<SpriteRenderer>().material.color = currentColors[currentColorEquipped];
                }
                else
                {
                    //play a cute squeeky noise
                    ani.SetTrigger("squeak");
                    StartCoroutine(ResetTriggerAfterTime("squeak", 0.1f));
                }

                if (!(other.gameObject.tag == "laser" || other.gameObject.tag == "enemy_laser"))
                    Destroy(other.gameObject);
            }
		}
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController _otherPlayer = other.gameObject.GetComponent<PlayerController>();
            if (_otherPlayer.IsDead)
            {
                GameManager.Instance.rmInstance.gameObject.SetActive(true);
                isRevivingPlayer = true;
            }
        }
    }

    private IEnumerator ResetTriggerAfterTime(string trigger, float time)
    {
        yield return new WaitForSeconds(time);
        ani.ResetTrigger(trigger);
    }

    public int GetCurrentColorIndex()
    {
        int num;

        if (playerNum == 1)
        {
            if (currentColorEquipped == 0)
                num = GameManager.Instance.playerOnePrimaryColorIndex;
            else
                num = GameManager.Instance.playerOneSecondaryColorIndex;
        }
        else
        {
            if (currentColorEquipped == 0)
                num = GameManager.Instance.playerTwoPrimaryColorIndex;
            else
                num = GameManager.Instance.playerTwoSecondaryColorIndex;
        }

        return num;
    }

    public Color GetCurrentColor()
    {
        return currentColors[currentColorEquipped];
    }

    /**
     * 0 -> rubber duck squeak
     */ 
    public void PlaySomeSound(int whichSound)
    {
        //aud.Play();
        switch (whichSound)
        {
            case 0:
                audioSources[whichSound].Play();
                break;
            default:
                break;
        }
    }


	/*public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
	{
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}*/

	/*public void Fire()
	{
		if (currDelay <= 0)
		{
			GameObject _bullet = Instantiate(bullet, transform.localPosition, transform.rotation);
			//_bullet.transform.LookAt(cursorObj.transform);
			Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
			cursorPos.z = 5.23f;
			Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
			cursorPos.x = cursorPos.x - objectPos.x;
			cursorPos.y = cursorPos.y - objectPos.y;
			float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg;
			_bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
			_bullet.GetComponent<Bullet>().FireSprite.material.color = currentColors[currentColorEquipped];

			//Debug.Log("mousePosition: " + Input.mousePosition);
			//Debug.Log("cursorPos: " + Camera.main.WorldToScreenPoint(cursorObj.transform.position));
			currDelay = fireDelay;
		}
	}*/
}
