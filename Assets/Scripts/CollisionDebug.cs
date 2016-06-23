using UnityEngine;
using System.Collections;

public class CollisionDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D _collision){
		Debug.LogError("Collision with " + _collision.gameObject + " at " + Time.frameCount);
	}

	void OnTriggerEnter2D(Collider2D _collider){
		Debug.Log("Trigger with " + _collider.gameObject + " at " + Time.frameCount);
	}
}
