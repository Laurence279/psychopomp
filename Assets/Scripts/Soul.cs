using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Soul : MonoBehaviour
{

    private GameObject boundBeacon = null;
    private GameObject soulBank;

    private bool isSpecial = false;

    [SerializeField] List<GameObject> waypoints = new List<GameObject>();   
    [SerializeField] int currentWaypointIndex = 0;

    private void Awake()
    {
        soulBank = FindObjectOfType<Door>().gameObject;
    }
    private void Start()
    {
        //isSpecial = Random.Range(1, 101) > 70;
    }

    private void FixedUpdate()
    {
        if (!isSpecial) return;
        GetComponent<AIController>().SetTargetObj(soulBank);
    }


    public void SetBeacon(GameObject beacon)
    {
        boundBeacon = beacon;
        GetComponent<AIController>().SetWanderArea(beacon.transform.position);
    }

    public GameObject GetBeacon() => boundBeacon;

    public void BankSoul()
    {
        Destroy(gameObject);
        PlayerController.GetPlayer().IncrementSouls();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Get the beacon
        if (!collision.gameObject.CompareTag("Beacon")) return;
        GameObject beacon = collision.gameObject;
        waypoints = SearchPath(beacon);
        GetComponent<AIController>().SetTargetObj(beacon);
    }

    public GameObject GetNextTarget()
    {
        return waypoints[1];
    }

    private List<GameObject> SearchPath(GameObject beacon)
    {
        Queue<KeyValuePair<GameObject, GameObject>> queue = new Queue<KeyValuePair<GameObject, GameObject>>();
        queue.Enqueue(new KeyValuePair<GameObject, GameObject>(beacon, null));
        Dictionary<GameObject, GameObject> searched = new Dictionary<GameObject, GameObject>();
        while(queue.Count > 0)
        {
            KeyValuePair<GameObject, GameObject> currentBeacon = queue.Dequeue();
            if(!searched.ContainsKey(currentBeacon.Key))
            {
                searched[currentBeacon.Key] = currentBeacon.Value;
                if (currentBeacon.Key.GetComponentInChildren<SoulBank>())
                {
                    List<GameObject> result = new List<GameObject>();
                    var current = currentBeacon.Key;
                    while (current != null)
                    {
                        result.Add(current);
                        current = searched[current];
                    }
                    result.Reverse();
                    return result;
                }
                else
                {
                    var neighbours = currentBeacon.Key.GetComponent<Beacon>().GetNeighbours();
                    foreach(var neighbour in neighbours)
                    {
                        queue.Enqueue(new KeyValuePair<GameObject, GameObject>(neighbour, currentBeacon.Key));
                    }
                }
            }
        }
        return null;
    }

}
