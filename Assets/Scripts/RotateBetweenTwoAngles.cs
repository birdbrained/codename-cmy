using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBetweenTwoAngles : MonoBehaviour
{
    [SerializeField]
    private float maxAngleRange;
    [SerializeField]
    private float angleSubtractionAmount;
    [SerializeField]
    private float speed;
    private bool canRotate = true;
    public bool CanRotate
    {
        get
        {
            return canRotate;
        }
        set
        {
            canRotate = value;
        }
    }
    private float t = 0.0f;
	
	// Update is called once per frame
	void Update()
    {
        if (canRotate)
        {
            t += Time.deltaTime;
            float angle = Mathf.PingPong(t * speed, maxAngleRange) - angleSubtractionAmount;
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
        }
	}
}
