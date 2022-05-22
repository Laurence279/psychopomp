using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private float lastDir = 1;
    private Animator animator;
    private Vector2 movement = new Vector2();
    [SerializeField] private float speed;
    public int soulCount = 0;

    public static GameObject GetPlayer()
    {
        return FindObjectOfType<PlayerController>().gameObject;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var grid = FindObjectOfType<Grid>();
            Vector3Int cellPos = grid.WorldToCell(transform.position);
            transform.position = grid.GetCellCenterLocal(cellPos);
        }
    }

    private void FixedUpdate() 
    {

        Move();
        if(movement.magnitude == 0)
        {
            animator.SetBool("isMoving", false);
        }

    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        animator.SetBool("isMoving", true);
        movement = ctx.ReadValue<Vector2>();
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Soul")
        {
            collision.gameObject.GetComponent<AIController>().FollowPlayer();
        }
    }



}
