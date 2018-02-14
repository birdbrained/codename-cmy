using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private Rigidbody2D rb;
	[SerializeField]
	private float speed;
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

	//switching colors
	[SerializeField]
	private SpriteRenderer[] renderers;
	[SerializeField]
	private float switchDuration;
	private float t = 0.0f;
	private bool isSwitchingColors = false;

	public bool controllerConnected;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();

		facingRight = true;
		//Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
		if (cursorObj != null)
		{
			cursorSr = cursorObj.gameObject.GetComponent<SpriteRenderer>();
		}
		currentColors[0] = totalColors[0];
		currentColors[1] = totalColors[1];
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void FixedUpdate()
	{
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

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

		//fire a projectile
		if (!isSwitchingColors)
		{
			if (Input.GetAxis("SwitchColor") == 1 || Input.GetMouseButtonDown(0))
			{
				Fire();
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				isSwitchingColors = true;
			}
		}
		//or else if you are currently switching colors, switch colors
		else
		{
			for (int i = 0; i < renderers.Length; i++)
			{
				if (currentColorEquipped == 0)
				{
					renderers[i].material.color = Color.Lerp(currentColors[0], currentColors[1], t);
				}
				else
				{
					renderers[i].material.color = Color.Lerp(currentColors[1], currentColors[0], t);
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

		if (currDelay > 0)
		{
			currDelay -= Time.deltaTime;
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

	public void Fire()
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
	}
}
