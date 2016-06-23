using UnityEngine;
using System.Collections;

public class AttackHit : MonoBehaviour {
	public HitEffect[] hitEffects;
	public float currentHitEffectIndex;

	void OnCollisionEnter2D(Collision2D col){
		if (currentHitEffectIndex >=0 && currentHitEffectIndex < hitEffects.Length) {
			Vector2 scaleDirection = new Vector2(transform.parent.localScale.x, 1);
			col.rigidbody.velocity = Vector2.Scale(hitEffects [(int)currentHitEffectIndex].velocity, scaleDirection);
		}
	}
//
//	public void SetHitEffect(string _name){
//		if (_name.Equals("None")){
//			currentHitEffect = null;
//			return;
//		}
//		foreach (HitEffect h in hitEffects){
//			if (h.attackName.Equals(_name))
//				currentHitEffect = h;
//		}
//	}

	[System.Serializable]
	public class HitEffect{
		public string attackName;
		public Vector2 velocity;
	}
}
