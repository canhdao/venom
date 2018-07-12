using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Venom : MonoBehaviour {
	public GameObject PFB_BREAK;

	public GameObject arm;
	public GameObject hand;

	private Animator animator;

	private const float BREAK_OFFSET_X = 0.25f;
	private const float BREAK_OFFSET_Y = 2.75f;

	private Vector2 originalVector;
	private Vector3 originalScale;

	// Use this for initialization
	void Start() {
		arm.SetActive(false);
		originalVector = new Vector2(hand.transform.position.x - arm.transform.position.x, hand.transform.position.y - arm.transform.position.y);
		originalScale = arm.transform.localScale;

		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() {
	}

	public void OnCompleteAnimationUltimate() {
		GameObject effect = Instantiate(PFB_BREAK);
		effect.transform.position = new Vector3(transform.position.x + BREAK_OFFSET_X, transform.position.y + BREAK_OFFSET_Y, transform.position.z);
	}

	public void Attack(float x, float y) {
		Vector2 targetVector = new Vector2(x - arm.transform.position.x, y - arm.transform.position.y);

		float angle = Vector2.SignedAngle(originalVector, targetVector);
		arm.transform.localEulerAngles = new Vector3(0, 0, angle);

		arm.transform.localScale = new Vector3(
			originalScale.x,
			Mathf.Sqrt(targetVector.sqrMagnitude / originalVector.sqrMagnitude) * originalScale.y,
			originalScale.z);

		arm.SetActive(true);

		animator.SetTrigger("attack");
	}

	public void AttackComplete() {
		arm.SetActive(false);

		animator.SetTrigger("idle");
	}
}
