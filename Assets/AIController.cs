using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float speed = 1;
    private GameObject player = null;
    public Vector3 target;
    [SerializeField] private float wanderRadius = 3;
    private bool isCollected = false;

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
        if (isCollected)
        {
            target = player.transform.position;
            if (Vector2.Distance(transform.position, target) < 1f) return;
        }
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    IEnumerator MovementCoroutine()
    {
        // Find random point to move to
        target = Random.insideUnitCircle * wanderRadius;
        // Move to point
        yield return WaitForMovement();
        // Wait x amount of seconds
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        // Repeat
        yield return MovementCoroutine();
    }

    IEnumerator WaitForMovement()
    {
        while(Vector2.Distance(transform.position, target) > 0.001f)
        {
            yield return new WaitForFixedUpdate();
            yield return WaitForMovement();
        }
    }

    public void FollowPlayer()
    {
        StopAllCoroutines();
        player = PlayerController.GetPlayer();
        isCollected = true;
    }





}
