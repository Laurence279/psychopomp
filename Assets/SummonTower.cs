using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : MonoBehaviour
{

    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private GameObject spawnPrefab = null;
    private IEnumerator spawnCoroutine; 

    private void Start()
    { 
        spawnCoroutine = SpawnCoroutine();
        StartCoroutine(spawnCoroutine);
    }

    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);
            Spawn();
        }

    }

    private void Spawn()
    {
        GameObject newEntity = Instantiate(spawnPrefab, transform);
    }
}
