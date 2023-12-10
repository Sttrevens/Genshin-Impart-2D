using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool isWalking = false;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    // This variable will keep track of the last horizontal direction the player was facing
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animation
        isWalking = movement.magnitude > 0;
        animator.SetBool("IsWalking", isWalking);

        if (movement.x < 0 && facingRight)
        {
            // If the player is moving left and facing right, flip the sprite and update facing direction
            facingRight = false;
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0 && !facingRight)
        {
            // If the player is moving right and facing left, flip the sprite and update facing direction
            facingRight = true;
            spriteRenderer.flipX = false;
        }

        // Since there are no up and down animations, we don't need to pass MoveY to the animator
        animator.SetFloat("MoveX", Mathf.Abs(movement.x)); // We pass the absolute value for left/right walking animation
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}