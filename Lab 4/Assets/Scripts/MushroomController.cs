using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private bool startRight = true;
    public float speed;
    public bool bullied = false;

    private Rigidbody2D enemyBody;
    private BoxCollider2D mushCollider;
    private SpriteRenderer mushRenderer;
    private AudioSource mushAudio;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyBody.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        mushRenderer = GetComponent<SpriteRenderer>();
        mushCollider = GetComponent<BoxCollider2D>();
        mushAudio = GetComponent<AudioSource>();
        PickSide();
    }

    void PickSide()
    {
        if (Random.value >= 0.5) startRight = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Obstacle") && Mathf.Abs(enemyBody.velocity.magnitude) < 0.1f)
        {
            if (startRight == true) startRight = false;
            else startRight = true;
        }
        if (col.gameObject.CompareTag("Item") && Mathf.Abs(enemyBody.velocity.magnitude) < 0.1f)
        {
            if (startRight == true) startRight = false;
            else startRight = true;
        }
        if (col.gameObject.CompareTag("Player"))
        {
            bullied = true;
            StartCoroutine(consume());
            mushCollider.isTrigger=true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement;
        float vy = enemyBody.velocity.y;
        float vx = enemyBody.velocity.x;
        if (bullied)
        {
            if (Mathf.Abs(vx) < 5.0f)  bullied = false;
        }
        else
        {
            if (startRight) movement = new Vector2(5, vy);
            else movement = new Vector2(-5, vy);
            enemyBody.velocity = movement;
            vx = enemyBody.velocity.x;
            if (Mathf.Abs(vx) < 2.0f)
            {
                if (startRight == true) startRight = false;
                else startRight = true;
            }
        }
    }

    IEnumerator consume()
    {
        this.mushAudio.PlayOneShot(this.mushAudio.clip);
        int steps = 1000;
        float stepper = 1.0f / (float)steps;

        for (int i = 0; i < steps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - stepper, this.transform.localScale.y - stepper, this.transform.localScale.z);
        }
        for (int i = 0; i < 2 * steps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - stepper, this.transform.localScale.y - stepper, this.transform.localScale.z);
        }
        this.mushRenderer.enabled = false;
        yield break;
    }
}
