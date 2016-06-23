using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class TearEvent : MonoBehaviour {
	[SerializeField]SpriteRenderer[] backgrounds;
	[SerializeField]GameObject hand;
	[SerializeField]float duration = 1.0f;
	[SerializeField]Transform respawnPoint;

	Transform handInstance;
	bool tearing = false;
	float timer = 0.0f;

	void Awake(){
		Init();
		GameEventManager.GameEnd += Init;
	}

	// Use this for initialization
	void Start () {


		handInstance.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (tearing){
			//Turn on the hand if it is off
			if (!handInstance.gameObject.activeSelf) {
				handInstance.gameObject.SetActive(true);
			}
			//Lerp between the start position and the bottom of this sprite over time.
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			float progress = Mathf.Lerp(sr.transform.position.y + sr.sprite.bounds.max.y*sr.transform.localScale.y, 
			                            sr.transform.position.y + sr.sprite.bounds.min.y*sr.transform.localScale.y,
			                            (timer+=Time.deltaTime)/duration);
			
			sr.material.SetFloat("_Clip_Y", progress);
			foreach (SpriteRenderer background in backgrounds) {
				background.material.SetFloat("_Clip_Y", progress);
			}
			handInstance.transform.position = new Vector3(handInstance.transform.position.x, progress,
			                                              handInstance.transform.position.z);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player"){
			tearing = true;
			other.transform.root.GetComponent<PlayerHealth>().SetRespawnPoint(respawnPoint.position);
		}
	}

	void Init(){
		GameEventManager.GameEnd -= Init;
		tearing = false;
		timer = 0;
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.material.SetFloat("_Inverse_Y", 1);
		sr.material.SetFloat("_Clip_Y", sr.transform.position.y + sr.sprite.bounds.max.y * sr.transform.localScale.y);
		sr.material.SetFloat("_Clip_X", sr.transform.position.x + sr.sprite.bounds.max.x * sr.transform.localScale.x);
//		Debug.Log(sr.sprite.bounds.max);
//		Debug.Log(sr.sprite.bounds.min);
		foreach (SpriteRenderer background in backgrounds) {
						background.material.SetFloat ("_Inverse_Y", 0);
						background.material.SetFloat ("_Clip_Y", sr.transform.position.y + sr.sprite.bounds.max.y * sr.transform.localScale.y);
						background.material.SetFloat ("_Clip_X", sr.transform.position.x + sr.sprite.bounds.center.x * sr.transform.localScale.x);
				}
		handInstance = (Instantiate(hand, new Vector3(sr.transform.position.x, sr.transform.position.y + sr.sprite.bounds.max.y * sr.transform.localScale.y),
		                            Quaternion.identity) as GameObject).transform;
	}
}
