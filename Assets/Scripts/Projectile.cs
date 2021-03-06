using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 2f;


    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void FixedUpdate()
    {
       transform.rotation = Quaternion.LookRotation(Vector3.forward, target - transform.position);
       transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void SetTarget(Vector2 newTarget) => target = newTarget;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Demon"))
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
            Destroy(gameObject);
        }
        
    }

}
