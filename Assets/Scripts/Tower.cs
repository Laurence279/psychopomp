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
            target = collision.gameObject;
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


}
