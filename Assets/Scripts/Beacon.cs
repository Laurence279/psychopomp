using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beacon : MonoBehaviour
{

    private GameObject soulBank;
    [SerializeField] private float radius = 5f;
    [SerializeField] List<GameObject> neighbours = new List<GameObject>();
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private LayerMask terrainLayer;

    private void Start()
    {
        StartCoroutine(BeaconEmitter());
    }

    private void Awake()
    {
        soulBank = FindObjectOfType<SoulBank>().gameObject;
    }

    IEnumerator BeaconEmitter()
    {
        Collider2D[] arr = Physics2D.OverlapCircleAll(transform.position, radius, detectionLayers);
        foreach (Collider2D c in arr)
        {
            if (c.gameObject.CompareTag("Soul") == true)
            {

                var soul = c.gameObject.GetComponent<Soul>();
                if (soul.GetBeacon() == null)
                {
                    soul.SetBeacon(gameObject);
                    continue;
                }

                var currentBeacon = soul.GetBeacon();
                float currentBeaconDistance = GetDistanceToBank(currentBeacon);
                float thisBeaconDistance = GetDistanceToBank(gameObject);

                if (thisBeaconDistance > currentBeaconDistance) continue;

                soul.SetBeacon(gameObject);

            }

        }

        yield return new WaitForSeconds(Random.Range(1f, 3f));
        yield return BeaconEmitter();
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < neighbours.Count; i++)
        {
            Debug.DrawLine(transform.position, neighbours[i].transform.position, Color.red);
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Beacon") == true && !neighbours.Contains(c.gameObject))
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, c.gameObject.transform.position, NavMesh.AllAreas, path);
            if(path.status == NavMeshPathStatus.PathComplete && path.corners.Length <= 5)
            {
                neighbours.Add(c.gameObject);
            }
        }
        else
        {
            neighbours.Remove(c.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Beacon") == true && neighbours.Contains(c.gameObject))
        {
           neighbours.Remove(c.gameObject);
        }
    }

    private float GetDistanceToBank(GameObject beacon)
    {
        return Vector2.Distance(beacon.transform.position, soulBank.transform.position);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(transform.position, radius);
    }




}
