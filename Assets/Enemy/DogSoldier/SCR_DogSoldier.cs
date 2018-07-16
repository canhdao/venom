using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DogSoldier : SCR_Enemy {
	// Use this for initialization
	public override void Awake() {
		base.Awake();
	}
	
	// Update is called once per frame
	public override void Update() {
		base.Update();
	}

	public override void Die() {
		base.Die();

		GameObject dogLeft = Instantiate(SCR_Gameplay.instance.PFB_DOG);
		dogLeft.transform.position = transform.position;
		dogLeft.GetComponent<SCR_Enemy>().speedX = -DIAGONAL_SPEED_X;
		dogLeft.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y;

		GameObject dogRight = Instantiate(SCR_Gameplay.instance.PFB_DOG);
		dogRight.transform.position = transform.position;
		dogRight.GetComponent<SCR_Enemy>().speedX = DIAGONAL_SPEED_X;
		dogRight.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y;
	}
}
