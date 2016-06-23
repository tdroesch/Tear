using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour
{
		void OnTriggerEnter2D (Collider2D col)
		{
				if (col.tag == "Player") {
						GameEventManager.OnGameOver ();
//						StartCoroutine (Restart ());
				}
				if (col.tag == "Enemy") {
						Destroy (col.gameObject);
				}
		}

		IEnumerator Restart ()
		{
				yield return new WaitForSeconds (10);
				GameEventManager.OnGameEnd ();
		}
}
