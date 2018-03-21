using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[ExecuteInEditMode]
public class Transition : MonoBehaviour 
{
	[SerializeField]
	private Material transitionMaterial;
	private Material _transM;
	//[SerializeField]
	private string nextLevel;
	private bool isTransitioning = false;
	private bool done = false;
	private float t = 0.0f;

	void Start()
	{
		_transM = Material.Instantiate(transitionMaterial);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, _transM);
	}

	void Update()
	{
		if (isTransitioning && t <= 1.0f)
		{
			_transM.SetFloat("_Cutoff", t);
			t += 0.01f;
		}
		else if (t > 1.0f)
		{
			SceneManager.LoadScene(nextLevel);
			//_transM.SetFloat("_Cutoff", 0.0f);
		}
	}

	public void ExecuteTransition(string lvl)
	{
		isTransitioning = true;
		nextLevel = lvl;
	}

	public void ExecuteRandomMode(string lvl)
	{
		GameManager.Instance.HardMode = 1;
		GameManager.Instance.RandomizePlayerControls = true;
		ExecuteTransition(lvl);
	}

	public void ExecuteTrueYokelMode(string lvl)
	{
		GameManager.Instance.HardMode = 2;
		GameManager.Instance.RandomizePlayerControls = true;
		ExecuteTransition(lvl);
	}
}
