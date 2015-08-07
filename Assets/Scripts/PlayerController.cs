using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public string unitName;
    public float healthPoints;
    public float maxHealth;
    public float moveSpeed = 5.0f;

    Rigidbody2D myBody;

    public bool attacking;
    public bool jumpCheck;
    bool facingRight = true;

    public LayerMask whatIsGround;
    public Transform groundCheck;

    public GameObject lightAttackArea;
    public GameObject heavyAttackArea;
    public GameObject knockoutAttack;

    public bool attackChain01 = false;
    public bool attackChain02 = false;
    public bool attackChain03 = false;

    public float attackTimeStart = 0.0f;

    void Start()
    {
        maxHealth = 100.0f;
        healthPoints = maxHealth;

        myBody = this.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        jumpCheck = Physics2D.OverlapCircle(groundCheck.position, 0.5f, (whatIsGround));
        CheckPlayerInput();
        ResetAttackTimer();
    }

    public void Move(string dir)
    {
        if (dir == "Left")
        {
            if (facingRight)
            {
                SwapPlayer();
            }
                myBody.velocity = new Vector2((moveSpeed * -1.0f), myBody.velocity.y);
        }
        if (dir == "Right")
        {
            if (!facingRight)
            {
                SwapPlayer();
            }
                myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
        if (dir == "Up")
        {
            if (jumpCheck == true)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 8.0f);
            }
        }
        if (dir == "Zero")
        {
            myBody.velocity = new Vector2(0.0f, myBody.velocity.y);
        }
    }

    public void Attack(string type)
    {
        if (type == "light")
        {
            if (attackChain02)
            {
                Move("zero");
            }
            else if (attackChain02 && attackChain01)
            {
                attackTimeStart = Time.time;
                attackChain03 = true;
                GameObject spawnedAttack = (GameObject)Instantiate(lightAttackArea, (this.gameObject.transform.position + (new Vector3(facingRight ? 0.8f : -0.8f, 0.0f, 0.0f))), this.gameObject.transform.rotation);
                spawnedAttack.transform.parent = this.gameObject.transform;
            }
            else if (attackChain01 && !attackChain02)
            {
                attackTimeStart = Time.time;
                attackChain02 = true;
                GameObject spawnedAttack = (GameObject)Instantiate(lightAttackArea, (this.gameObject.transform.position + (new Vector3(facingRight ? 0.8f : -0.8f, 0.0f, 0.0f))), this.gameObject.transform.rotation);
                spawnedAttack.transform.parent = this.gameObject.transform;
            }
            else if (!attackChain03)
            {
                attackTimeStart = Time.time;
                attackChain01 = true;
                GameObject spawnedAttack = (GameObject)Instantiate(lightAttackArea, (this.gameObject.transform.position + (new Vector3(facingRight ? 0.8f : -0.8f, 0.0f, 0.0f))), this.gameObject.transform.rotation);
                spawnedAttack.transform.parent = this.gameObject.transform;
            }
        }
        else if (type == "heavy")
        {
            if (attackChain01 && !attackChain03)
            {
                attackTimeStart = Time.time;
                attackChain02 = true;
                attackChain03 = true;
                SpawnHeavyAttack();
            }
            else
            {
                attackTimeStart = Time.time;
                ChargeForAttack();
                Invoke("SpawnHeavyAttack", 0.4f);
                attackChain03 = true;
            }
        }
        else if (type == "knockout")
        {
            GameObject spawnedAttack = (GameObject)Instantiate(knockoutAttack, (this.gameObject.transform.position + (new Vector3(facingRight ? 0.8f : -0.8f, 0.0f, 0.0f))), this.gameObject.transform.rotation);
            spawnedAttack.transform.parent = this.gameObject.transform;
        }
    }

    public void ChargeForAttack()
    {
        moveSpeed = 0.2f;
        Invoke("ResetMoveSpeed", 0.4f);
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = 5.0f;
    }

    public void ResetAttackTimer()
    {
        if (attackChain03 || attackChain02 || attackChain01)
        {
            if (Time.time - attackTimeStart > 0.5f)
            {
                attackChain01 = false;
                attackChain02 = false;
                attackChain03 = false;
            }
        }
    }

    public void SpawnHeavyAttack()
    {
        GameObject spawnedAttack = (GameObject)Instantiate(heavyAttackArea, (this.gameObject.transform.position + (new Vector3(facingRight ? 0.8f : -0.8f, 0.0f, 0.0f))), this.gameObject.transform.rotation);
        spawnedAttack.transform.parent = this.gameObject.transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            healthPoints = healthPoints - 10.0f;
        }
    }

    void SwapPlayer()
    {
        facingRight = !facingRight;
        Vector3 theScale = this.transform.localScale;
        theScale.x *= -1;
        this.transform.localScale = theScale;
    }

    void CheckPlayerInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Move("Right");
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move("Left");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move("Up");
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Move("Zero");
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            Attack("light");
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            Attack("heavy");
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            Attack("knockout");
        }
    }
}