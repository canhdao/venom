using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Truck : SCR_Enemy {
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

		GameObject dogMiddle = Instantiate(SCR_Gameplay.instance.PFB_DOG);
		dogMiddle.transform.position = transform.position;
		dogMiddle.GetComponent<SCR_Enemy>().speedX = 0;
		dogMiddle.GetComponent<SCR_Enemy>().speedY = 2 * STRAIGHT_SPEED;

		GameObject dogRight = Instantiate(SCR_Gameplay.instance.PFB_DOG);
		dogRight.transform.position = transform.position;
		dogRight.GetComponent<SCR_Enemy>().speedX = DIAGONAL_SPEED_X;
		dogRight.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y;
		
		GameObject dogMiddleLeft = Instantiate(SCR_Gameplay.instance.PFB_DOG);
		dogMiddleLeft.transform.position = transform.position;
		dogMiddleLeft.GetComponent<SCR_Enemy>().speedX = -DIAGONAL_SPEED_X * 0.5f;
		dogMiddleLeft.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y * 1.2f;
		
		GameObject dogMiddleRight = Instantiate(SCR_Gameplay.instance.PFB_DOG);
		dogMiddleRight.transform.position = transform.position;
		dogMiddleRight.GetComponent<SCR_Enemy>().speedX = DIAGONAL_SPEED_X * 0.5f;
		dogMiddleRight.GetComponent<SCR_Enemy>().speedY = DIAGONAL_SPEED_Y * 1.2f;
	}

	public override float GetSpawnMargin() {
		return GetComponent<BoxCollider2D>().size.y * transform.localScale.y;
	}
}
