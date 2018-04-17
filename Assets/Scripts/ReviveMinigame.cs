using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveMinigame : MonoBehaviour
{
    [SerializeField]
    private GameObject spinner;
    [SerializeField]
    private int rotateSpeed;
    private bool canRotate = false;
    private bool canGiveHeals = true;
    [SerializeField]
    private float baseHealAmount = 50.0f;

    private PlayerController deadPlayer;
    public PlayerController DeadPlayer
    {
        get
        {
            return deadPlayer;
        }
        set
        {
            deadPlayer = value;
        }
    }
    private string submitAxis = "Submit";
    public string SubmitAxis
    {
        get
        {
            return submitAxis;
        }
        set
        {
            submitAxis = value;
        }
    }

    void Start()
    {
        //Instance = this;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        canRotate = true;
        canGiveHeals = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (canRotate)
        {
            spinner.transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime, Space.World);
        }

        if (Input.GetAxis(submitAxis) > 0 && canGiveHeals)
        {
            canRotate = false;
            //Debug.Log("rotation is: " + spinner.transform.rotation.eulerAngles);
            float offset = Mathf.Abs(spinner.transform.rotation.eulerAngles.z - 180);
            float offsetPercentage = 1.0f - (offset / 180.0f);
            Debug.Log("distance from 180 degrees is: " + offset + ", percent close: " + offsetPercentage);

            //heal the other player
            float finalHeals = baseHealAmount * offsetPercentage;
            deadPlayer.RevivePlayer(finalHeals);
            canGiveHeals = false;
            deadPlayer = null;

            PlayerController[] players = FindObjectsOfType<PlayerController>();
            foreach (PlayerController pc in players)
            {
                pc.IsRevivingPlayer = false;
            }

            gameObject.SetActive(false);
        }
	}
}
