using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private bool hitBarrier = false;

    private Rigidbody2D enemyBody;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void MoveGoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Barrier"))
        {
            hitBarrier = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hitBarrier)
        {
            moveRight *= -1;
            ComputeVelocity();
            hitBarrier = false;
        }
        else if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
        }
        else
        {
            moveRight *= -1;
            ComputeVelocity();
        }
        MoveGoomba();
    }
}
