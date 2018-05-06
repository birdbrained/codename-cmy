using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaplerMinion : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer glow;
    [SerializeField]
    private float attackRange = 200.0f;
    private GameObject currTarget;
    private PlayerController[] targets;
    [SerializeField]
    private Transform bulletSpawnPos;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float normalDamage;
    [SerializeField]
    private float attackDelayTime = 4.0f;
    private float attackTimer = 0.0f;
    public bool Attacking { get; set; }
    private Color myColor;
    private int myColorIndex;

    // Use this for initialization
    void Start ()
    {
        targets = FindObjectsOfType<PlayerController>();

        Attacking = false;
	}
	
	// Update is called once per frame
	void Update()
    {
        float maxDistance = 1000.0f;
        GameObject myTarget = null;

		foreach (PlayerController player in targets)
        {
            float distance = Vector3.Distance(player.gameObject.transform.position, transform.position);
            if (distance < maxDistance)
            {
                myTarget = player.gameObject;
            }
        }

        if (myTarget != null)
            currTarget = myTarget;
	}

    void FixedUpdate()
    {
        if (!Attacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelayTime)
            {
                attackTimer = 0.0f;
                //Attacking = true;
                Attack();
            }
        }
    }

    void Attack()
    {
        GameObject _bullet = Instantiate(bulletObj, bulletSpawnPos.position, transform.rotation);
        Bullet _bulletComponent = _bullet.GetComponent<Bullet>();
        _bullet.tag = "enemy_bullet";

        Vector3 cursorPos = Camera.main.WorldToScreenPoint(currTarget.transform.position);
        cursorPos.z = 5.23f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
        cursorPos.x = cursorPos.x - objectPos.x;
        cursorPos.y = cursorPos.y - objectPos.y;
        float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg;
        _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
        _bulletComponent.SetBulletAttributes(gameObject, myColor, myColorIndex, normalDamage);
    }

    public void SetupMinion(Color c, int index, float damage)
    {
        myColor = c;
        myColorIndex = index;
        if (damage >= 0.0f)
            normalDamage = damage;
        glow.material.color = c;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player_bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "laser")
        {
            Destroy(gameObject);
        }
    }
}
