using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerInventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        moveInput = Vector2.zero;

      
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y -= 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1;
        }

        moveInput = moveInput.normalized;

        
        if (moveInput.x > 0.01f)
        {
            spriteRenderer.flipX = false; // Facing Right
        }
        else if (moveInput.x < -0.01f)
        {
            spriteRenderer.flipX = true;  // Facing Left
        }

        
        if (animator != null)
        {
            bool isMoving = moveInput.magnitude > 0.01f;
            animator.SetBool("isMoving", isMoving);

            if (inventory != null)
            {
                animator.SetBool("isCarrying", inventory.HasBook());
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}