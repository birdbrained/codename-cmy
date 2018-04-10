using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownDistortion : MonoBehaviour 
{
	[SerializeField]
	private Material mat;
	private Material _mat;
	private float freq;
	private float scale;
	private float speed;
	[SerializeField]
	private float decrementSpeed;

	// Use this for initialization
	void Start() 
	{
		if (mat != null)
		{
			_mat = Material.Instantiate(mat);
			freq = mat.GetFloat("_Freq");
			scale = mat.GetFloat("_Scale");
			speed = mat.GetFloat("_Speed");
			GetComponent<SpriteRenderer>().material = _mat;
		}
	}
	
	void FixedUpdate()
	{
		if (freq > 0.0f)
		{
			freq -= decrementSpeed;
			_mat.SetFloat("_Freq", freq);
		}
		else
		{
			_mat.SetFloat("_Freq", 0.0f);
		}
		if (scale > 0.0f)
		{
			scale -= decrementSpeed;
			_mat.SetFloat("_Scale", scale);
		}
		else
		{
			_mat.SetFloat("_Scale", 0.0f);
		}
		if (speed > 0.0f)
		{
			speed -= decrementSpeed;
			_mat.SetFloat("_Speed", speed);
		}
		else
		{
			_mat.SetFloat("_Speed", 0.0f);
		}
	}
}
