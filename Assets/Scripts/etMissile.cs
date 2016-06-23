using UnityEngine;
using System.Collections;

public class etMissile : MonoBehaviour {

	public GameObject missile;
	[HideInInspector] public bool missileIsOut=false;
	[HideInInspector] public bool isHere=false;
	bool missileCanGo=false;
	GameObject newMissile;
	int i=0;

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {
		if(missileCanGo)
		{
			newMissile=Instantiate(missile,this.transform.position,Quaternion.identity) as GameObject;
			missileIsOut=true;
			Debug.Log(newMissile.name);
			missileCanGo=false;
			StartCoroutine(isOut());
		}
	}

	void OnTriggerStay2D( Collider2D col)
	{
		if( col.tag=="Player" && i==0)
		{
			i=1;
			StartCoroutine(counter());
			if(!missileIsOut)
			{
				StartCoroutine(waitForIt());
//				if(missileCanGo==true)
//				{
//					newMissile=Instantiate(missile,this.transform.position,Quaternion.identity) as GameObject;
//					missileIsOut=true;
//					//Debug.Log(newMissile.name);
//					newMissile.GetComponent<Rigidbody2D>().AddForce((Vector2.up*-1)*100f);
//					missileCanGo=false;
//					StartCoroutine(isOut());
//				}
			}

		}
	}

	IEnumerator isOut()
	{
		yield return new WaitForSeconds(5);
		//Debug.LogWarning("it works");
		missileIsOut = false;

	}

	IEnumerator counter()
	{
		yield return new WaitForSeconds(0.9f);
		i = 0;
		
	}

	IEnumerator waitForIt()
	{
		isHere = true;
		yield return new WaitForSeconds(1);
		missileCanGo = true;
		isHere = false;
//		newMissile=Instantiate(missile,this.transform.position,Quaternion.identity) as GameObject;
//		missileIsOut=true;
//		//Debug.Log(newMissile.name);
//		//newMissile.GetComponent<Rigidbody2D>().AddForce((Vector2.up*-1)*100f);
//		missileCanGo=false;
//		StartCoroutine(isOut());
		
	}
}
