using UnityEngine;
using System.Collections;


public class RootMotionAnimator2D : MonoBehaviour {
	Vector2 preAnimationPosition;
	Vector2 preAnimationScale;
	[SerializeField] Vector2 rootMotion;
	[SerializeField] bool applyRootMotion = false;

	// Use this for initialization
	void Awake () {
		preAnimationPosition = GetComponent<Rigidbody2D>().position;
		preAnimationScale = transform.localScale;
		rootMotion = Vector2.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if  (applyRootMotion) {
			GetComponent<Rigidbody2D>().MovePosition(preAnimationPosition + Vector2.Scale(rootMotion, preAnimationScale));
		}
		else {
			preAnimationPosition = GetComponent<Rigidbody2D>().position;
			preAnimationScale = transform.localScale;
		}
	}

	public void ResetRigidBodyMotion(){
		GetComponent<Rigidbody2D>().velocity = new Vector2();
	}
}
