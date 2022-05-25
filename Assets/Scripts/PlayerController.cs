using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Animator animator;
    private Vector2 movement = new Vector2();
    [SerializeField] private float speed;
    private int soulCount = 0;
    private float timeSinceLastAttacked = Mathf.Infinity;
    [SerializeField] private float attackRate = 0.2f;

    // Attacking and weapon info
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject weaponOrigin;
    [SerializeField] float weaponDamage = 1f;

    public void IncrementSouls() => soulCount++;

    public int GetSouls => soulCount;

    private void SetSpeed(int newSpeed) => speed = newSpeed;

    public static PlayerController GetPlayer()
    {
        return FindObjectOfType<PlayerController>();
    }


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {



/*        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var grid = FindObjectOfType<Grid>();
            Vector3Int cellPos = grid.WorldToCell(transform.position);
            transform.position = grid.GetCellCenterLocal(cellPos);
        }*/
    }

    private void FixedUpdate() 
    {

        Move();
        if(movement.magnitude == 0)
        {
            animator.SetBool("isMoving", false);
        }
        timeSinceLastAttacked += Time.deltaTime;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        animator.SetBool("isMoving", true);
        movement = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
       if (EventSystem.current.IsPointerOverGameObject()) return;
       if(ctx.performed && timeSinceLastAttacked > attackRate)
        {
            timeSinceLastAttacked = 0;
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }

    }

    public void Move()
    {

        Vector2 currentPos = rb.position;
        Vector2 adjustedMovement = movement * speed;
        Vector2 newPos = currentPos + adjustedMovement * Time.deltaTime;

        float direction = movement.x;
        if(direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }


        rb.MovePosition(newPos);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Soul")
        {
            print("Hello");
            //collision.gameObject.GetComponent<AIController>().FollowPlayer();
            collision.gameObject.GetComponent<AIController>().SetTargetObj(FindObjectOfType<SoulBank>().gameObject);
        }
    }

    public void Hit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(weaponOrigin.transform.position, 0.5f, enemyLayer);
        foreach(var collider in colliders)
        {
            collider.GetComponent<Health>().Damage(weaponDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawSphere(weaponOrigin.transform.position, 0.5f);
    }




}
