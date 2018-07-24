using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Venom : MonoBehaviour {
	public GameObject PFB_BREAK;

	public GameObject upperArmLeft;
	public GameObject forearmLeft;
	public GameObject handLeft;

	public GameObject upperArmRight;
	public GameObject forearmRight;
	public GameObject handRight;

	public GameObject tongueStart;
	public GameObject tongueEnd;

	private Animator animator;

	// special
	private const float BREAK_OFFSET_X_SPECIAL	= -1.5f;
	private const float BREAK_OFFSET_Y_SPECIAL	= 3.0f;

	// ultimate
	private const float BREAK_OFFSET_X_ULTIMATE	= 0.25f;
	private const float BREAK_OFFSET_Y_ULTIMATE	= 2.75f;

	// normal attack
	private Vector2 originalVectorLeft;
	private Vector3 originalScaleLeft;

	private Vector2 originalVectorRight;
	private Vector3 originalScaleRight;

	private Vector2 originalVectorTongue;
	private Vector3 originalScaleTongue;

	private float attackTime;

	void Awake() {
		upperArmLeft.SetActive(false);
		forearmLeft.SetActive(false);

		upperArmRight.SetActive(false);
		forearmRight.SetActive(false);

		tongueStart.SetActive(false);
	}

	// Use this for initialization
	void Start() {
		originalVectorLeft = new Vector2(handLeft.transform.position.x - forearmLeft.transform.position.x, handLeft.transform.position.y - forearmLeft.transform.position.y);
		originalScaleLeft = forearmLeft.transform.localScale;

		originalVectorRight = new Vector2(handRight.transform.position.x - forearmRight.transform.position.x, handRight.transform.position.y - forearmRight.transform.position.y);
		originalScaleRight = forearmRight.transform.localScale;

		originalVectorTongue = new Vector2(tongueEnd.transform.position.x - tongueStart.transform.position.x, tongueEnd.transform.position.y - tongueStart.transform.position.y);
		originalScaleTongue = tongueStart.transform.localScale;

		animator = GetComponent<Animator>();

		attackTime = -1;
	}
	
	// Update is called once per frame
	void Update() {
		if (attackTime >= 0) {
			attackTime += Time.deltaTime;
			if (attackTime >= 0.3f) {
				AttackComplete();
				attackTime = -1;
			}
		}
	}

	private void OnShowBreakSpecial() {
		GameObject effect = Instantiate(PFB_BREAK);
		effect.transform.position = new Vector3(
			transform.position.x + BREAK_OFFSET_X_SPECIAL,
			transform.position.y + BREAK_OFFSET_Y_SPECIAL,
			transform.position.z);
	}
	
	private void OnShowBreakUltimate() {
		GameObject effect = Instantiate(PFB_BREAK);
		effect.transform.position = new Vector3(
			transform.position.x + BREAK_OFFSET_X_ULTIMATE,
			transform.position.y + BREAK_OFFSET_Y_ULTIMATE,
			transform.position.z);
	}

	public void Attack(float x, float y) {
		GameObject forearm = null;
		GameObject upperArm = null;
		Vector2 originalVector;
		Vector3 originalScale;

		float TONGUE_LEFT = -SCR_Gameplay.screenWidth / 6;
		float TONGUE_RIGHT = -TONGUE_LEFT;

		AttackComplete();
		
		if (x < TONGUE_LEFT) {
			animator.SetBool("attackLeft", true);
			forearm = forearmLeft;
			upperArm = upperArmLeft;
			originalVector = originalVectorLeft;
			originalScale = originalScaleLeft;
		}
		else if (x > TONGUE_RIGHT) {
			animator.SetBool("attackRight", true);
			forearm = forearmRight;
			upperArm = upperArmRight;
			originalVector = originalVectorRight;
			originalScale = originalScaleRight;
		}
		else {
			animator.SetBool("attackTongue", true);
			originalVector = originalVectorTongue;
			originalScale = originalScaleTongue;
		}

		if (x < TONGUE_LEFT || x > TONGUE_RIGHT) {
			Vector2 targetVector = new Vector2(x - forearm.transform.position.x, y - forearm.transform.position.y);

			float angle = Vector2.SignedAngle(originalVector, targetVector);
			forearm.transform.localEulerAngles = new Vector3(0, 0, angle);

			forearm.transform.localScale = new Vector3(
				originalScale.x,
				Mathf.Sqrt(targetVector.sqrMagnitude / originalVector.sqrMagnitude) * originalScale.y,
				originalScale.z);

			upperArm.transform.localEulerAngles = forearm.transform.localEulerAngles;
			
			upperArm.SetActive(true);
			forearm.SetActive(true);
		}
		else {
			Vector2 targetVector = new Vector2(x - tongueStart.transform.position.x, y - tongueStart.transform.position.y);

			float angle = Vector2.SignedAngle(originalVector, targetVector);
			tongueStart.transform.localEulerAngles = new Vector3(0, 0, angle);

			tongueStart.transform.localScale = new Vector3(
				originalScale.x,
				Mathf.Sqrt(targetVector.sqrMagnitude / originalVector.sqrMagnitude) * originalScale.y,
				originalScale.z);

			tongueStart.SetActive(true);
		}

		attackTime = 0;
	}

	private void AttackComplete() {
		forearmLeft.SetActive(false);
		upperArmLeft.SetActive(false);

		forearmRight.SetActive(false);
		upperArmRight.SetActive(false);

		tongueStart.SetActive(false);

		animator.SetBool("attackLeft", false);
		animator.SetBool("attackRight", false);
		animator.SetBool("attackTongue", false);
	}
}
