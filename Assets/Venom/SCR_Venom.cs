using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Venom : MonoBehaviour {
	public GameObject PFB_BREAK;

	private const float BREAK_OFFSET_X = 0.25f;
	private const float BREAK_OFFSET_Y = 2.75f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCompleteAnimationUltimate() {
		GameObject effect = Instantiate(PFB_BREAK);
		effect.transform.position = new Vector3(transform.position.x + BREAK_OFFSET_X, transform.position.y + BREAK_OFFSET_Y, transform.position.z);
	}
}
