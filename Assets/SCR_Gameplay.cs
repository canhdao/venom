using System.Collections;
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

	public GameObject[]	PFB_ENEMY;
	public GameObject	PFB_DOG;
	public GameObject	PFB_PATTERN;

	public GameObject	venom;

	public GameObject	cvsGameplay;
	public GameObject	cvsGameOver;

	public GameObject	txtScore;
	public GameObject	txtWave;
	public GameObject	txtBest;
	public GameObject	txtKilled;

	public GameObject	title;
	public GameObject	tapToPlay;
	
	public AudioSource	source;
	
	public AudioClip	sndMainMenu;
	public AudioClip	sndGameplay;
	public AudioClip	sndHit;

	public static SCR_Gameplay instance;

	public static float screenWidth;
	public static float screenHeight;

	public GameState state;

	private float spawnTime;

	private int score;
	private int best;

	private InterstitialAd interstitial;

	private static float timeShowAds = TIME_SHOW_ADS;
	
	// Spawn enemies wave by wave
	public bool		spawningEnemies;
	public int		currentWave;
	public int		spawnCount;
	public float	gapTime;

	// Use this for initialization
	void Start() {
		Application.targetFrameRate = 60;
		
		screenHeight = 19.2f;
		screenWidth = screenHeight * Screen.width / Screen.height;

		spawnTime = 0;
		score = 0;
		best = PlayerPrefs.GetInt("best", 0);

		txtScore.SetActive(false);
		txtWave.SetActive(false);

		transform.position = new Vector3(transform.position.x, CAMERA_POSITION_READY, transform.position.z);
		Camera.main.orthographicSize = CAMERA_SIZE_READY;

		cvsGameplay.SetActive(true);
		cvsGameOver.SetActive(false);
		
		source.clip = sndMainMenu;
		source.Play();
		
		spawningEnemies = false;
		spawnCount = 0;
		currentWave = 0;

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
							source.PlayOneShot(sndHit);
		            	}
		            }
		        }
			}

			if (Input.GetMouseButtonDown(1)) {
				venom.GetComponent<Animator>().SetTrigger("special");
			}
			
			if (spawningEnemies) {
				spawnTime -= Time.deltaTime;
				if (spawnTime <= 0) {
					float r = Random.Range(0f, 100f);
					if (r < SCR_Config.RATE_PATTERN_2) {
						SpawnPattern(PatternType.TWO);
					}
					else if (r < SCR_Config.RATE_PATTERN_2 + SCR_Config.RATE_PATTERN_3) {
						SpawnPattern(PatternType.THREE);
					}
					else {
						SpawnPattern(PatternType.ONE);
					}
					spawnTime = Random.Range(SPAWN_TIME_MIN, SPAWN_TIME_MAX);
				}
			}
			else if (currentWave >= 1) {
				gapTime += Time.deltaTime;
				if (gapTime >= SCR_Config.WAVE_GAP_TIME) {
					SpawnWave(currentWave + 1);
				}
			}
		}

		if (state == GameState.READY) {
			if (Input.GetMouseButtonDown(0)) {
				title.SetActive(false);
				tapToPlay.SetActive(false);
				txtScore.SetActive(true);
				SpawnWave(1);
				venom.GetComponent<Animator>().SetTrigger("ultimate");
				ZoomCamera();
				source.clip = sndGameplay;
				source.Play();
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
		txtKilled.GetComponent<Text>().text = "KILLED: " + score.ToString();
		txtBest.GetComponent<Text>().text = "BEST: " + best.ToString();

		cvsGameplay.SetActive(false);
		cvsGameOver.SetActive(true);

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

	private void SpawnPattern(PatternType type) {
		GameObject pattern = Instantiate(PFB_PATTERN);
		SCR_Pattern scrPattern = pattern.GetComponent<SCR_Pattern>();
		scrPattern.Spawn(type);
	}

	private void SpawnWave(int wave) {
		txtWave.GetComponent<Text>().text = "Wave " + wave.ToString();
		txtWave.SetActive(true);
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.3f, "easetype", "easeInOutSine", "onupdate", "UpdateFadeWave"));
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.3f, "delay", 3.0f, "easetype", "easeInOutSine", "onupdate", "UpdateFadeWave", "oncomplete", "CompleteFadeWave"));
		
		spawningEnemies = true;
		spawnCount = 0;
		currentWave = wave;
	}

	private void UpdateFadeWave(float alpha) {
		Text text = txtWave.GetComponent<Text>();
		text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
	}

	private void CompleteFadeWave() {
		txtWave.SetActive(false);
	}
}
