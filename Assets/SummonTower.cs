using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : MonoBehaviour
{

    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private GameObject spawnPrefab = null;
    [SerializeField] private int spawnLimit = 10;
    private List<GameObject> spawnList = new List<GameObject>();
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
            if(spawnList.Count < spawnLimit)
            {
                Spawn();
            }
        }

    }

    private void Spawn()
    {
        GameObject newEntity = Instantiate(spawnPrefab, transform);
        spawnList.Add(newEntity);
        newEntity.GetComponent<Health>().OnDie += OnSpawnedUnitKilled;
    }

    private void OnSpawnedUnitKilled(GameObject obj)
    {
        spawnList.Remove(obj);
    }
}
