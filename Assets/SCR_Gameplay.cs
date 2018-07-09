using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Gameplay : MonoBehaviour {
	public GameObject venom;

	// Use this for initialization
	void Start () {
		/*
		const float REF_WIDTH = 1080;
		// const float REF_HEIGHT = 1920;

		// Fixed width
		float h = (float)Screen.height / Screen.width * REF_WIDTH;
		Camera.main.orthographicSize = 0.005f * h;
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			venom.GetComponent<Animator>().SetTrigger("ultimate");
		}
	}
}
