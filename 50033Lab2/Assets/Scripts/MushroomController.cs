using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private bool startRight = true;
    public float speed;
    public bool bullied = false;

    private Rigidbody2D enemyBody;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyBody.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        PickSide();
    }

    void PickSide()
    {
        if (Random.value >= 0.5) startRight = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Obstacle") && Mathf.Abs(enemyBody.velocity.x) < 0.1f)
        {
            if (startRight == true) startRight = false;
            else startRight = true;
        }
        if (col.gameObject.CompareTag("Item") && Mathf.Abs(enemyBody.velocity.x) < 0.1f)
        {
            if (startRight == true) startRight = false;
            else startRight = true;
        }
        if (col.gameObject.CompareTag("Player"))
        {
            bullied = true;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
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
            if (Mathf.Abs(vx) < 2.0f) startRight = !startRight;
        }
    }
}
