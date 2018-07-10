using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Soldier : MonoBehaviour {
	private const int MOVE_SPEED = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, transform.position.y - MOVE_SPEED * Time.deltaTime, transform.position.z);
		if (transform.position.y < -SCR_Gameplay.screenHeight * 0.5f) {
			Destroy(gameObject);
		}
	}
}
