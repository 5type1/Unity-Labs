using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowMush : MonoBehaviour, ConsumableInterface
{
	public Texture t;
	public void consumedBy(GameObject player)
	{
		// give player jump boost
		player.GetComponent<PlayerController>().infJump = true;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player)
	{
		yield return new WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().infJump = false;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, 0, this);
			GetComponent<Collider2D>().enabled = false;
		}
	}
}
