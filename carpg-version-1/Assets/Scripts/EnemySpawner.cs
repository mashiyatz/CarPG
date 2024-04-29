using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefabs;
    private bool isSpawning;
    public Transform player;
    
    void Start()
    {
            
    }

    void Update()
    {
        if (transform.childCount == 0 && !isSpawning)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        isSpawning = true;
        int numSpawn = Random.Range(1, 4);
        for (int i = 0; i < numSpawn; i++)
        {
            GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, 90, 0), transform);
            go.GetComponent<Collider>().enabled = false;
            go.GetComponent<EnemyControl>().currentSpeed = new Vector3(10f, 0, 0f);
            yield return null;
        }
        isSpawning = false;
    }
}
