using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphQuestionEnemySpawner2 : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDistance = 8f;
    public int seed = 1234;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        Random.InitState(seed);
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(0, spawnDistance, 0), // Above
            new Vector3(0, -spawnDistance, 0), // Below
            new Vector3(-spawnDistance, 0, 0), // Left
            new Vector3(spawnDistance, 0, 0) // Right
        };

        GameObject[] enemies = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, player.transform.position + positions[i], Quaternion.identity);
            enemies[i].name = "Enemy " + (i + 1);
        }

        // Shooting directions
        ShootTowards(enemies[0], enemies[1].transform.position);
        ShootTowards(enemies[1], enemies[2].transform.position);
        ShootTowards(enemies[2], enemies[3].transform.position);
    }

    void ShootTowards(GameObject shooter, Vector3 target)
    {
        GraphQuestionEnemy2 enemyScript = shooter.GetComponent<GraphQuestionEnemy2>();
        if (enemyScript != null)
        {
            enemyScript.ShootAt(target);
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
