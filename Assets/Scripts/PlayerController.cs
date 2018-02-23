using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	private Rigidbody2D rb;
	private Animator ani;
	[SerializeField]
	private float speed;
	[SerializeField]
	private int maxHealth = 100;
	private int currHealth;
	private bool facingRight;
	//[SerializeField]
	//private Texture2D cursorTexture;
	//public CursorMode cursorMode = CursorMode.Auto;
	//[SerializeField]
	//private Vector2 hotspot = Vector2.zero;
	[SerializeField]
	private GameObject cursorObj;
	private SpriteRenderer cursorSr;
	[SerializeField]
	private GameObject armObj;
	private PointAtObject armPointer;
	[SerializeField]
	private float cursorDistance;
	[SerializeField]
	private GameObject bullet;
	[SerializeField]
	private float fireDelay;
	private float currDelay = 0.0f;

	//data about color
	[SerializeField]
	private Color[] totalColors;
	//private int currentColorIndex;
	private Color[] currentColors = new Color[2];
	private bool primaryColorEquipped = true;
	private short currentColorEquipped = 0;

	//weapon details
	[SerializeField]
	public Weapon[] totalWeapons;
	private Weapon[] currentWeapons = new Weapon[2];
	private Weapon currentWeapon;

	//switching colors
	[SerializeField]
	private SpriteRenderer[] renderersToColor;
	[SerializeField]
	private Image[] imagesToColor;
	[SerializeField]
	private float switchDuration;
	private float t = 0.0f;
	private bool isSwitchingColors = false;


	[SerializeField]
	private Image delayRing;

	public bool controllerConnected;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
		ani = GetComponent<Animator>();

		if (armObj != null)
		{
			armPointer = armObj.GetComponent<PointAtObject>();
		}

		currHealth = maxHealth;
		facingRight = true;
		//Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
		if (cursorObj != null)
		{
			cursorSr = cursorObj.gameObject.GetComponent<SpriteRenderer>();
		}
		currentColors[0] = totalColors[0];
		currentColors[1] = totalColors[1];
		currentWeapons[0] = totalWeapons[0];
		currentWeapons[1] = totalWeapons[1];
		currentWeapon = currentWeapons[0];

		foreach (SpriteRenderer r in renderersToColor)
		{
			r.material.color = currentColors[0];
		}
		foreach (Image im in imagesToColor)
		{
			//im.color = new Color(255, 255, 255, 255);
			im.color = currentColors[0];
		}
		foreach (Weapon w in currentWeapons)
		{
			w.ControllerConnected = controllerConnected;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (delayRing != null)
		{
			delayRing.fillAmount = ((float)currentWeapon.CurrChargeTime / (float)currentWeapon.ChargeTime);
		}
	}

	void FixedUpdate()
	{
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		if (ani != null)
		{
			HandleAnimations(hor, ver);
		}

		rb.velocity = new Vector2(
			Mathf.Lerp(0, hor * speed, 0.8f),
			Mathf.Lerp(0, ver * speed, 0.8f)
		);

		if (cursorObj != null)
		{
			if (controllerConnected)
			{
				float hor2 = Input.GetAxis("Horizontal2");
				float ver2 = Input.GetAxis("Vertical2");

				if (hor2 == 0.0f && ver2 == 0.0f)
				{
					cursorSr.enabled = false;
				} 
				else
				{
					cursorSr.enabled = true;
				}

				cursorObj.transform.localPosition = new Vector3(
					hor2 * cursorDistance + transform.localPosition.x, 
					ver2 * cursorDistance + transform.localPosition.y, 0);
			}
			else
			{
				cursorObj.transform.position = Vector2.Lerp(cursorObj.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1.0f);
				//cursorObj.transform.localPosition = Input.mousePosition;
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
			//firing, please change controller input future matt, fire should not be change color
			if ((!controllerConnected && Input.GetMouseButton(0)) || (controllerConnected && Input.GetAxis("XboxOneTrigger") < 0))
			{
				//Fire();
				currentWeapon = currentWeapons[currentColorEquipped];
				//currentWeapons[currentColorEquipped].BulletColor = currentColors[currentColorEquipped];
				//currentWeapons[currentColorEquipped].Fire();
				//currentWeapons[currentColorEquipped].CurrChargeTime += Time.deltaTime;
				currentWeapon.BulletColor = currentColors[currentColorEquipped];
				currentWeapon.Fire();
				currentWeapon.CurrChargeTime += Time.deltaTime;
			}
			if ((!controllerConnected && Input.GetKeyDown(KeyCode.F)) || (controllerConnected && Input.GetAxis("SwitchColor") == 1))
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
			}
		}

		if ((!controllerConnected && Input.GetMouseButtonUp(0)) || (controllerConnected && Input.GetAxis("XboxOneTrigger") >= 0))
		{
			//Debug.Log("mouse button up");
			//unleash the charge move!
			//if (currentWeapons[currentColorEquipped].CurrChargeTime >= currentWeapons[currentColorEquipped].ChargeTime)
			if (currentWeapon.CurrChargeTime >= currentWeapon.ChargeTime)
			{
				//currentWeapons[currentColorEquipped].ChargeFire();
				currentWeapon.ChargeFire();
			}
			currentWeapon.CurrChargeTime = 0.0f;
		}

		if (currDelay > 0)
		{
			currDelay -= Time.deltaTime;
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
