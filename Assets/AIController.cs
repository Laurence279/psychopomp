using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float speed = 1;
    private PlayerController player = null;
    public GameObject targetObj;
    public Vector3 target;
    [SerializeField] private float wanderRadius = 3;
    private bool isCollected = false;

    public Vector3 GetTarget() => target;
    public void SetTarget(Vector3 newTarget) => target = newTarget;
    public void SetTargetObj(GameObject newTarget) => targetObj = newTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(MovementCoroutine());
    }

    private void FixedUpdate()
    {
        if(targetObj)
        {
            target = targetObj.transform.position;
        }
        //if (Vector2.Distance(transform.position, target) < 1f) return;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    IEnumerator MovementCoroutine()
    {   
        // Find random point to move to
        target = Random.insideUnitCircle * wanderRadius;
        // Move to point
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        target = transform.position;
        // Wait x amount of seconds
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        // Repeat
        StartCoroutine(MovementCoroutine());
    }

    public void FollowPlayer()
    {
        StopAllCoroutines();
        player = PlayerController.GetPlayer();
        SetTargetObj(player.gameObject);
    }



}
