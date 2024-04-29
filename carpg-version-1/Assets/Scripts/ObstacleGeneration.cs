using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obstaclePrefabs;
    private Coroutine timer = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer ??= StartCoroutine(RandomTimer());
    }

    void GenerateObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 randomPosition = transform.position + Vector3.forward * Random.Range(-8, 8) + Vector3.up * 0.5f;
        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
        GameObject go = Instantiate(prefab, randomPosition, Quaternion.Euler(randomRotation), transform);
    }

    IEnumerator RandomTimer()
    {
        float timeOut = Random.Range(5.0f, 8.0f);
        yield return new WaitForSeconds(timeOut);
        GenerateObstacle();
        timer = null;
    }
}
