using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : AIController
{
    [SerializeField] private int scanEnemiesBufferSize = 100;
    [SerializeField] private float spottingDistance = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private void FixedUpdate()
    {
        if (AttackingBehaviour()) return;
        MoveBehaviour();
    }

    public override void MoveBehaviour()
    {
        target = PlayerController.GetPlayer().gameObject.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target, 5f * Time.deltaTime);
    }

    private bool AttackingBehaviour()
    {
        var target = FindEnemy();
        if (!target) return false;
        SetTargetObj(target);
        return true;
    }

    private GameObject FindEnemy()
    {
        GameObject enemy = null;

        int maxColliders = scanEnemiesBufferSize;
        Collider2D[] hitColliders = new Collider2D[maxColliders];
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, spottingDistance, hitColliders, enemyLayer);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;
            if (!enemy)
            {
                enemy = collider.gameObject;
            }

            if (Vector3.Distance(transform.position, collider.gameObject.transform.position) < Vector3.Distance(transform.position, enemy.transform.position))
            {
                enemy = collider.gameObject;
            }
        }

        return enemy;
    }



}
