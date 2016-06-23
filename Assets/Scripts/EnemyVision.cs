using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour {

	public bool onSight = false;
	public Vector3 direction;
	public Collider2D player;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (direction);
//	if(onSight)
//			this.transform.parent.rigidbody2D.velocity=direction;
	}
	void FixedUpdate(){
		if(onSight)
		{
			direction = player.transform.position - transform.position;
			//this.transform.parent.rigidbody2D.velocity=direction;

		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		//Debug.Log (col.gameObject.tag);
		//Debug.Log (onSight);
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Player")
		{
			player=col;
			onSight=true;
			//Debug.Log (onSight);
			direction = player.transform.position - transform.position;
//			this.transform.parent.rigidbody2D.velocity=direction;
			//Debug.LogWarning (col.gameObject.tag);
//			Debug.Log (direction);
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		//Debug.Log (col.gameObject.tag);
		//Debug.Log (onSight);
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Player")
		{
			onSight=false;
		}
	}
}
