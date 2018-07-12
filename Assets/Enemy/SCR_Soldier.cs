using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoldierState {
	RUN,
	DIE
}
/*
public enum MoveType {
	STRAIGHT,
	DIAGONAL
}
*/
public class SCR_Soldier : MonoBehaviour {
	private const float	STRAIGHT_SPEED		= 3;
	private const float DIAGONAL_SPEED_X	= 3f;
	private const float DIAGONAL_SPEED_Y	= 5; 
	private const float	DIAGONAL_RATE		= 0.5f;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private SoldierState state = SoldierState.RUN;

	//private MoveType moveType = MoveType.STRAIGHT;

	private float speedX;
	private float speedY;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

		float r = Random.Range(0f, 1f);
		if (r < DIAGONAL_RATE) {
			//moveType = MoveType.DIAGONAL;
			r = Random.Range(0f, 1f);
			if (r < 0.5f) {
				speedX = -DIAGONAL_SPEED_X;
			}
			else {
				speedX = DIAGONAL_SPEED_X;
			}
			speedY = DIAGONAL_SPEED_Y;
		}
		else {
			//moveType = MoveType.STRAIGHT;
			speedX = 0;
			speedY = STRAIGHT_SPEED;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (state == SoldierState.RUN) {
			float newX = transform.position.x + speedX * Time.deltaTime;
			float newY = transform.position.y - speedY * Time.deltaTime;
			transform.position = new Vector3(newX, newY, transform.position.z);

			if (newX > SCR_Gameplay.screenWidth * 0.5f) {
				speedX = -Mathf.Abs(speedX);
			}

			if (newX < -SCR_Gameplay.screenWidth * 0.5f) {
				speedX = Mathf.Abs(speedX);
			}

			spriteRenderer.sortingOrder = (int)(SCR_Gameplay.screenHeight * 0.5f - transform.position.y) + 2;
			if (transform.position.y < -SCR_Gameplay.screenHeight * 0.5f) {
				Destroy(gameObject);
			}
		}
	}

	public void Die() {
		animator.SetTrigger("die");
		spriteRenderer.sortingOrder = 1;
		iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", 0.5f, "delay", 1f, "oncomplete", "AutoDestroy"));
		state = SoldierState.DIE;
	}

	private void AutoDestroy() {
		Destroy(gameObject);
	}
}
