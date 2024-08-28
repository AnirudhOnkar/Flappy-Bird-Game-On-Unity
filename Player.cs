using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    private void Awake()
    {
        // Initialize spriteRenderer and start sprite animation
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Repeatedly call AnimateSprite to update sprite frames
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        // Reset position and direction
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero; // Fixed typo
    }

    private void Update()
    {
        // Check for space key or mouse button press
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
            }
        }

        // Apply gravity over time
        direction.y += gravity * Time.deltaTime;

        // Update the player's position
        transform.position += direction * Time.deltaTime;
    }

    private void AnimateSprite()
    {
        // Increment the sprite index to switch to the next sprite
        spriteIndex++;

        // Loop back to the first sprite if we exceed the array length
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        // Update the spriteRenderer with the new sprite
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for collisions with obstacles or scoring objects
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.IncreaseScore();
            }
        }
    }
}
