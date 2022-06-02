using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
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
        timeSinceAttack = 0;
        Fire(target);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Demon"))
        {
            if(target == null)
            {
                target = collision.gameObject;
                return;
            }
            else
            {
                if(Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, collision.gameObject.transform.position))
                {
                    target = collision.gameObject;
                    return;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Demon"))
        {
            target = null;
        }
    }



    private void Fire(GameObject target)
    {
        if(target == null) return;
        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = transform.position;
        newProjectile.GetComponent<Projectile>().SetTarget(target.transform.position);
    }

    private void OnDrawGizmos()
    {
        if (!target) return;
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(target.gameObject.transform.position, 1f);
    }


}
