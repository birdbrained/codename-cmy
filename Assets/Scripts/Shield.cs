using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour 
{
	[SerializeField]
	private string ignoreCollisionTag;
	[SerializeField]
	private string bulletTagToDefend;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("Collided with object, tag is: " + other.gameObject.tag);
		/*if (other.gameObject.tag.CompareTo(ignoreCollisionTag) == 0)
		{
			//do nothing
			Debug.Log("ignore collsion");
		}*/
		if (other.gameObject.tag.CompareTo(bulletTagToDefend) == 0)
		{
			//Debug.Log("delete it!");
			GameObject.Destroy(other.gameObject);
		}
	}
}
