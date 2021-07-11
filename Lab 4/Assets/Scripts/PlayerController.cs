using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameConstants gameConstants;
    public float speed;
    public float upSpeed;
    public float airUpSpeed;
    public float maxSpeed = 100;
    public bool infJump = false;
    private float moveHorizontal;
    private Rigidbody2D monoBody;
    private SpriteRenderer monoSprite;
    private Animator monoAnimator;
    private AudioSource monoAudioSource;
    private BoxCollider2D monoCollider;
    private bool onGroundState = true;
    private bool stopMove = false;
    private bool jumpHold = false;
    private bool faceRight = true;
    private bool dead = false;
    public ParticleSystem dustCloud;
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 60 FPS
        Application.targetFrameRate = 60;
        monoBody = GetComponent<Rigidbody2D>();
        monoSprite = GetComponent<SpriteRenderer>();
        monoAnimator = GetComponent<Animator>();
        monoAudioSource = GetComponent<AudioSource>();
        monoCollider = GetComponent<BoxCollider2D>();
        GameManager.OnPlayerDeath += PlayerDiesSequence;
    }
     
    void FixedUpdate()
    {
        if (dead) monoBody.velocity = Vector2.up * 5.0f;
        else
        {
            if (Input.GetKey("space") && jumpHold)
            {
                monoBody.AddForce(Vector2.up * airUpSpeed, ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown("space") && onGroundState)
            {
                monoBody.AddForce(Vector2.up * upSpeed * gameConstants.jumpBonus, ForceMode2D.Impulse);
                onGroundState = false;
                jumpHold = true;
                monoAnimator.SetBool("onGround", onGroundState);
                PlayJumpSound();
            }

            if (Input.GetKeyUp("space") && jumpHold)
            {
                jumpHold = false;
            }

            // dynamic rigidbody
            moveHorizontal = Input.GetAxis("Horizontal");
            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                if (monoBody.velocity.magnitude < maxSpeed)
                    monoBody.AddForce(movement * speed * gameConstants.speedBonus);
            }

            // left the stop move functionality in here but personally didn't like it
            //if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            //{
            // stop
            //if (onGroundState) monoBody.velocity = Vector2.zero;
            //else stopMove = true;
            //}
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // stops moving when hitting the ground if let go of horizontal movement mid jump (currently disabled)
        if (stopMove) monoBody.velocity = Vector2.zero;
        if (onGroundState == false)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                onGroundState = true;
                dustCloud.Play();
            }
            if (col.gameObject.CompareTag("Obstacle") && Mathf.Abs(monoBody.velocity.y) < 0.01f)
            {
                onGroundState = true;
                dustCloud.Play();
            }
        }
        monoAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a") && faceRight)
        {
            faceRight = false;
            monoSprite.flipX = true;
            if (Mathf.Abs(monoBody.velocity.x) > 10.0)
            {
                monoAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("d") && !faceRight)
        {
            faceRight = true;
            monoSprite.flipX = false;
            if (Mathf.Abs(monoBody.velocity.x) > 10.0)
            {
                monoAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("z"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject);
        }

        if (Input.GetKeyDown("x"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);
        }

        monoAnimator.SetFloat("xSpeed", Mathf.Abs(monoBody.velocity.x));
        if (infJump) onGroundState = true;
    }

    void PlayJumpSound()
    {
        monoAudioSource.PlayOneShot(monoAudioSource.clip);
    }

    void PlayerDiesSequence()
    {
        // Mario dies
        Debug.Log("Mario dies");
        dead = true;
        monoAnimator.SetBool("Dead", true);
        monoCollider.isTrigger = true;
    }
}
