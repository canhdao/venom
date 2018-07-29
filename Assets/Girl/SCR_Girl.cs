using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Girl : SCR_Enemy {
	public override void OnOutOfScreen() {
		Destroy(gameObject);
	}

	public override void Die() {
		animator.SetTrigger("die");
		Invoke("AutoDestroy", 3f);
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
		state = EnemyState.DIE;

		SCR_Gameplay.instance.ShowSpot(transform.position);
	}

	public override void AutoDestroy() {
		base.AutoDestroy();
		SCR_Gameplay.instance.GameOver();
	}
}
