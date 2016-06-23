using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddMissileForce(float force){
		GetComponent<Rigidbody2D>().AddForce((Vector2.down)*force, ForceMode2D.Impulse);
	}
}
