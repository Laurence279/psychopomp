using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private float lastDir = 1;
    private Animator animator;
    private Vector2 movement = new Vector2();
    [SerializeField] private float speed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() 
    {
        Move();
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
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



    


}
