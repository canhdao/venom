using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Break : MonoBehaviour {
	private const float FULL_ALPHA_TIME = 0.5f;

	private float fullAlphaTime;
	private bool fading;

	// Use this for initialization
	void Start () {
		fullAlphaTime = FULL_ALPHA_TIME;
		fading = false;
	}
	
	// Update is called once per frame
	void Update () {
		fullAlphaTime -= Time.deltaTime;
		if (fullAlphaTime <= 0 && !fading) {
			iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", 1.0f, "easetype", "easeInOutSine", "oncomplete", "AutoDestroy"));
			fading = true;
		}
	}

	private void AudoDestroy() {
		Destroy(gameObject);
	}
}
