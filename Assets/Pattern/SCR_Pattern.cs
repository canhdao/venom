using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternType {
	ONE,
	TWO,
	THREE,
	LINE
}

public class SCR_Pattern : MonoBehaviour {
	public PatternType type = PatternType.ONE;
	
	void Start() {
		
	}
	
	void Update() {
		if (transform.childCount <= 0) {
			Destroy(gameObject);
		}
	}
	
	public void Spawn(PatternType type) {
		float distance = 2;
		float x = Random.Range(-SCR_Gameplay.screenWidth * 0.3f, SCR_Gameplay.screenWidth * 0.3f);
		float y = SCR_Gameplay.screenHeight * 0.5f;
		float noise = SCR_Gameplay.screenHeight * 0.05f;
		
		if (type == PatternType.ONE) {
			SpawnEnemy();
		}

		if (type == PatternType.TWO) {
			SpawnEnemy(x - distance * 0.5f, y + Random.Range(0f, noise));
			SpawnEnemy(x + distance * 0.5f, y + Random.Range(0f, noise));
		}

		if (type == PatternType.THREE) {
			SpawnEnemy(x - distance, y + Random.Range(0f, noise));
			SpawnEnemy(x, y + Random.Range(0f, noise));
			SpawnEnemy(x + distance, y + Random.Range(0f, noise));
		}

		if (type == PatternType.LINE) {
			MoveType moveType = MoveType.STRAIGHT;

			float r = Random.Range(0f, 1f);
			if (r < 1.0f / 3.0f) {
				moveType = MoveType.STRAIGHT;
				SpawnEnemy(x + Random.Range(-noise * 0.5f, noise * 0.5f), y);
				SpawnEnemy(x + Random.Range(-noise * 0.5f, noise * 0.5f), y + distance);
				SpawnEnemy(x + Random.Range(-noise * 0.5f, noise * 0.5f), y + 2 * distance);
			}
			else if (r < 2.0f / 3.0f) {
				moveType = MoveType.DIAGONAL_LEFT;
				SpawnEnemy(x, y);
				SpawnEnemy(x + distance, y + distance);
				SpawnEnemy(x + 2 * distance, y + 2 * distance);
			}
			else {
				moveType = MoveType.DIAGONAL_RIGHT;
				SpawnEnemy(x, y);
				SpawnEnemy(x - distance, y + distance);
				SpawnEnemy(x - 2 * distance, y + 2 * distance);
			}

			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild(i).GetComponent<SCR_Enemy>().SetMoveType(moveType);
			}
		}
		
		if (type != PatternType.LINE) {
			MoveType moveType = transform.GetChild(0).GetComponent<SCR_Enemy>().GetMoveType();
			for (int i = 1; i < transform.childCount; i++) {
				transform.GetChild(i).GetComponent<SCR_Enemy>().SetMoveType(moveType);
			}
		}
	}
	
	private GameObject SpawnEnemy(float x, float y) {
		GameObject enemy = SpawnEnemy();
		enemy.transform.position = new Vector3(x, y, enemy.transform.position.z);
		return enemy;
	}

	private GameObject SpawnEnemy() {
		int wave = SCR_Gameplay.instance.currentWave - 1;
		if (wave > SCR_Config.NUMBER_ENEMIES.Length - 1) wave = SCR_Config.NUMBER_ENEMIES.Length - 1;
		
		float rateSoldier1		= SCR_Config.RATE_SOLDIER_1[wave];
		float rateSoldier2		= SCR_Config.RATE_SOLDIER_2[wave];
		float rateSoldier3		= SCR_Config.RATE_SOLDIER_3[wave];
		float rateSoldier4		= SCR_Config.RATE_SOLDIER_4[wave];
		float rateDogSoldier	= SCR_Config.RATE_DOG_SOLDIER[wave];
		float rateTruck			= SCR_Config.RATE_TRUCK[wave];
		//float rateGirl			= SCR_Config.RATE_GIRL[wave];
		
		float r = Random.Range(0.0f, 100.0f);
		int choose = 0;
		
		if (r < rateSoldier1) {
			choose = 0;
		}
		else if (r < rateSoldier1 + rateSoldier2) {
			choose = 1;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3) {
			choose = 2;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3 + rateSoldier4) {
			choose = 3;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3 + rateSoldier4 + rateDogSoldier) {
			choose = 4;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3 + rateSoldier4 + rateDogSoldier + rateTruck) {
			choose = 5;
		}
		else {
			choose = 6;
		}
		
		GameObject prefab = SCR_Gameplay.instance.PFB_ENEMY[choose];
		GameObject enemy = Instantiate(prefab);
		SCR_Enemy scrEnemy = enemy.GetComponent<SCR_Enemy>();
		
		enemy.transform.parent = transform;
		
		float marginX = scrEnemy.GetSpawnMarginX();
		float marginY = scrEnemy.GetSpawnMarginY();
		enemy.transform.position = new Vector3(
			Random.Range(-SCR_Gameplay.screenWidth * 0.5f + marginX, SCR_Gameplay.screenWidth * 0.5f - marginX),
			SCR_Gameplay.screenHeight * 0.5f + marginY,
			enemy.transform.position.z);
		
		SCR_Gameplay.instance.spawnCount++;
		if (SCR_Gameplay.instance.spawnCount >= SCR_Config.NUMBER_ENEMIES[wave]) {
			SCR_Gameplay.instance.spawningEnemies = false;
			SCR_Gameplay.instance.gapTime = 0;
		}

		return enemy;
	}	
}
