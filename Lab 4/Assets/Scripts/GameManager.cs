using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Text score;
	private int playerScore = 0;
	public static event gameEvent OnPlayerDeath;
	public static event gameEvent OnScore;

	public delegate void gameEvent();

	public void increaseScore()
	{
		playerScore += 1;
		score.text = "SCORE: " + playerScore.ToString();
		OnScore();
	}

	public void damagePlayer()
	{
		OnPlayerDeath();
	}
}