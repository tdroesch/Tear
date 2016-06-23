using UnityEngine;
using System.Collections;
using System.Text;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour {
	
	public float slope = 0.3f;
	public float stepHeight = 0.1f;
	public float maxGroundSpeed = 5;
	public float maxAirSpeed = 5;
	public float acceleration = 5;
	public float friction = 10;
	public float minJumpHeight = 5;
	public float maxJumpHeight = 7;
	public float jumpSpeed = 10;
	public bool doubleJump = false;
	public float gravityScale = 1;

	public LayerMask passThroughPlatforms;

	bool grounded = false;

	float horizontal = 0;
	bool jump = false;
	bool midJump = false;
	float minTargetJump = 0;
	float maxTargetJump = 0;

	// Use this for initialization
	void Start () {
		if (GetComponent<Collider2D>() == null || GetComponent<Collider2D>().enabled == false) {
			Collider2D[] childrenColliders = GetComponentsInChildren<Collider2D> (false);
			bool activeCollider = false;
			if (childrenColliders.Length > 0) {
				foreach (Collider2D c in childrenColliders) {
					activeCollider = activeCollider | c.enabled;
				}
			}
			if (!activeCollider) {
				Debug.LogError(this.ToString() + " Error:\n" + "There are no active colliders on this object.");
			}
		}
		GetComponent<Rigidbody2D>().gravityScale = gravityScale;
		GetComponent<Rigidbody2D>().fixedAngle = true;
		GetComponent<Rigidbody2D>().mass = 1;
	}
	
	// Update is called once per frame
	// Handle User Input Here
	void Update () {
		horizontal = Input.GetAxis("Horizontal");
		jump = Input.GetButton("Jump");
	}

	// FixedUpdate is called once per physics frame
	// Hand motion here
	void FixedUpdate (){
		Vector2 vel = GetComponent<Rigidbody2D>().velocity;
		vel.x += horizontal*acceleration*Time.fixedDeltaTime;
		vel.x = Mathf.Sign(vel.x)*Mathf.Min(Mathf.Abs(vel.x), maxGroundSpeed);

		if (grounded){
			if (horizontal == 0 || Mathf.Sign(vel.x) != Mathf.Sign(horizontal)){
				float frictionAcc = friction*Time.fixedDeltaTime;
				vel.x = Mathf.Abs(vel.x)>frictionAcc ? vel.x-Mathf.Sign(vel.x)*frictionAcc : 0;
			}
			if (jump){
				minTargetJump = GetComponent<Rigidbody2D>().position.y + minJumpHeight;
				maxTargetJump = GetComponent<Rigidbody2D>().position.y + maxJumpHeight;
				midJump = true;
			}
		}

		if (midJump){
			if (GetComponent<Rigidbody2D>().position.y < minTargetJump || (jump && GetComponent<Rigidbody2D>().position.y < maxTargetJump)){
				vel.y = jumpSpeed;
			} else midJump = false;
		}
		vel.y = Mathf.Sign(vel.y)*Mathf.Min(Mathf.Abs(vel.y), jumpSpeed);
		
		jump = false;
		GetComponent<Rigidbody2D>().velocity = vel;
	}

	void OnCollisionEnter2D(Collision2D col){
//		if (col.gameObject.layer = passThroughPlatforms.value)
	}

	void OnCollisionStay2D(Collision2D col){
		if (col.contacts.Length > 1){
			float lowerbound = GetLowerBound();
			Vector2 point1 = col.contacts[0].point;
			Vector2 point2 = col.contacts[1].point;
			if (point1.x != point2.x && point1.y == point2.y && point2.y <= lowerbound){
				grounded = true;
			}

//			Debug.Log(transform.InverseTransformPoint(cp.point));
		}
		if (col.contacts.Length > 2){ Debug.LogWarning ("More than two collision points for " + this.ToString());}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.contacts.Length > 1){
			float lowerbound = GetLowerBound();
			Vector2 point1 = col.contacts[0].point;
			Vector2 point2 = col.contacts[1].point;
			if (point1.x != point2.x && point1.y == point2.y && point2.y < lowerbound){
				grounded = false;
			}
			
			//			Debug.Log(transform.InverseTransformPoint(cp.point));
		}
		if (col.contacts.Length > 2){ Debug.LogWarning ("More than two collision points for " + this.ToString());}
//		foreach (ContactPoint2D cp in col.contacts){
//			Debug.Log("Exit " + Time.frameCount + " " + transform.InverseTransformPoint(cp.point) + "\n" +
//			          col.contacts.Length + " " + transform.InverseTransformPoint(cp.point).x + " " + transform.InverseTransformPoint(cp.point).y);
//		}
	}

	float GetLowerBound(){
		float lowerbound = 0;
		if (GetComponent<Collider2D>() != null){
			lowerbound = GetComponent<Collider2D>().bounds.center.y-GetComponent<Collider2D>().bounds.extents.y;
		}
		foreach (Collider2D c in transform.GetComponentsInChildren<Collider2D>(false)){
			float testLB = c.bounds.center.y-c.bounds.extents.y;
			lowerbound = Mathf.Min(testLB, lowerbound);
		}
		return lowerbound;
	}


}
