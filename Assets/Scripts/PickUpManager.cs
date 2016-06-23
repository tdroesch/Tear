using UnityEngine;
using System.Collections;

public class PickUpManager : MonoBehaviour {

	public bool healthPack;
	public bool hasShield;
	public bool hasSword;
	Animator anim;
//	string[] pickUpNames;

	// Use this for initialization
	void Start () {
//		pickUpNames [0] = "Sword";
//		pickUpNames [2] = "Red Cross";
//		pickUpNames [3] = "Shield";
		anim = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("Sword", hasSword);
		anim.SetBool("Shield", hasShield);
	}


	void OnTriggerEnter2D (Collider2D Pu)
	{
		if(Pu.tag=="PickUp")
		{
			if (Pu.name == "Sword")
				hasSword = true;
			if (Pu.name == "Burger")
				healthPack = true;
			if (Pu.name == "Shield")
				hasShield = true;
			Destroy(Pu.gameObject);
		}


	}
}
