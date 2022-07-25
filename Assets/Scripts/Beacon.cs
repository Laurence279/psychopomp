using System;
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

    public List<GameObject> GetNeighbours() => neighbours;

    private void Start()
    {
        FindNeighbours();
    }

    private void FindNeighbours()
    {
        Collider2D[] arr = Physics2D.OverlapCircleAll(transform.position, radius, detectionLayers);
        foreach (Collider2D c in arr)
        {
            if (c.gameObject.GetComponent<Beacon>() || c.gameObject.GetComponent<SoulBank>())
            {
                if (c.gameObject == gameObject || neighbours.Contains(c.gameObject)) continue;
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, c.gameObject.transform.position, NavMesh.AllAreas, path);
                if (path.status == NavMeshPathStatus.PathComplete && path.corners.Length <= 5)
                {
                    neighbours.Add(c.gameObject);
                    c.gameObject.GetComponentInParent<Health>().OnDie += HandleNeighbourDestroyed;
                }
            }
        }
    }

    private void HandleNeighbourDestroyed(GameObject obj)
    {
        neighbours.Remove(obj.GetComponentInChildren<Beacon>().gameObject);
    }

    private void Awake()
    {
        soulBank = FindObjectOfType<SoulBank>().gameObject;
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < neighbours.Count; i++)
        {
            if (!neighbours[i]) continue;
            Debug.DrawLine(transform.position, neighbours[i].transform.position, Color.red);
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<Beacon>() && !neighbours.Contains(c.gameObject) || c.gameObject.GetComponent<SoulBank>() && !neighbours.Contains(c.gameObject))
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, c.gameObject.transform.position, NavMesh.AllAreas, path);
            if(path.status == NavMeshPathStatus.PathComplete && path.corners.Length <= 5)
            {
                neighbours.Add(c.gameObject);
            }
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

    private void OnDestroy()
    {

    }



}
