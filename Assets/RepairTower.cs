using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTower : MonoBehaviour
{
    private GameObject target = null;
    private int towerLayer = 0;
    [SerializeField] private float repairValue = 1f;
    private void Awake()
    {
        towerLayer = LayerMask.NameToLayer("Tower");
    }

    private void Start()
    {
        StartCoroutine(BeaconEmitter());
    }

    private void Repair()
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth.GetHitPoints() >= targetHealth.GetMaxHitPoints() || targetHealth.isDead())
        {
            target = null;
            return;
        }
        targetHealth.Heal(repairValue);

    }

    IEnumerator BeaconEmitter()
    {
        Collider2D[] arr = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius, 1 << LayerMask.NameToLayer("Tower"));
        foreach (Collider2D c in arr)
        {
            if (c.gameObject.layer != towerLayer || c.gameObject == gameObject) continue;
            if (target == null)
            {
                target = c.gameObject;
                continue;
            }
            else
            {
                if (Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, c.gameObject.transform.position))
                {
                    target = c.gameObject;
                    continue;
                }
            }

        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
        if (target) Repair();
        yield return BeaconEmitter();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != towerLayer) return;
        if (target == null)
        {
            target = collision.gameObject;
            return;
        }
        else
        {
            if (Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, collision.gameObject.transform.position))
            {
                target = collision.gameObject;
                return;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (!target) return;
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(target.gameObject.transform.position, 1f);
    }
}
