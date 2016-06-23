using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public bool facingRight = false;
	public float moveSpeed = 2f;		// The speed the enemy moves at.
//	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
//	public bool heat = false;
	private EnemyVision sight;
//	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private Transform frontGroundCheck;
	private Transform groundCheck;
	private bool stun = true;
	Animator anim;
	int stunHash = Animator.StringToHash ("Stun");
	int attackHash = Animator.StringToHash ("Attack");
	
	void OnEnable ()
	{
		// Setting up the references.
//		ren = transform.Find ("body").GetComponent<SpriteRenderer> ();
		frontCheck = transform.Find ("frontCheck").transform;
		frontGroundCheck = transform.Find ("frontGroundCheck").transform;
		groundCheck = transform.Find ("groundCheck").transform;
//		score = GameObject.Find("Score").GetComponent<Score>();

		sight = transform.Find ("Sight").GetComponent<EnemyVision> ();

		anim = GetComponent<Animator> ();

//		GetComponent<Rigidbody2D> ().velocity = new Vector2 (Mathf.Sign ((GameObject.FindGameObjectWithTag ("Player").transform.position.x) - transform.position.x) * moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		GetComponent<enemyGUI>().DamageCallback += TakeDamage;

		GameEventManager.GameStart += GameStart;
		//for(int=
//		healthBar = this.transform.GetChild (5).GetChild (0).GetComponent<SpriteRenderer> ();
		//Debug.Log (this.transform.chil);

		// Getting the intial scale of the healthbar (whilst the player has full health).
//		healthScale = healthBar.transform.localScale;
	}

//	void OnCollisionEnter2D (Collision2D col)
//	{
//		//Debug.LogWarning (col.gameObject.tag);
//		// If the colliding gameobject is an Enemy...
//		if (col.gameObject.layer == LayerMask.NameToLayer ("PlayerAttack")) {
////			Debug.LogWarning ("ggggggogooooooooollllllllllll");
//			if (HP > 0f) {
//				// ... take damage and reset the lastHitTime.
//				TakeDamage (col.transform); 
//			}
//				// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
//				else {
//				Death ();
//			}
//		}
//	}

	void FixedUpdate ()
	{
		if (!stun) {
			// Create an array of all the colliders in front of the enemy.
//			Collider2D[] frontHits = Physics2D.OverlapPointAll (frontCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
//			
//			// Check each of the colliders.
//			foreach (Collider2D c in frontHits) {
//				// If any of the colliders is an Obstacle...
//				if (c.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
//					// ... Flip the enemy and stop checking the other colliders.
//					Flip ();
//					break;
//				}
//			}

			if (Physics2D.OverlapPoint(frontCheck.position, LayerMask.GetMask("Ground")) != null ||
			    (Physics2D.OverlapPoint(frontGroundCheck.position, LayerMask.GetMask("Ground")) == null &&
			 	Physics2D.OverlapPoint(groundCheck.position, LayerMask.GetMask("Ground")) != null)){
				Flip ();
			}

			if (sight.onSight) {
				anim.SetTrigger (attackHash);
			}
			
			//sight.
			// Set the enemy's velocity to moveSpeed in the x direction.
			if (Physics2D.OverlapPoint(groundCheck.position, LayerMask.GetMask("Ground")) != null) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			}
			
			if (GetComponent<Rigidbody2D> ().velocity.x > 0.1 && !facingRight)
				// ... flip the player.
				Flip ();
			
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (GetComponent<Rigidbody2D> ().velocity.x < -0.1 && facingRight)
				// ... flip the player.
				Flip ();
		}
//		// If the enemy has one hit point left and has a damagedEnemy sprite...
//		if(HP == 1 && damagedEnemy != null)
//			// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
//			ren.sprite = damagedEnemy;
//			
//		// If the enemy has zero or fewer hit points and isn't dead yet...
//		if(HP <= 0 && !dead)
//			// ... call the death function.
//			Death ();
	}

	public void TakeDamage ()
	{
		anim.SetTrigger(stunHash);
		StartCoroutine (StunTimer (1f));
	}

	IEnumerator StunTimer (float time)
	{
		stun = true;
		yield return new WaitForSeconds (time);
		stun = false;
	}
	public void GameStart(){
		stun = false;
	}
//	public void TakeDamage (Transform enemy)
//	{
//		anim.SetTrigger(stunHash);
//		// Create a vector that's from the enemy to the player with an upwards boost.
////		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
//		
//		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
////		rigidbody2D.AddForce(hurtVector * hurtForce);
//
//		// Reduce the number of hit points by one.
//		HP -= 5;
////		Debug.LogWarning ("minus life"+HP);
//
////		UpdateHealthBar ();
////		// Reduce the player's health by 10.
////		health -= damageAmount;
//	}
	
//	void Death ()
//	{
//		Destroy (gameObject);
//		// Find all of the sprite renderers on this object and it's children.
////		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();
//
//		// Disable all of them sprite renderers.
////		foreach(SpriteRenderer s in otherRenderers)
////		{
////			s.enabled = false;
////		}
//
//		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
////		ren.enabled = true;
////		ren.sprite = deadEnemy;
//
////		// Increase the score by 100 points
////		score.score += 100;
//
//
//		// Allow the enemy to rotate and spin it by adding a torque.
////		rigidbody2D.fixedAngle = false;
////		rigidbody2D.AddTorque(Random.Range(deathSpinMin,deathSpinMax));
//
//		// Find all of the colliders on the gameobject and set them all to be triggers.
////		Collider2D[] cols = GetComponents<Collider2D>();
////		foreach(Collider2D c in cols)
////		{
////			c.isTrigger = true;
////		}
//
////		// Play a random audioclip from the deathClips array.
////		int i = Random.Range(0, deathClips.Length);
////		AudioSource.PlayClipAtPoint(deathClips[i], transform.position);
//
////		// Create a vector that is just above the enemy.
////		Vector3 scorePos;
////		scorePos = transform.position;
////		scorePos.y += 1.5f;
//
////		// Instantiate the 100 points prefab at this point.
////		Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
//	}

	public void Flip ()
	{
		facingRight = !facingRight;
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
		GetComponent<enemyGUI>().Flip();
	}

//	public void UpdateHealthBar ()
//	{
//		if(!heat)
//		{
//			healthy=healthBar.transform.Find("HealthBar").GetComponent<SpriteRenderer>();
//		//Debug.Log (this.transform.chil);
//		
//		// Getting the intial scale of the healthbar (whilst the player has full health).
//			healthScale = healthy.transform.localScale;
//			heat=true;
//		}
	// Set the health bar's colour to proportion of the way between green and red based on the player's health.
	//healthy.material.color = Color.Lerp(Color.green, Color.red, 1 - HP * 0.01f);
		
	// Set the scale of the health bar to be proportional to the player's health.
//		healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
//		Debug.LogWarning ("minus shit" + healthScale.x * HP * 0.01f);
//	}
}
