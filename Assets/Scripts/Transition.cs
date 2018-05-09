using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[ExecuteInEditMode]
public class Transition : MonoBehaviour 
{
    private static Transition instance;
    public static Transition Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Transition>();
            }
            return instance;
        }
    }

    [SerializeField]
	private Material transitionMaterial;
	private Material _transM;
	//[SerializeField]
	private string nextLevel;
	private bool isTransitioning = false;
	private bool done = false;
	private float t = 0.0f;
    private static int transitionType = 0;
    public int TransitionType
    {
        get
        {
            return transitionType;
        }
        set
        {
            transitionType = value;
        }
    }

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

    public void SetTransitionType(int type)
    {
        transitionType = type;
    }

    public void SetTransitionTypeFromGameManager()
    {
        transitionType = GameManager.Instance.HardMode;
    }

    public void ExecuteTransitionToRandomStage()
    {
        string lvl = "";
        int stageIndex = Random.Range(0, 10);
        switch (stageIndex)
        {
            case 1:
                lvl = "TelephoneRoom";
                break;
            case 2:
                lvl = "StaplerRoom";
                break;
            case 3:
                lvl = "LampRoom";
                break;
            case 4:
                lvl = "LappyRoom";
                break;
            case 5:
                lvl = "ClockRoom";
                break;
            case 6:
                lvl = "SissorsRoom";
                break;
            case 7:
                lvl = "RefresherRoom";
                break;
            case 8:
                lvl = "TVRoom";
                break;
            case 9:
                lvl = "BirdRoom";
                break;
            default:
                lvl = "PrinterRoom";
                break;
        }
        ExecuteTransition(lvl);

    }

	public void ExecuteTransition(string lvl)
	{
		isTransitioning = true;
		nextLevel = lvl;
        switch (transitionType)
        {
            case 1:
            case 2:
                GameManager.Instance.HardMode = transitionType;
                GameManager.Instance.RandomizePlayerControls = true;
                break;
            default:
                GameManager.Instance.HardMode = 0;
                GameManager.Instance.RandomizePlayerControls = false;
                break;
        }
	}

    public void ExecuteNormalMode(string lvl)
    {
        GameManager.Instance.HardMode = 0;
        GameManager.Instance.RandomizePlayerControls = false;
        ExecuteTransition(lvl);
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
