using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOnline : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private float speed;
    public float damageAmount;
    public int colorIndex;
    [SerializeField]
    private SpriteRenderer fireSprite;
    public SpriteRenderer FireSprite
    {
        get
        {
            return fireSprite;
        }
    }
    [SerializeField]
    private bool isLaserSphere;
    private float laserSphereTimer = 0.0f;
    private LineRenderer lr;
    [SerializeField]
    private Collider2D lineCollider;
    public GameObject owner;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaserSphere)
        {
            transform.Translate(Vector3.down * speed);
        }
        else if (isLaserSphere && laserSphereTimer < 0.5f)
        {
            laserSphereTimer += Time.deltaTime;
            transform.Translate(Vector3.down * speed);
        }
        else if (laserSphereTimer >= 0.5f)
        {
            if (lr != null && lineCollider != null)
            {
                lr.material.SetColor("_Color", GameManager.Instance.PlayerColors[colorIndex]);
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, -transform.up * 20 + transform.position);
                lineCollider.enabled = true;
            }
        }
    }

    public void ChangeBulletSpeed(float f)
    {
        speed = f;
    }

    public void ChangeBulletSpeedByPercent(float percent)
    {
        speed *= percent;
    }

    public void SetBulletAttributes(GameObject myOwner, Color myColor, int myColorIndex, float myDamageAmount)
    {
        owner = myOwner;
        if (FireSprite != null)
            FireSprite.material.color = myColor;
        colorIndex = myColorIndex;
        damageAmount = myDamageAmount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != owner)
        {
            if (lineCollider != null)
            {
                lineCollider.enabled = false;
                lineCollider.enabled = true;
            }
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(colorIndex);
            stream.SendNext(owner);
        }
        else
        {
            colorIndex = (int)stream.ReceiveNext();
            owner = (GameObject)stream.ReceiveNext();
        }
    }

    //void OnTrigger
}
