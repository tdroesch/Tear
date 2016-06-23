using UnityEngine;
using System.Collections;

public static class GameEventManager{

	public delegate void GameEvent();
    
	//Declare the events of type that is the deligate.
	//You can add functions to the event with the +=
	public static event GameEvent GameStart, GameOver, GameEnd, Respawn;
    
	public static void OnGameStart() {
		if (GameStart != null) {
			GameStart();
		}
	}
    
	public static void OnGameOver() {
		if (GameOver != null) {
			GameOver();
		}
	}

	public static void OnRespawn() {
		if (Respawn != null) {
			Respawn();
		}
	}

	public static void OnGameEnd() {
		if (GameEnd != null) {
			GameEnd();
		}
	}
}
