using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject[] enemys;
	Animator anim;

	int handHash = Animator.StringToHash("Draw");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player") {
			anim.SetTrigger (handHash);
		}
	}

	void ActivateEnemy(int i){
		enemys[i].SetActive (true);
		if (enemys[i].GetComponent<Enemy>()) enemys[i].GetComponent<Enemy>().GameStart();
	}

	void DestroySpawner(){
		Destroy (gameObject);
	}
}
