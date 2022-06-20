using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTower : MonoBehaviour
{
    private GameObject currentTarget = null;
    private int towerLayer = 0;
    [SerializeField] private float repairValue = 1f;
    public List<GameObject> targets = new List<GameObject>();
    private void Awake()
    {
        towerLayer = LayerMask.NameToLayer("Tower");
    }

    private void Start()
    {
        StartCoroutine(Repair());
    }

    IEnumerator Repair()
    {
        while (true)
        { 
            SetTarget();
            if(currentTarget)
            {
                currentTarget.GetComponent<Health>().Heal(repairValue);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void SetTarget()
    {
        foreach (var target in targets)
        {
            if(target == null)
            {
                continue;
            }
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth.GetHitPoints() >= targetHealth.GetMaxHitPoints()) continue;
            if (currentTarget == null)
            {
                currentTarget = target;
                continue;
            }
            currentTarget = GetClosestTarget(target, currentTarget);
        }
    }

    private GameObject GetClosestTarget(GameObject a, GameObject b)
    {
        return Vector2.Distance(transform.position, a.transform.position) < Vector2.Distance(transform.position, b.transform.position) ? a : b;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != towerLayer) return;
        if (targets.Contains(collision.gameObject)) return;
        targets.Add(collision.gameObject);
        collision.gameObject.GetComponent<Health>().OnDie += OnTowerDestroyed;
    }

    private void OnTowerDestroyed(GameObject obj)
    {
        targets.Remove(obj);
    }

    private void OnDrawGizmos()
    {
        if (!currentTarget) return;
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(currentTarget.gameObject.transform.position, 1f);
    }
}
