using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private float speed = 1;
    private PlayerController player = null;
    public GameObject targetObj;
    public Vector3 target;
    [SerializeField] private float wanderRadius = 3f;
    private Vector2 wanderArea;
    private Rigidbody2D rb;

    [SerializeField] private float minMoveWaitTime = 0f;
    [SerializeField] private float maxMoveWaitTime = 1f;

    public Vector3 GetTarget() => target;

    public GameObject GetTargetObj() => targetObj;
    public void SetTarget(Vector3 newTarget) => target = newTarget;
    public void SetTargetObj(GameObject newTarget) => targetObj = newTarget;

    public float GetWanderRadius() => wanderRadius;


    // Attacking behaviour

    [SerializeField] private string attackAnimName = "";
    [SerializeField] private bool isAttacker = false;
    [SerializeField] private float weaponDamage = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackRate = 1f;
    private float timeSinceLastAttack = Mathf.Infinity;
    [SerializeField] private int scanEnemiesBufferSize = 100;
    [SerializeField] private float spottingDistance = 5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private bool isRanged = false;
    [SerializeField] private GameObject projectile = null;

    public void SetWanderArea(Vector2 origin)
    {
        wanderArea = origin;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        wanderArea = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(MovementCoroutine());
    }

    private void FixedUpdate()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (isAttacker && AttackingBehaviour()) return;
        MoveBehaviour();
    }

    public virtual void MoveBehaviour()
    {
        if (targetObj)
        {
            target = targetObj.transform.position;
        }
        if (Vector2.Distance(transform.position, target) < .25f)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        float direction = (target - transform.position).normalized.x;
        if (direction != 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = direction < 0 ? true : false;
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimName)) return;
        animator.SetBool("isMoving", true);
        rb.MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));

    }
    IEnumerator MovementCoroutine()
    {
        // Find random point to move to

        Vector2 randomPos = Random.insideUnitCircle * wanderRadius;

        // Moves in a random direction
        //target = new Vector2(transform.position.x + randomPos.x, transform.position.y + randomPos.y);

        //Moves anywhere within a circle boundary with the centre as a set point
        GetPositionWithinBounds(wanderArea, randomPos);

        // Move to point
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        target = transform.position;
        // Wait x amount of seconds
        yield return new WaitForSeconds(Random.Range(minMoveWaitTime, maxMoveWaitTime));
        // Repeat
        StartCoroutine(MovementCoroutine());
    }

    private void GetPositionWithinBounds(Vector2 origin, Vector2 variance)
    {
        target = new Vector2(origin.x + variance.x, origin.y + variance.y);
    }

    public void FollowPlayer()
    {
        StopAllCoroutines();
        player = PlayerController.GetPlayer();
        SetTargetObj(player.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (targetObj && Vector2.Distance(transform.position, collision.transform.position) > Vector2.Distance(transform.position, targetObj.transform.position)) return; 
            SetTargetObj(collision.gameObject);
        }

    }

    private bool AttackingBehaviour()
    {
        if (!targetObj) return false;
        if (!CheckIsInRange(targetObj)) return false;
        Attack(targetObj);
        return true;
    }

    private bool CheckIsInRange(GameObject target)
    {
        return Vector2.Distance(transform.position, target.transform.position) < attackRange;
    }

    private void Attack(GameObject target)
    {
        transform.position = transform.position;
        if (timeSinceLastAttack < attackRate) return;
        animator.SetTrigger("attack");
        timeSinceLastAttack = 0;
        float direction = (target.transform.position - transform.position).normalized.x;
        if (direction != 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = direction < 0 ? true : false;
        }

    }

    public void Hit()
    {
        if(isRanged)
        {
            if (targetObj == null) return;
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position;
            newProjectile.GetComponent<Projectile>().SetTarget(targetObj.transform.position);
        }
        else
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);
            if (collider != null && collider.GetComponent<Health>())
            {
                collider.GetComponent<Health>().Damage(weaponDamage);
            }
        }
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
    private void OnDestroy()
    {
        if (GetComponentInParent<Spawner>() == null) return;
        GetComponentInParent<Spawner>().RemoveFromList(gameObject);
    }


}
