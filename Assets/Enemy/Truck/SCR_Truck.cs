using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Truck : SCR_Enemy {
	public GameObject PFB_DOG;

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

		GameObject dogLeft = Instantiate(PFB_DOG);
		dogLeft.transform.position = transform.position;
		dogLeft.GetComponent<SCR_Enemy>().speedX = -DIAGONAL_SPEED_X;
		dogLeft.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y;

		GameObject dogMiddle = Instantiate(PFB_DOG);
		dogMiddle.transform.position = transform.position;
		dogMiddle.GetComponent<SCR_Enemy>().speedX = 0;
		dogMiddle.GetComponent<SCR_Enemy>().speedY = 2 * STRAIGHT_SPEED;

		GameObject dogRight = Instantiate(PFB_DOG);
		dogRight.transform.position = transform.position;
		dogRight.GetComponent<SCR_Enemy>().speedX = DIAGONAL_SPEED_X;
		dogRight.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y;
	}
}
