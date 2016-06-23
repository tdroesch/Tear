using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public float health = 100f;
	float maxHealth;
	bool isAlive;
	Vector2 respawnPoint;
	Vector2 startPoint;

	// Use this for initialization
	void Start() {
		maxHealth = health;
		respawnPoint = transform.position;
		startPoint = transform.position;
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<Renderer>().enabled = false;
		isAlive = false;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameEnd += GameEnd;
	}
	
	// Update is called once per frame
	void Update() {
		if (health <= 0 && isAlive) {
			GameEventManager.OnGameOver();
		}
	}

	void Respawn() {
		GetComponent<Rigidbody2D>().isKinematic = false;
		health = maxHealth;
		isAlive = true;
		transform.position = respawnPoint;
	}

	void GameOver() {
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<Rigidbody>().velocity = new Vector3();
		isAlive = false;
	}

	void GameStart() {
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<Renderer>().enabled = true;
		transform.position = startPoint;
		respawnPoint = startPoint;
		health = maxHealth;
		isAlive = true;
	}

	void GameEnd() {
		GameEventManager.Respawn -= Respawn;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameEnd -= GameEnd;
		GetComponent<Renderer>().enabled = false;
	}
}
