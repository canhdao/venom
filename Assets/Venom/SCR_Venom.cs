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

	private Animator animator;

	// ultimate
	private const float BREAK_OFFSET_X = 0.25f;
	private const float BREAK_OFFSET_Y = 2.75f;

	// normal attack
	private Vector2 originalVectorLeft;
	private Vector3 originalScaleLeft;

	private Vector2 originalVectorRight;
	private Vector3 originalScaleRight;

	private float attackTime;

	// Use this for initialization
	void Start() {
		upperArmLeft.SetActive(false);
		forearmLeft.SetActive(false);

		upperArmRight.SetActive(false);
		forearmRight.SetActive(false);

		originalVectorLeft = new Vector2(handLeft.transform.position.x - forearmLeft.transform.position.x, handLeft.transform.position.y - forearmLeft.transform.position.y);
		originalScaleLeft = forearmLeft.transform.localScale;

		originalVectorRight = new Vector2(handRight.transform.position.x - forearmRight.transform.position.x, handRight.transform.position.y - forearmRight.transform.position.y);
		originalScaleRight = forearmRight.transform.localScale;

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

	private void OnCompleteAnimationUltimate() {
		GameObject effect = Instantiate(PFB_BREAK);
		effect.transform.position = new Vector3(transform.position.x + BREAK_OFFSET_X, transform.position.y + BREAK_OFFSET_Y, transform.position.z);
	}

	public void Attack(float x, float y) {
		GameObject forearm = null;
		GameObject upperArm = null;
		Vector2 originalVector;
		Vector3 originalScale;

		if (x < 0) {
			animator.SetBool("attackLeft", true);
			forearm = forearmLeft;
			upperArm = upperArmLeft;
			originalVector = originalVectorLeft;
			originalScale = originalScaleLeft;
		}
		else {
			animator.SetBool("attackRight", true);
			forearm = forearmRight;
			upperArm = upperArmRight;
			originalVector = originalVectorRight;
			originalScale = originalScaleRight;
		}

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

		attackTime = 0;
	}

	private void AttackComplete() {
		forearmLeft.SetActive(false);
		upperArmLeft.SetActive(false);

		forearmRight.SetActive(false);
		upperArmRight.SetActive(false);

		animator.SetBool("attackLeft", false);
		animator.SetBool("attackRight", false);
	}
}
