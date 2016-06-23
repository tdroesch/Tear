using UnityEngine;
using System.Collections;

public class RobotAI : MonoBehaviour {
	public float range = 25.0f;
	public GameObject nut, bolt;
	public float throwTime = 0.5f;
	public float recoverTime = 0.417f;
	public Vector2 throwOffSet;
	public float throwSpeed = 100.0f;
	public float teleportTime = 1.0f;
	public float minTeleportDistance = 5.0f;
	public Transform[] teleportLocations;
	public bool facingRight = false;
//	public float moveSpeed = 2f;		// The speed the enemy moves at.
//	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
//	private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private EnemyVision sight;
	private SpriteRenderer ren;			// Reference to the sprite renderer.
	[SerializeField]
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	bool busy = false;
	IEnumerator actionRoutine;
	Animator anim;
	int throwHash = Animator.StringToHash("Toss");
	int stunHash = Animator.StringToHash("Stun");

	enemyGUI healthBar;

	
	void Awake() {
		// Setting up the references.
//		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
		
		sight = transform.Find("Sight").GetComponent<EnemyVision>();

		anim = GetComponent<Animator>();

		GetComponent<enemyGUI> ().DamageCallback += TakeDamage;
	

		//for(int=
//		healthBar = this.transform.GetChild(5).GetChild(0).GetComponent<SpriteRenderer>();
		//Debug.Log (this.transform.chil);
		
		// Getting the intial scale of the healthbar (whilst the player has full health).
//		healthScale = healthBar.transform.localScale;
	}

	void Start() {
//		GameObject.FindGameObjectWithTag("Player");
	}
//
//	void OnCollisionEnter2D(Collision2D col) {
//		//Debug.LogWarning (col.gameObject.tag);
//		// If the colliding gameobject is an Enemy...
//		if (col.collider.name == "attack_collider") {
//			if (HP > 0f) {
//				// ... take damage and reset the lastHitTime.
//				TakeDamage(col.transform); 
//			}
//			// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
//			else {
//				Destroy(gameObject);
//			}
//		}
//	}
	
	IEnumerator Throw(Vector3 target) {
		busy = true;
		anim.SetTrigger(throwHash);
		yield return new WaitForSeconds(throwTime);
		int projNum = Random.Range(0, 2);
		GameObject proj = Instantiate(projNum == 0 ? nut : bolt, transform.position + new Vector3(throwOffSet.x,throwOffSet.y), Quaternion.identity) as GameObject;
		proj.GetComponent<Rigidbody2D>().velocity = (target - proj.transform.position).normalized*throwSpeed;
		proj.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(120.0f,240.0f);
		Destroy(proj, 10);
		yield return new WaitForSeconds(recoverTime);
		busy = false;
	}

	IEnumerator Teleport(){
		busy = true;
		yield return new WaitForSeconds(teleportTime);
		bool canTeleport = false;
		Vector3 location = transform.position;
		foreach (Transform t in teleportLocations){
			if (teleportCheck(t.position)) {
				canTeleport = true;
			}
		}
		while (canTeleport){
			canTeleport = false;
			location = teleportLocations[Random.Range(0, teleportLocations.Length)].position;
			if (teleportCheck(location)){
				transform.position = location;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				busy = false;
			}
			else {
				foreach (Transform t in teleportLocations){
					if (teleportCheck(t.position)) {
						canTeleport = true;
					}
				}
			}
		}
	}
	
	void FixedUpdate() {
		if (anim.GetCurrentAnimatorStateInfo(0).nameHash != Animator.StringToHash("Base Layer.robot_stun")) {
			// Create an array of all the colliders in front of the enemy.
			Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);
			
			// Check each of the colliders.
			foreach (Collider2D c in frontHits) {
				// If any of the colliders is an Obstacle...
				if (c.tag == "Obstacle") {
					// ... Flip the enemy and stop checking the other colliders.
					Flip();
					break;
				}
			}
			if (sight.onSight) {
				// Start throwing shit at them
				if (!busy) {
					bool canTeleport = false;
					foreach (Transform t in teleportLocations){
						if (teleportCheck(t.position)) {
							canTeleport = true;
						}
					}
					if (Vector3.Distance(transform.position, sight.player.transform.position) <= minTeleportDistance && canTeleport){
						if (actionRoutine != null)
							StopCoroutine(actionRoutine);
						actionRoutine = Teleport();
						StartCoroutine(actionRoutine);
					}
					else {
						actionRoutine = Throw(sight.player.transform.position);
						StartCoroutine(actionRoutine);
					}
				}
				//			rigidbody2D.velocity = new Vector2(sight.direction.x * moveSpeed, rigidbody2D.velocity.y);
				//			Debug.Log(sight.direction);
			}
			
			//sight.
			// Set the enemy's velocity to moveSpeed in the x direction.
			//rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);	
			//		rigidbody2D.velocity = new Vector2(sight.direction.x * moveSpeed, rigidbody2D.velocity.y);
			
			if (sight.direction.x > 0.1 && !facingRight)
				// ... flip the player.
				Flip();
			
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (sight.direction.x < -0.1 && facingRight)
				// ... flip the player.
				Flip();
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
	}

	public void TakeDamage() {
		// Cancel any throws
		if (actionRoutine != null)
			StopCoroutine(actionRoutine);
		busy = false;
		anim.SetTrigger(stunHash);

		// Create a vector that's from the enemy to the player with an upwards boost.
//		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
//		
//		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
//		rigidbody2D.AddForce(hurtVector * hurtForce);
		
		// Reduce the number of hit points by one.
//		HP -= 5;
//		Debug.LogWarning("minus life" + HP);
		
//		UpdateHealthBar();
		//		// Reduce the player's health by 10.
		//		health -= damageAmount;
	}

	public void Flip() {
		facingRight = !facingRight;
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
		GetComponent<enemyGUI>().Flip();
	}

	bool teleportCheck(Vector3 t){
		if (Vector3.Distance(t,transform.position) >= minTeleportDistance){
			if (!Physics2D.OverlapCircle(t, minTeleportDistance/2, LayerMask.GetMask("Enemy", "Player"))) {
				return true;
			}
		}
		return false;
	}
	
//	public void UpdateHealthBar() {
//		// Set the scale of the health bar to be proportional to the player's health.
////		healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
////		Debug.LogWarning("minus shit" + healthScale.x * HP * 0.01f);
//	}
}
