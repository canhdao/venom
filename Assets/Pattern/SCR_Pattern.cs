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
	public const int ENEMY_SOLDIER_1	= 0;
	public const int ENEMY_SOLDIER_2	= 1;
	public const int ENEMY_SOLDIER_3	= 2;
	public const int ENEMY_SOLDIER_4	= 3;
	public const int ENEMY_DOG_SOLDIER	= 4;
	public const int ENEMY_TRUCK		= 5;
	public const int ENEMY_GIRL			= 6;

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
				SpawnEnemy(x + 3 * distance, y + 3 * distance);
				SpawnEnemy(x + 4 * distance, y + 4 * distance);
			}
			else {
				moveType = MoveType.DIAGONAL_RIGHT;
				SpawnEnemy(x, y);
				SpawnEnemy(x - distance, y + distance);
				SpawnEnemy(x - 2 * distance, y + 2 * distance);
				SpawnEnemy(x - 3 * distance, y + 3 * distance);
				SpawnEnemy(x - 4 * distance, y + 4 * distance);
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
		
		int choose = ChooseEnemy(wave);

		if (SCR_Truck.timeFromLastSpawn < SCR_Config.TRUCK_SPAWN_GAP_TIME) {
			while (choose == ENEMY_TRUCK) {
				choose = ChooseEnemy(wave);
			}
		}

		if (choose == ENEMY_TRUCK) {
			SCR_Truck.timeFromLastSpawn = 0;
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

	private int ChooseEnemy(int wave) {
		float rateSoldier1		= SCR_Config.RATE_SOLDIER_1[wave];
		float rateSoldier2		= SCR_Config.RATE_SOLDIER_2[wave];
		float rateSoldier3		= SCR_Config.RATE_SOLDIER_3[wave];
		float rateSoldier4		= SCR_Config.RATE_SOLDIER_4[wave];
		float rateDogSoldier	= SCR_Config.RATE_DOG_SOLDIER[wave];
		float rateTruck			= SCR_Config.RATE_TRUCK[wave];
		//float rateGirl			= SCR_Config.RATE_GIRL[wave];
		
		float r = Random.Range(0.0f, 100.0f);
		int choose = ENEMY_SOLDIER_1;
		
		if (r < rateSoldier1) {
			choose = ENEMY_SOLDIER_1;
		}
		else if (r < rateSoldier1 + rateSoldier2) {
			choose = ENEMY_SOLDIER_2;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3) {
			choose = ENEMY_SOLDIER_3;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3 + rateSoldier4) {
			choose = ENEMY_SOLDIER_4;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3 + rateSoldier4 + rateDogSoldier) {
			choose = ENEMY_DOG_SOLDIER;
		}
		else if (r < rateSoldier1 + rateSoldier2 + rateSoldier3 + rateSoldier4 + rateDogSoldier + rateTruck) {
			choose = ENEMY_TRUCK;
		}
		else {
			choose = ENEMY_GIRL;
		}

		return choose;
	}
}
