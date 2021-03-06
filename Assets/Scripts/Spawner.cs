using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnEntities = null;
    [SerializeField] private GameObject spawnTargetOrigin = null;
    public List<GameObject> spawnList = new List<GameObject>();
    [SerializeField] private int spawnLimit = 50;
    [SerializeField, Range(0f, 10f)] private float spawnRate = 1f;
    [SerializeField] public UnityEvent onSpawn;
    private int numToSpawn = 1;
    [SerializeField] private float spawnDelay = 0f;
    private bool isDelayed = false;
    [SerializeField] public bool isDemonSpawner = false;
    public void IncreaseSpawnRate(float val) => Mathf.Max(1f, spawnRate - val);

    private void Awake()
    {
        spawnList = new List<GameObject>();
    }
    private void Start()
    {
        StartCoroutine(InitialiseSpawner());
    }

    public void IncreaseDifficulty()
    {
        numToSpawn += 2;
    }

    public void AttackPlayer(GameObject target)
    {
        foreach (GameObject entity in spawnList)
        {
            entity.GetComponent<AIController>().SetTargetObj(target);
        }
    }

    IEnumerator WaitForSpawnDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
    }


    IEnumerator InitialiseSpawner()
    {
        if(!isDelayed)
        {
            yield return WaitForSpawnDelay();
            isDelayed = true;
        }
        yield return new WaitForSeconds(Random.Range(1f, 5f) + spawnRate);
        Spawn();
        yield return InitialiseSpawner();
    }
    private void Spawn()
    {
        for(int i = 0; i < numToSpawn; i++)
        {
            if (isDemonSpawner && PlayerController.GetPlayer().demonsSpawned >= PlayerController.GetPlayer().demonCountToWin) return;
            if (spawnList.Count >= spawnLimit) return;
            GameObject newEntity = Instantiate(spawnEntities[Random.Range(0, spawnEntities.Length)], transform);
            newEntity.GetComponent<AIController>().SetWanderArea(spawnTargetOrigin.transform.position);
            spawnList.Add(newEntity);
            onSpawn.Invoke();
        }

    }

    public void StopSpawns()
    {
        StopAllCoroutines();
    }

    public void RemoveFromList(GameObject obj)
    {
        spawnList.Remove(obj);
        if(obj.tag == "Demon")
        {
            PlayerController.GetPlayer().DecrementDemons();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnEntities.Length < 1) return;
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(spawnTargetOrigin.transform.position, spawnEntities[0].GetComponent<AIController>().GetWanderRadius());
    }


}
