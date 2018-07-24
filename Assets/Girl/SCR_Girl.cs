using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Girl : SCR_Enemy {
	public override void OnOutOfScreen() {
		Destroy(gameObject);
	}

	public override void AutoDestroy() {
		base.AutoDestroy();
		SCR_Gameplay.instance.GameOver();
	}
}
