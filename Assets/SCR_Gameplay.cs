﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public enum GameState {
	READY,
	PLAY,
	FINISH
}

public class SCR_Gameplay : MonoBehaviour {
	private const float TIME_SHOW_ADS			= 15;

	private const float SPAWN_TIME_MIN			= 1;
	private const float SPAWN_TIME_MAX			= 2;

	private const float CAMERA_SIZE_READY		= 4.8f;
	private const float CAMERA_SIZE_PLAY		= 9.6f;

	private const float CAMERA_POSITION_READY	= -4.8f;
	private const float CAMERA_POSITION_PLAY	= 0;

	public GameObject[] PFB_ENEMY;
	public GameObject PFB_DOG;

	public GameObject venom;
	public GameObject txtScore;
	public GameObject txtBest;

	public GameObject title;
	public GameObject tapToPlay;

	public static SCR_Gameplay instance;

	public static float screenWidth;
	public static float screenHeight;

	public GameState state;

	private float spawnTime;

	private int score;
	private int best;

	private InterstitialAd interstitial;

	private static float timeShowAds = TIME_SHOW_ADS;

	// Use this for initialization
	void Start() {
		Application.targetFrameRate = 60;
		
		screenHeight = 19.2f;
		screenWidth = screenHeight * Screen.width / Screen.height;

		spawnTime = 0;
		score = 0;
		best = PlayerPrefs.GetInt("best", 0);

		txtScore.SetActive(false);
		txtBest.SetActive(false);

		transform.position = new Vector3(transform.position.x, CAMERA_POSITION_READY, transform.position.z);
		Camera.main.orthographicSize = CAMERA_SIZE_READY;

		state = GameState.READY;

		instance = this;

		#if UNITY_ANDROID
			string appId = "ca-app-pub-0081066185741622~1212788759";
		#elif UNITY_IPHONE
			string appId = "ca-app-pub-0081066185741622~2263405726";
		#else
			string appId = "unexpected_platform";
		#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);

		RequestInterstitial();
	}
	
	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
			string adUnitId = "ca-app-pub-0081066185741622/3435202389";
		#elif UNITY_IPHONE
			string adUnitId = "ca-app-pub-0081066185741622/4481949928";
		#else
			string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddTestDevice("f76690eb0615cccc73b4c57165f1621e").AddTestDevice("36e6813a9776d338128e27da33a0467f").Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}
	
	private void ShowAds() {
		interstitial.Show();
		interstitial.Destroy();
		RequestInterstitial();
		timeShowAds = 0;
	}

	// Update is called once per frame
	void Update() {
		timeShowAds += Time.unscaledDeltaTime;

		if (state == GameState.PLAY) {
			if (Input.GetMouseButtonDown(0)) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				venom.GetComponent<SCR_Venom>().Attack(pos.x, pos.y);

				RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
	 			
		        foreach (RaycastHit2D hit in hits) {
		            SCR_Enemy scrEnemy = hit.collider.GetComponent<SCR_Enemy>();
		            if (scrEnemy != null) {
		            	if (scrEnemy.state == EnemyState.RUN) {
		            		scrEnemy.Die();
		            		IncreaseScore();
		            	}
		            }
		        }
			}

			//if (Input.GetMouseButtonUp(0)) {
			//	venom.GetComponent<SCR_Venom>().AttackComplete();
			//}

			if (Input.GetMouseButtonDown(1)) {
				venom.GetComponent<Animator>().SetTrigger("ultimate");
			}

			spawnTime -= Time.deltaTime;
			if (spawnTime <= 0) {
				SpawnEnemy();
				spawnTime = Random.Range(SPAWN_TIME_MIN, SPAWN_TIME_MAX);
			}
		}

		if (state == GameState.READY) {
			if (Input.GetMouseButtonDown(0)) {
				title.SetActive(false);
				tapToPlay.SetActive(false);
				txtScore.SetActive(true);
				ZoomCamera();
				state = GameState.PLAY;
			}
		}
		
		if (state == GameState.FINISH) {
			if (Input.GetMouseButtonDown(0)) {
				SceneManager.LoadScene("SCN_Gameplay");
			}
		}
	}

	public void IncreaseScore() {
		score++;
		txtScore.GetComponent<Text>().text = score.ToString();

		if (best < score) {
			best = score;
		}
	}

	public void GameOver() {
		txtBest.GetComponent<Text>().text = "BEST " + best.ToString();
		txtBest.SetActive(true);
		state = GameState.FINISH;

		if (timeShowAds >= TIME_SHOW_ADS && interstitial.IsLoaded()) {
			ShowAds();
		}
	}

	private void ZoomCamera() {
		iTween.MoveTo(gameObject, iTween.Hash("y", CAMERA_POSITION_PLAY, "time", 0.5f, "easetype", "easeOutSine"));
		iTween.ValueTo(gameObject, iTween.Hash("from", CAMERA_SIZE_READY, "to", CAMERA_SIZE_PLAY, "time", 0.5f, "easetype", "easeOutSine", "onupdate", "UpdateCameraSize"));
	}

	private void UpdateCameraSize(float size) {
		Camera.main.orthographicSize = size;
	}

	private void SpawnEnemy() {
		GameObject prefab = PFB_ENEMY[Random.Range(0, PFB_ENEMY.Length)];
		GameObject enemy = Instantiate(prefab);
		float margin = prefab.GetComponent<SCR_Enemy>().GetSpawnMargin();
		enemy.transform.position = new Vector3(Random.Range(-screenWidth * 0.5f, screenWidth * 0.5f), screenHeight * 0.5f + margin, enemy.transform.position.z);
	}
}
