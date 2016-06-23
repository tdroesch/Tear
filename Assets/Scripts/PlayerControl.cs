using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
//	[HideInInspector]
//	public bool airjump= false;
//	public bool canAirJump = false;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	//public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	//public AudioClip[] taunts;				// Array of clips for when the player taunts.
//	public float tauntProbability = 50f;	// Chance of a taunt happening.
//	public float tauntDelay = 1f;			// Delay for when the taunt should happen.
	
	public float friction = 10;
	[HideInInspector]
	public bool flipable = true;

	//private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	[HideInInspector]
//	public bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.
	//Animator hashvalues
	int jumpHash = Animator.StringToHash("Jump");
	int groundedHash = Animator.StringToHash("Grounded");
	int speedHash = Animator.StringToHash("Speed");

	bool canMove = true;

	public void Hit(float time){
		canMove = false;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		StartCoroutine(moveTimer(time));
	}


	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		//grounded = 	Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if(Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
		{
			anim.SetBool(groundedHash, true);
		}
		else
		{
			anim.SetBool(groundedHash, false);
		}
//		Debug.Log("grounded"+ anim.GetBool(groundedHash));
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && anim.GetBool(groundedHash)){
			jump = true;
			// Set the Jump animator trigger parameter.
			anim.SetTrigger(jumpHash);
		}
//		if (Input.GetButtonDown ("Jump") && !grounded && canAirJump)
//		{
//			airjump = true;
//			//do air jump
//			Debug.Log("airjump");
//		}
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}


	void FixedUpdate ()
	{
		// Cache the horizontal input.
		if (canMove) {
			float h = Input.GetAxis("Horizontal");
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat(speedHash, Mathf.Abs(h));
			
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
				// ... add a force to the player.
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
			//Debug.Log(rigidbody2D.velocity);
			
			// If the player's horizontal velocity is greater than the maxSpeed...
			if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			
			if (anim.GetBool(groundedHash)) {
				if (h == 0 || Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) != Mathf.Sign(h)) {
					Vector2 vel = GetComponent<Rigidbody2D>().velocity;
					float frictionAcc = friction * Time.fixedDeltaTime;
					vel.x = Mathf.Abs(vel.x) > frictionAcc ? vel.x - Mathf.Sign(vel.x) * frictionAcc : 0;
					GetComponent<Rigidbody2D>().velocity = vel;
				}
			}
			
			// If the input is moving the player right and the player is facing left...
			if (h > 0 && !facingRight && flipable)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (h < 0 && facingRight && flipable)
				// ... flip the player.
				Flip();
			
			// If the player should jump...
			if (anim.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.hero_jump") && jump) {
			
			
			
				//			// Play a random jump audio clip.
				//			int i = Random.Range(0, jumpClips.Length);
				//			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
			
				// Add a vertical force to the player.
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			
				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jump = false;
				//			canAirJump = true;
				//Debug.Log("grounded"+ grounded);
				//Debug.Log("canairjump"+canAirJump);
			}
		}
		// If the player should Airjump...
//		if(airjump)
//		{
//			// Set the AirJump animator trigger parameter.
//			anim.SetTrigger("AirJump");
//			
//			//			// Play a random jump audio clip.
//			//			int i = Random.Range(0, jumpClips.Length);
//			//			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
//			
//			// Add a vertical force to the player.
//			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
//			//Debug.Log("jumpforce"+jumpForce);
//			
//			// Make sure the player can't airjump again until the airjump conditions from Update are satisfied.
//			airjump = false;
//			canAirJump = false;
//		}
	}
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	IEnumerator moveTimer(float time) {
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	void SwitchKinematic(int value){
		GetComponent<Rigidbody2D>().isKinematic = value>0;
	}
}
