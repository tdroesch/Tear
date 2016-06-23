using UnityEngine;
using System.Collections;

public class ET : MonoBehaviour {

	etMissile detector;
	GameObject tell;
	Animator anim;
	public Transform Player;
	public float Height=46f;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();
		tell = transform.FindChild("tell").gameObject;
		tell.GetComponent<Renderer>().enabled=false;
		detector = transform.FindChild("Detector").GetComponent<etMissile>();
		GetComponent<enemyGUI>().DamageCallback += TakeDamage;
		//Player = transform.Find("Hero");
//		Debug.Log(Player.name);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(detector.isHere)
		{
			anim.SetTrigger("under");
			//Debug.LogWarning("itttworks");
			tell.GetComponent<Renderer>().enabled=true;
		}
		else
			tell.GetComponent<Renderer>().enabled=false;

		anim.SetFloat("Speed", GetComponent<Rigidbody2D>().velocity.x);
	
	}

	void FixedUpdate()
	{
		Vector2 target = new Vector2(Player.position.x, Player.transform.position.y+Height);
		this.GetComponent<Rigidbody2D>().velocity=(target-this.GetComponent<Rigidbody2D>().position);
	}

	public void TakeDamage ()
	{
		StartCoroutine (ToggleRenderer());
	}

	IEnumerator ToggleRenderer(){
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		float timer = 0f;
		while (timer < 1f) {
			yield return null;
			timer += Time.deltaTime;
			yield return null;
			timer += Time.deltaTime;
			yield return null;
			timer += Time.deltaTime;
			sr.enabled = !sr.enabled;
		}
		sr.enabled = true;
	}
}
