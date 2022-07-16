using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{

    [SerializeField] GameObject soulSpawnerPrefab;
    [SerializeField] GameObject demonSpawnerPrefab;
    [SerializeField] int soulSpawners = 10;
    [SerializeField] int demonSpawners = 5;
    [SerializeField] GameObject[] spawners = null;
    private void Start()
    {
        for (int i = 0; i < spawners.Length - 1; i++)
        {
            var temp = spawners[i];
            int rnd = Random.Range(i, spawners.Length);
            spawners[i] = spawners[rnd];
            spawners[rnd] = temp;
        }

        for (int i = 0; i < spawners.Length; i++)
        {
            if(soulSpawners > 0)
            {
                GameObject newSpawner = Instantiate(soulSpawnerPrefab, transform);
                newSpawner.transform.position = spawners[i].transform.position;
                Destroy(spawners[i].gameObject);
                soulSpawners--;
                continue;
            }
            if (demonSpawners > 0)
            {
                GameObject newSpawner = Instantiate(demonSpawnerPrefab, transform);
                newSpawner.transform.position = spawners[i].transform.position;
                Destroy(spawners[i].gameObject, 1f);
                demonSpawners--;
                continue;
            }
            Destroy(spawners[i].gameObject);

        }

        spawners = null;

    }

}
