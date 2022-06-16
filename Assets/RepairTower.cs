using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTower : MonoBehaviour
{
    [SerializeField] private GameObject projectile = null;
    private GameObject target = null;
    private float timeSinceAttack = Mathf.Infinity;
    [SerializeField] private float attackRate = 2f;

    private void FixedUpdate()
    {
        timeSinceAttack += Time.deltaTime;
        if (target == null) return;
        if (timeSinceAttack < attackRate) return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!target) return;
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(target.gameObject.transform.position, 1f);
    }
}
