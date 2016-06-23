using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;
	float jumpForce = 90000f;			// Amount of force added when the player jumps.
//	bool jump=false;
	//pick up
	public PickUpManager PUM;
	//Animator hashvalues
	int attackHash = Animator.StringToHash ("Attack");
	int lungeHash = Animator.StringToHash ("Lunge");
	int liftHash = Animator.StringToHash ("Lift");
	int speedHash = Animator.StringToHash("Speed");

	void Awake ()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator> ();
		playerCtrl = transform.root.GetComponent<PlayerControl> ();
		PUM = gameObject.GetComponent<PickUpManager> ();
	}

	void Start(){
		anim.SetTrigger(lungeHash);
		anim.SetTrigger(liftHash);
	}
	void Update ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			anim.SetTrigger (attackHash);
//			anim.SetFloat(speedHash, 0);
//			playerCtrl.Hit(0.5f);
		}
		if (Input.GetButtonDown ("Fire2"))
			anim.SetTrigger (lungeHash);
		if (Input.GetButtonDown ("Fire3"))
			anim.SetTrigger (liftHash);
		
//		if(Input.GetKeyDown(KeyCode.E))
//		{
//			anim.SetTrigger("Punch");
//		}
//		if (Input.GetButtonDown("Fire1")) {
//			// If the fire button is pressed...
//			if (anim.GetBool(groundedHash)) {
//				if (PUM.hasSword) {
//					anim.SetTrigger("Slash2");
//					if(playerCtrl.facingRight)
//						this.rigidbody2D.AddForce(new Vector2(jumpForce,0f));
//					else
//						this.rigidbody2D.AddForce(new Vector2(-(jumpForce),0f));
//					}
////					if (Input.GetAxis("Vertical") > 0.1f) {
////						//do up anim
////						Debug.Log("do up anim");
////					} else if (Input.GetAxis("Vertical") < -0.1f) {
////						//do down anim
////						anim.SetTrigger("Slash2");
////						Debug.Log("do down anim");
////					} else {
////						//Do normal anim
////						if (anim.GetCurrentAnimatorStateInfo(0).IsName("hero_l_slash")) {
////							anim.SetTrigger("Slash2");
////						} else {
////							anim.SetTrigger("Slash");
////						}
////						Debug.Log("do normal anim");
//					}
//				else {
//					Debug.Log(playerCtrl.canAirJump);
//					if (playerCtrl.canAirJump == true) {
//						if (Input.GetAxis("Vertical") > 0.1f) {
//							//do up in air anim
//							anim.SetTrigger("AirKick");
//							Debug.Log("do up in air anim");
//						} else if (Input.GetAxis("Vertical") < -0.1f) {
//							//do down in air anim
//							Debug.Log("do down in air anim");
//						} else {
//							//Do normal Air anim
//							anim.SetTrigger("AirKick");
//							playerCtrl.airjump=false;
//						playerCtrl.canAirJump=false;
//							Debug.Log("do normal in air anim");
//						}
//					} else {
//						Debug.Log("dsd");
//					}
//				}
//		}
//		else if(Input.GetButtonDown("Fire2"))
//		{
//			if(PUM.hasShield)
//			{			
//				anim.SetTrigger("ShieldUp");
//				StartCoroutine("jumpWait");
//
//			}
//		}
	}

	IEnumerator jumpWait ()
	{
		yield return new WaitForSeconds (0.2f);
		this.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0f, jumpForce));
	}

	void resetAttack ()
	{
		anim.ResetTrigger (attackHash);
	}

	void FixedUpdate ()
	{
//		if(jump)
//		{
//			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
//			Debug.LogWarning("JUMP");
//		}

	}
}
