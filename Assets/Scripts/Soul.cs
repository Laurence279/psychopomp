using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Soul : MonoBehaviour
{

    private GameObject boundBeacon = null;
    private GameObject soulBank;

    private bool isSpecial = false;



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
        print(ValidatePath(beacon));
        if (!ValidatePath(beacon)) return;
        SetBeacon(beacon);
    }

    private bool ValidatePath(GameObject beacon)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        List<GameObject> searched = new List<GameObject>();
        queue.Enqueue(beacon);
        while(queue.Count > 0)
        {
            GameObject currentBeacon = queue.Dequeue();
            if(!searched.Contains(currentBeacon))
            {
                if (currentBeacon.GetComponentInChildren<SoulBank>()) return true;

                var neighbours = currentBeacon.GetComponent<Beacon>().GetNeighbours();
                foreach(var neighbour in neighbours)
                {
                    queue.Enqueue(neighbour);
                }
                searched.Add(currentBeacon);
            }
        }
        return false;
    }

}
