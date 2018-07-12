using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Gameplay : MonoBehaviour {
	public GameObject[] PFB_ENEMY;

	public GameObject venom;

	public static float screenWidth;
	public static float screenHeight;

	private const float SPAWN_TIME_MIN = 1;
	private const float SPAWN_TIME_MAX = 2;

	private float spawnTime;

	// Use this for initialization
	void Start () {
		screenHeight = 19.2f;
		screenWidth = screenHeight * Screen.width / Screen.height;

		spawnTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			venom.GetComponent<SCR_Venom>().Attack(pos.x, pos.y);

			RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
 			
	        foreach (RaycastHit2D hit in hits) {
	            SCR_Soldier scrSoldier = hit.collider.GetComponent<SCR_Soldier>();
	            if (scrSoldier != null) {
	            	scrSoldier.Die();
	            }
	        }
		}

		if (Input.GetMouseButtonUp(0)) {
			venom.GetComponent<SCR_Venom>().AttackComplete();
		}

		if (Input.GetMouseButtonDown(1)) {
			venom.GetComponent<Animator>().SetTrigger("ultimate");
		}

		spawnTime -= Time.deltaTime;
		if (spawnTime <= 0) {
			GameObject soldier = Instantiate(PFB_ENEMY[Random.Range(0, PFB_ENEMY.Length)]);
			soldier.transform.position = new Vector3(Random.Range(-screenWidth * 0.5f, screenWidth * 0.5f), screenHeight * 0.5f, soldier.transform.position.z);
			spawnTime = Random.Range(SPAWN_TIME_MIN, SPAWN_TIME_MAX);
		}
	}
}
