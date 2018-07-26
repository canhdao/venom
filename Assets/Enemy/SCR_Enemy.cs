using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
	RUN,
	DIE
}

public enum MoveType {
	STRAIGHT,
	DIAGONAL_LEFT,
	DIAGONAL_RIGHT
	// ZIG_ZAG // to-do
	// CIRCLE
}

public enum Speed {
	NORMAL,
	FAST
}

public class SCR_Enemy : MonoBehaviour {
	public const float STRAIGHT_SPEED		= 3;
	public const float DIAGONAL_SPEED_X		= 3;
	public const float DIAGONAL_SPEED_Y		= 5; 
	public const float DIAGONAL_RATE		= 0.5f;

	public const float BOOST_SPEED_RATE		= 0.2f;
	public const float BOOST_SPEED_Y_START	= 0.4f;
	public const float BOOST_SPEED_Y_END	= 0.8f;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	public EnemyState state = EnemyState.RUN;

	private MoveType moveType = MoveType.STRAIGHT;
	private Speed speed = Speed.NORMAL;

	public float speedX;
	public float speedY;
	
	private float normalSpeedX;
	private float normalSpeedY;
	
	private float fastSpeedX;
	private float fastSpeedY;

	private bool boostSpeed;
	private float boostSpeedY;
	
	// Use this for initialization
	public virtual void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

		float r = Random.Range(0f, 1f);
		if (r < DIAGONAL_RATE) {
			float d = Random.Range(0f, 1f);
			if (d < 0.5f) {
				SetMoveType(MoveType.DIAGONAL_LEFT);
			}
			else {
				SetMoveType(MoveType.DIAGONAL_RIGHT);
			}
		}
		else {
			SetMoveType(MoveType.STRAIGHT);
		}

		r = Random.Range(0f, 1f);
		if (r < BOOST_SPEED_RATE) {
			boostSpeed = true;
			boostSpeedY = (Random.Range(BOOST_SPEED_Y_START, BOOST_SPEED_Y_END) - 0.5f) * SCR_Gameplay.screenHeight;
		}
		else {
			boostSpeed = false;
		}
	}
	
	// Update is called once per frame
	public virtual void Update() {
		if (SCR_Gameplay.instance.state == GameState.PLAY) {
			if (state == EnemyState.RUN) {
				float newX = transform.position.x + speedX * Time.deltaTime;
				float newY = transform.position.y - speedY * Time.deltaTime;
				transform.position = new Vector3(newX, newY, newY);

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

				if (boostSpeed) {
					if (transform.position.y <= boostSpeedY) {
						SetSpeed(Speed.FAST);
						boostSpeed = false;
					}
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

	public virtual float GetSpawnMarginX() {
		return GetComponent<CapsuleCollider2D>().size.x * transform.localScale.x;
	}

	public virtual float GetSpawnMarginY() {
		return GetComponent<CapsuleCollider2D>().size.y * transform.localScale.y;
	}

	public virtual void OnOutOfScreen() {
		SCR_Gameplay.instance.GameOver();
	}
	
	public virtual MoveType GetMoveType() {
		return moveType;
	}
	
	public virtual void SetMoveType(MoveType mt) {
		moveType = mt;
		
		if (moveType == MoveType.STRAIGHT) {
			speedX = 0;
			speedY = STRAIGHT_SPEED;
		}
		
		if (moveType == MoveType.DIAGONAL_LEFT) {
			speedX = -DIAGONAL_SPEED_X;
			speedY = DIAGONAL_SPEED_Y;
		}
		
		if (moveType == MoveType.DIAGONAL_RIGHT) {
			speedX = DIAGONAL_SPEED_X;
			speedY = DIAGONAL_SPEED_Y;
		}
		
		normalSpeedX = Mathf.Abs(speedX);
		normalSpeedY = Mathf.Abs(speedY);
		
		fastSpeedX = 2 * normalSpeedX;
		fastSpeedY = 2 * normalSpeedY;
	}
	
	public virtual void SetSpeed(Speed s) {
		speed = s;

		int r = 0;
		
		if (speed == Speed.NORMAL) {
			r = Random.Range(0, 2);
			if (r == 0) r = -1;
			speedX = r * normalSpeedX;
			speedY = normalSpeedY;
		}
		else {
			r = Random.Range(0, 2);
			if (r == 0) r = -1;
			speedX = r * fastSpeedX;
			speedY = fastSpeedY;
		}
	}
}
