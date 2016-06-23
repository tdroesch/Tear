using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public float transitionTime = 3.0f;
	public float followTime = 0.5f;
	public float lead = 3.0f;
	public float maxLead = 25.0f;
	public float yOffSet = 1.0f;
	public float yLeadScale = 0.1f;
	public GameObject player;
	float cameraSize;
	bool follow = true;
	// Use this for initialization
	void Start() {
		cameraSize = GetComponent<Camera>().orthographicSize;
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (follow) {
			Vector3 followPosition = new Vector3(Mathf.Lerp(transform.position.x, player.GetComponent<Rigidbody2D>().position.x + 
															Mathf.Sign(player.GetComponent<Rigidbody2D>().velocity.x) * 
															Mathf.Min(Mathf.Abs(lead * player.GetComponent<Rigidbody2D>().velocity.x), maxLead),
			                                                Time.fixedDeltaTime / followTime),
			                                     			Mathf.Lerp(transform.position.y, player.GetComponent<Rigidbody2D>().position.y + yOffSet + 
															yLeadScale * Mathf.Sign(player.GetComponent<Rigidbody2D>().velocity.y) * 
															Mathf.Min(Mathf.Abs(lead * player.GetComponent<Rigidbody2D>().velocity.y), maxLead),
			           										Time.fixedDeltaTime / followTime / (2*yLeadScale)));
			transform.position = followPosition;
			/*if (Input.GetButtonDown("Fire1")) {
				StartCoroutine(EnterCombatZone(new Vector3(0,0,30)));
			}*/
		}
		/*else {
			if (Input.GetButtonDown("Fire2")) {
				StartCoroutine(ExitCombatZone(new Vector3(0, 0, 30)));
			}
		}*/
	}

	IEnumerator EnterCombatZone(Vector3 args) {
		follow = false;
		Vector3 cameraPosition = transform.position;
		float transition = 0.0f;
		while (transition < transitionTime) {
			transform.position = Vector3.Lerp(cameraPosition, new Vector3(args.x, args.y), transition / transitionTime);
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(cameraSize, args.z, transition / transitionTime);
			transition += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator ExitCombatZone(Vector3 args) {
		float transition = 0.0f;
		while (transition < transitionTime) {
			transform.position = Vector3.Lerp(new Vector3(args.x, args.y), player.GetComponent<Rigidbody2D>().position, transition / transitionTime);
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(args.z, cameraSize, transition / transitionTime);
			transition += Time.deltaTime;
			yield return null;
		}
		follow = true;
	}
}
