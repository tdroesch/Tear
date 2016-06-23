using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemyGUI : MonoBehaviour {


	//GUIstuff
	public Image life;
	public Image stick_note;
	//public float currHp=100;
	public float lifeAmount=1;
	float overHead;
	float beforeMiddle;

	//life stuff
	public float repeatDamagePeriod = 1f;		// How frequently the player can be damaged.
	public float damageAmount = 0.1f;			// The amount of damage to take when enemies touch the player
	private float lastHitTime;					// The time at which the player was last hit.
//	float hurtForce = 10f;				// The force with which the player is pushed when hurt.


	// Delegates
	public delegate void EventCallback();

	public event EventCallback DamageCallback;



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		life.fillAmount = lifeAmount;
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
//		//Debug.LogWarning(col.collider.name);
//		// If the colliding gameobject is an Enemy...
		if (col.gameObject.layer == LayerMask.NameToLayer ("PlayerAttack")) {
//			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) {
				// ... and if the player still has health...
				TakeDamage (col.transform); 
				if (lifeAmount > 0.0001f) {
					// ... take damage and reset the lastHitTime.
					lastHitTime = Time.time; 
				}
				// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
				else {
					Destroy (gameObject);
//					Debug.LogError("I AM DEAD!!");
					//GameEventManager.OnGameOver();
				}
			}
		}
	}
	
	
	void TakeDamage (Transform enemy)
	{
		
		// Create a vector that's from the enemy to the player with an upwards boost.
//		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
		//Vector3 hurtVector = enemy.position - transform.position + Vector3.up * 10f;
		
		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
//		rigidbody2D.AddForce(hurtVector * hurtForce);
//		Debug.Log(hurtVector * hurtForce);
		// Reduce the player's health by 10.
//		Debug.Log(damageAmount);
		lifeAmount -= damageAmount;
		if (DamageCallback != null) {
			DamageCallback ();
		}
//		Debug.Log(lifeAmount);
	}

	public void Flip ()
	{
		Vector3 scale = stick_note.rectTransform.localScale;
		scale.x *= -1;
		stick_note.rectTransform.localScale = scale;
	}

//	void OnGUI (){
//
//		overHead = (this.GetComponent<Renderer>().bounds.size.y)* (this.name=="e.t"?0.25f:0.75f);
//		beforeMiddle = (this.GetComponent<Renderer>().bounds.size.x)*0.3f;
//		Vector3 headpos = (new Vector3 (-(beforeMiddle), overHead, 0)) + (this.transform.position);
//
//		Vector3 screenloc = Camera.main.WorldToScreenPoint (headpos);
//		
//		//LIFE_BAR
//		
//		//lifebar border
//		GUI.Box (new Rect (screenloc.x, (Screen.height - screenloc.y), Screen.width * 0.1f, Screen.height * 0.05f), "", stick_note);
//		
//		//----------------------------------------------porcentage times the size of the bar makes it move
//		GUI.BeginGroup (new Rect (screenloc.x+Screen.width*0.009f, (Screen.height - screenloc.y), (Screen.width*0.085f) *lifeAmount, Screen.height));
//		//--- The group is what is cropping off the life bar
//		//GUI.Box (new Rect (Screen.width * 0.01f, Screen.height * 0.02f, Screen.width * 0.1f, Screen.height * 0.05f), "",life);
//		GUI.Box (new Rect (-Screen.width*0.009f, 0f, Screen.width * 0.1f, Screen.height * 0.05f), "",life);
//		GUI.EndGroup();
//		
//		
//	}
}
