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
	[SerializeField]
	private float cursorDistance;

	public bool controllerConnected;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();

		facingRight = true;
		//Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
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
				if (facingRight)
					cursorObj.transform.localPosition = new Vector3(Input.GetAxis("Horizontal2") * cursorDistance, Input.GetAxis("Vertical2") * cursorDistance, 0);
				else
					cursorObj.transform.localPosition = new Vector3(Input.GetAxis("Horizontal2") * cursorDistance * -1, Input.GetAxis("Vertical2") * cursorDistance, 0);
			}
			else
			{
				cursorObj.transform.position = Vector2.Lerp(cursorObj.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1.0f);
				//cursorObj.transform.localPosition = Input.mousePosition;
			}
		}

		ChangeDirection(hor);
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
}
