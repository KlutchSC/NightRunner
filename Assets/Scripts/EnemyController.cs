using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    int maxHealth;
    public int curHealth;

    public bool stunned;
    public bool facingRight = true;
    
    public const float moveSpeed = 3.7f;

    public GameObject player;

    Rigidbody2D myBody;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stunned = false;
        maxHealth = 100;
        curHealth = maxHealth;
        myBody = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	void Update () 
    {
        CheckHealth();
        SeekPlayer();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "lightAttack")
        {
            TakeDamage(12);
        }
        if (other.gameObject.tag == "heavyAttack")
        {
            TakeDamage(26);
        }
        if (stunned)
        {
            if (other.gameObject.tag == "KnockOut")
            {
                TakeDamage(100);
            }
        }
    }

    public void TakeDamage(int dmgValue)
    {
        stunned = true;
        Invoke("RemoveStun", 0.2f);
        curHealth = curHealth - dmgValue;
    }

    public void CheckHealth()
    {
        if (!stunned)
        {
            if (curHealth <= 0)
            {
                stunned = true;
                Downed();
                Debug.Log("Im downed");
            }
        }
        else
        {
            if (curHealth <= -100)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    public void RemoveStun()
    {
        stunned = false;
    }

    public void Downed()
    {
        transform.localScale = new Vector3(1.0f, 1.2f, 1.0f);
        Invoke("GetBackUp", 3.0f);
    }

    public void GetBackUp()
    {
        transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
        curHealth = maxHealth;
        stunned = false;
        Debug.Log("I'm up!");
    }

    public void SeekPlayer()
    {
        if (!stunned)
        {
            if ((Mathf.Abs(player.transform.position.x - transform.position.x)) < 10.0f)
            {
                if ((Mathf.Abs(player.transform.position.x - transform.position.x)) < 1.2f)
                {
                    myBody.velocity = new Vector2(0.0f, 0.0f);
                }
                else
                {
                    if (player.transform.position.x < transform.position.x)
                    {
                        facingRight = false;
                        myBody.velocity = new Vector2((moveSpeed * -1), myBody.velocity.y);
                    }
                    else
                    {
                        facingRight = true;
                        myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
                    }
                }
            }
        }
    }
}
