using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnEntity = null;
    [SerializeField] private GameObject spawnTargetOrigin = null;
    [SerializeField] private float spawnTargetRadius = 3f;
    public List<GameObject> spawnList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(InitialiseSpawner());
    }

    IEnumerator InitialiseSpawner()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        Spawn();
        yield return InitialiseSpawner();
    }
    private void Spawn()
    {
        GameObject newEntity = Instantiate(spawnEntity, transform);
        newEntity.GetComponent<AIController>().SetSpawnArea(spawnTargetOrigin, spawnTargetRadius);
        spawnList.Add(newEntity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(spawnTargetOrigin.transform.position, spawnTargetRadius);
    }

}
