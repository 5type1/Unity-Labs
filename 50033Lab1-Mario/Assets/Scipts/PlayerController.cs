using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float iniSpeed;
    public float upSpeed;
    public float airUpSpeed;
    public float respawnX = -7.0f;
    public float respawnY = -2.86f;
    public float maxSpeed = 100;

    private float moveHorizontal;
    private Rigidbody2D monoBody;
    private SpriteRenderer monoSprite;
    private bool onGroundState = true;
    private bool stopMove = false;
    private bool jumpHold = false;
    private bool faceRight = true;

    public Transform enemyLocation;
    public Text scoreText;
    private int score = 0;
    private bool countScoreState = false;

    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        monoBody = GetComponent<Rigidbody2D>();
        monoSprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey("space") && jumpHold)
        {
            monoBody.AddForce(Vector2.up * airUpSpeed, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown("space") && onGroundState)
        {
            monoBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpHold = true;
            countScoreState = true;
        }

        if(Input.GetKeyUp("space") && jumpHold)
        {
            jumpHold = false;
        }

        // dynamic rigidbody
        moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (monoBody.velocity.magnitude < maxSpeed)
            {
                if (monoBody.velocity.magnitude < iniSpeed) monoBody.velocity = movement * iniSpeed;
                monoBody.AddForce(movement * speed);
            }
        }

        // left the stop move functionality in here but personally didn't like it
        //if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        //{
            // stop
            //if (onGroundState) monoBody.velocity = Vector2.zero;
            //else stopMove = true;
        //}
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // stops moving when hitting the ground if let go of horizontal movement mid jump (currently disabled)
        if (col.gameObject.CompareTag("Ground"))
        {
            if (stopMove) monoBody.velocity = Vector2.zero;
            onGroundState = true;
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            dead = true;
            Time.timeScale = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a") && faceRight)
        {
            faceRight = false;
            monoSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRight)
        {
            faceRight = true;
            monoSprite.flipX = false;
        }

        if (Input.anyKey && dead)
        {
            Debug.Log("Restart!");
            dead = false;
            Vector2 startposition = new Vector2(respawnX, respawnY);
            monoBody.MovePosition(startposition);
            score = 0;
            Time.timeScale = 1.0f;
        }

        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }
    }
}
