using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Soldier : MonoBehaviour {
	private const int MOVE_SPEED = 1;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, transform.position.y - MOVE_SPEED * Time.deltaTime, transform.position.z);

		spriteRenderer.sortingOrder = (int)(SCR_Gameplay.screenHeight * 0.5f - transform.position.y) + 1;
		if (transform.position.y < -SCR_Gameplay.screenHeight * 0.5f) {
			Destroy(gameObject);
		}
	}
}
