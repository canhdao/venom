using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {
	PLAY,
	FINISH
}

public class SCR_Gameplay : MonoBehaviour {
	public GameObject[] PFB_ENEMY;
	public GameObject PFB_DOG;

	public GameObject venom;
	public GameObject txtScore;
	public GameObject txtBest;

	public static SCR_Gameplay instance;

	public static float screenWidth;
	public static float screenHeight;

	public GameState state;

	private const float SPAWN_TIME_MIN = 1;
	private const float SPAWN_TIME_MAX = 2;

	private float spawnTime;

	private int score;
	private int best;

	// Use this for initialization
	void Start() {
		Application.targetFrameRate = 60;
		
		screenHeight = 19.2f;
		screenWidth = screenHeight * Screen.width / Screen.height;

		spawnTime = 0;
		score = 0;
		best = PlayerPrefs.GetInt("best", 0);

		txtBest.SetActive(false);

		state = GameState.PLAY;

		instance = this;
	}
	
	// Update is called once per frame
	void Update() {
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
				GameObject enemy = Instantiate(PFB_ENEMY[Random.Range(0, PFB_ENEMY.Length)]);
				enemy.transform.position = new Vector3(Random.Range(-screenWidth * 0.5f, screenWidth * 0.5f), screenHeight * 0.5f, enemy.transform.position.z);
				spawnTime = Random.Range(SPAWN_TIME_MIN, SPAWN_TIME_MAX);
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
	}
}
