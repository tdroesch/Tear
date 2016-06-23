using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {	
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	public float hitStunPeriod = 1f;
	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	float damageAmount = 0.1f;			// The amount of damage to take when enemies touch the player
	[SerializeField]
	float packAmount = 0.1f;            // the amount to restore health
	
	private float lastHitTime;					// The time at which the player was last hit.
	private PlayerControl playerControl;		// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player

	//GUIstuff
	public Image life;
//	public Image stick_note;
//	public float currHp=100;
	public float lifeAmount;

	//pick up
	public PickUpManager PUM;
	GameObject Sword;
	Vector2 respawnPoint;
	Vector2 startPoint;

	//Animator hashvalues
	int hitHash = Animator.StringToHash ("Hit");
	int aliveHash = Animator.StringToHash ("Alive");

	void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl> ();
		//healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator> ();
		PUM = gameObject.GetComponent<PickUpManager> ();

//		Sword = gameObject.transform.FindChild("sword").gameObject;
		
		respawnPoint = transform.position;
		startPoint = transform.position;
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<Renderer> ().enabled = false;
		lifeAmount = 1;
		anim.GetCurrentAnimatorStateInfo (0).IsName ("hero_l_slash");
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameEnd += GameEnd;

	}

	void Respawn ()
	{
		GetComponent<Rigidbody2D> ().isKinematic = false;
		lifeAmount = 1;
		transform.position = respawnPoint;
		GetComponent<PlayerControl> ().enabled = true;
		GetComponentInChildren<PlayerAttack> ().enabled = true;
		anim.Play ("hero_stand");
		anim.SetBool (aliveHash, true);
		anim.SetBool (hitHash, false);
	}
	
	void GameOver ()
	{
		// Find all of the colliders on the gameobject and set them all to be triggers.
//		Collider2D[] cols = GetComponents<Collider2D>();
//		foreach(Collider2D c in cols)
//		{
//			c.isTrigger = true;
//		}
//		
//		// Move all sprite parts of the player to the front
//		SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
//		foreach(SpriteRenderer s in spr)
//		{
//			s.sortingLayerName = "UI";
//		}
		
		// ... disable user Player Control script
		GetComponent<PlayerControl> ().enabled = false;
		
		// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
		GetComponentInChildren<PlayerAttack> ().enabled = false;

		
		GetComponent<Rigidbody2D> ().isKinematic = true;
		
		// ... Trigger the 'Die' animation state
		anim.SetBool (aliveHash, false);
		anim.SetBool (hitHash, true);
		lifeAmount = 0;
	}
	
	void GameStart ()
	{
		GetComponent<PlayerControl> ().enabled = true;
		GetComponentInChildren<PlayerAttack> ().enabled = true;
		lifeAmount = 1;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Renderer> ().enabled = true;
		transform.position = startPoint;
		respawnPoint = startPoint;
		anim.Play ("hero_stand");
		anim.SetBool (aliveHash, true);
		anim.SetBool (hitHash, false);
	}
	
	void GameEnd ()
	{
		GameEventManager.Respawn -= Respawn;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameEnd -= GameEnd;
		Application.LoadLevel (Application.loadedLevel);
	}

	void Update ()
	{
		if (PUM.healthPack == true) {
			AddLife ();
		}
		if (anim.GetBool (aliveHash) && anim.GetBool (hitHash) && Time.time > lastHitTime + hitStunPeriod) {
			anim.SetBool (hitHash, false);
		}
		life.fillAmount = lifeAmount;
	}

	void AddLife ()
	{
		lifeAmount += packAmount;
		lifeAmount = Mathf.Clamp01 (lifeAmount);
		PUM.healthPack = false;
	}

	IEnumerator ToggleRenderer(){
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		while (Time.time < lastHitTime + repeatDamagePeriod) {
			yield return null;
			yield return null;
			yield return null;
			sr.enabled = !sr.enabled;
		}
		sr.enabled = true;
	}

	void OnCollisionEnter2D (Collision2D col)
	{
//		if(col.gameObject.name=="robot")
//		{
		//Debug.Log(col.collider.transform.gameObject.name);
//			if(Sword.activeSelf==false)
//			{
		// If the colliding gameobject is an Enemy...
		if (col.gameObject.layer == LayerMask.NameToLayer ("EnemyAttack")) {
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod && lifeAmount > 0.0f) {
				// ... and if the player still has health...
//						if(lifeAmount > 0.09f)
//						{
				// ... take damage and reset the lastHitTime.
				anim.SetBool (hitHash, true);
				TakeDamage (col.transform); 
				lastHitTime = Time.time; 
				StartCoroutine(ToggleRenderer());
//						}
				// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
				if (lifeAmount <= 0.0f) {
					anim.SetBool (aliveHash, false);
					GameEventManager.OnGameOver ();
					//anim.SetTrigger("Die");
				}
			}
			// Destroy the object that hit
			if (col.gameObject.name != "attack_collider") {
				Destroy (col.gameObject);
			}
//				}
//			}
		}
	}

	void TakeDamage (Transform enemy)
	{
		// Make sure the player can't jump.
		playerControl.jump = false;
		playerControl.Hit (hitStunPeriod);

		// Create a vector that's from the enemy to the player with an upwards boost.
//		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
		//Vector3 hurtVector = enemy.position - transform.position + Vector3.up * 10f;

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
//		rigidbody2D.AddForce(hurtVector * hurtForce);
//		Debug.Log(hurtVector * hurtForce);
		// Reduce the player's health by 10.
		//Debug.Log(damageAmount);
		lifeAmount -= damageAmount;
		//Debug.Log(lifeAmount);
	}

	public void SetRespawnPoint(Vector2 point){
		respawnPoint = point;
	}
	
//	void OnGUI ()
//	{
		
		//LIFE_BAR
		//lifebar border
//		GUI.Box (new Rect (Screen.width * 0.01f, Screen.height * 0.001f, Screen.width * 0.25f, Screen.height * 0.3f), "", stick_note);
//		
//		//----------------------------------------------porcentage times the size of the bar makes it move
//		GUI.BeginGroup (new Rect (Screen.width * 0.05f, Screen.height * 0.03f, (Screen.width * 0.136f) * lifeAmount, Screen.height));
//		//--- The group is what is cropping off the life bar
//		GUI.Box (new Rect (-Screen.width * 0.033f, 0.0f, Screen.width * 0.2f, Screen.height * 0.3f), "", life);
//		GUI.EndGroup ();

		
//	}
}
