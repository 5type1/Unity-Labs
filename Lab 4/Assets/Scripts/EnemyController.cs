using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public GameConstants gameConstants;
	private int moveRight;
	private float originalX;
	private Vector2 velocity;
	private Rigidbody2D enemyBody;
	private Animator enemyAnimator;
	private bool isTheTyrantDead = false;

	void Start()
	{
		enemyBody = GetComponent<Rigidbody2D>();

		// get the starting position
		originalX = transform.position.x;

		// randomise initial direction
		moveRight = Random.Range(0, 2) == 0 ? -1 : 1;

		// compute initial velocity
		ComputeVelocity();

		GameManager.OnPlayerDeath += EnemyRejoice;

		enemyAnimator = GetComponent<Animator>();
	}

	void ComputeVelocity()
	{
		if (isTheTyrantDead) velocity = Vector2.zero;
		else velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
	}

	void MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime * gameConstants.enemySpeedBonus);
	}

	void Update()
	{
		if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
		{
			MoveEnemy();
		}
		else
		{
			if (enemyBody.position.x - originalX < 0)
			{
				moveRight = 1;
			}
			else
			{
				moveRight = -1;
			}
			ComputeVelocity();
			MoveEnemy();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// check if collides on top
			float yoffset = (other.transform.position.y - this.transform.position.y);
			if (yoffset > 0.75f)
			{
				KillSelf();
			}
			else
			{
				// hurt player
				CentralManager.centralManagerInstance.damagePlayer();
			}
		}
		if (other.gameObject.CompareTag("Obstacle"))
		{
			if (moveRight == 1) originalX = enemyBody.position.x - gameConstants.maxOffset;
			else originalX = enemyBody.position.x + gameConstants.maxOffset;
		}
	}

	void KillSelf()
	{
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

	IEnumerator flatten()
	{
		Debug.Log("Flatten starts");
		int steps = 5;
		float stepper = 1.0f / (float)steps;

		for (int i = 0; i < steps; i++)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield return null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		gameConstants.spawned -= 1;
		yield break;
	}

	void EnemyRejoice()
	{
		Debug.Log("Enemy killed Mario");
		enemyAnimator.SetBool("dance", true);
		isTheTyrantDead = true;
	}
}