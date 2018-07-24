using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
	RUN,
	DIE
}
/*
public enum MoveType {
	STRAIGHT,
	DIAGONAL
}
*/
public class SCR_Enemy : MonoBehaviour {
	public const float STRAIGHT_SPEED	= 3;
	public const float DIAGONAL_SPEED_X	= 3;
	public const float DIAGONAL_SPEED_Y	= 5; 
	public const float DIAGONAL_RATE		= 0.5f;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	public EnemyState state = EnemyState.RUN;

	//private MoveType moveType = MoveType.STRAIGHT;

	public float speedX;
	public float speedY;

	// Use this for initialization
	public virtual void Awake() {
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
	public virtual void Update() {
		if (SCR_Gameplay.instance.state == GameState.PLAY) {
			if (state == EnemyState.RUN) {
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
					OnOutOfScreen();
				}
			}
		}
	}

	public virtual void Die() {
		animator.SetTrigger("die");
		spriteRenderer.sortingOrder = 1;
		iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", 0.5f, "delay", 1f, "oncomplete", "AutoDestroy"));
		state = EnemyState.DIE;
	}

	public virtual void AutoDestroy() {
		Destroy(gameObject);
	}

	public virtual float GetSpawnMargin() {
		return GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y;
	}

	public virtual void OnOutOfScreen() {
		SCR_Gameplay.instance.GameOver();
	}
}
