using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnEntity = null;
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
    }

}
