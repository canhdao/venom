using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Venom : MonoBehaviour {
	public GameObject PFB_BREAK;

	public GameObject upperArm;
	public GameObject forearm;
	public GameObject hand;

	private Animator animator;

	private const float BREAK_OFFSET_X = 0.25f;
	private const float BREAK_OFFSET_Y = 2.75f;

	private Vector2 originalVector;
	private Vector3 originalScale;

	// Use this for initialization
	void Start() {
		upperArm.SetActive(false);
		forearm.SetActive(false);
		originalVector = new Vector2(hand.transform.position.x - forearm.transform.position.x, hand.transform.position.y - forearm.transform.position.y);
		originalScale = forearm.transform.localScale;

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
		Vector2 targetVector = new Vector2(x - forearm.transform.position.x, y - forearm.transform.position.y);

		float angle = Vector2.SignedAngle(originalVector, targetVector);
		forearm.transform.localEulerAngles = new Vector3(0, 0, angle);

		forearm.transform.localScale = new Vector3(
			originalScale.x,
			Mathf.Sqrt(targetVector.sqrMagnitude / originalVector.sqrMagnitude) * originalScale.y,
			originalScale.z);

		forearm.SetActive(true);

		animator.SetTrigger("attack");

		upperArm.transform.localEulerAngles = forearm.transform.localEulerAngles;
		upperArm.SetActive(true);
	}

	public void AttackComplete() {
		forearm.SetActive(false);
		upperArm.SetActive(false);

		animator.SetTrigger("idle");
	}
}
