using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject bookPrefab;
    public AudioClip spawnSound;
    public AudioClip bookDropSound;

    private Transform targetShelf;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool hasDroppedBook = false;
    private Vector3 lastPosition;

    public void Initialize(Transform shelf)
    {
        targetShelf = shelf;
        audioSource = GetComponent<AudioSource>();
        if (spawnSound && audioSource) audioSource.PlayOneShot(spawnSound);
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (targetShelf == null) return;

        // Move towards shelf
        transform.position = Vector2.MoveTowards(transform.position, targetShelf.position, moveSpeed * Time.deltaTime);

        // Track movement direction & flip sprite
        float movementDirection = transform.position.x - lastPosition.x;
        if (movementDirection > 0.01f)
        {
            spriteRenderer.flipX = false; // Facing Right
        }
        else if (movementDirection < -0.01f)
        {
            spriteRenderer.flipX = true;  // Facing Left
        }

        // Handle Moving vs Idle Animation
        bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0.001f;
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }

        lastPosition = transform.position;

        // Check if reached bookshelf to drop/grab book
        if (!hasDroppedBook && Vector2.Distance(transform.position, targetShelf.position) < 0.2f)
        {
            DropBook();
        }
    }

    private void DropBook()
{
    hasDroppedBook = true;

    if (animator != null)
    {
        animator.SetTrigger("GrabBook");
    }

    if (bookPrefab != null)
    {
        
        Vector3 dropPosition = transform.position + (Vector3.down * 1.5f);

        GameObject droppedBook = Instantiate(bookPrefab, dropPosition, Quaternion.identity);
        Book bookScript = droppedBook.GetComponent<Book>();
        
        BookColor randomColor = (BookColor)Random.Range(0, System.Enum.GetValues(typeof(BookColor)).Length);
        bookScript.SetColor(randomColor);

        GameManager.Instance.RegisterActiveBook(droppedBook.transform);
    }

    if (bookDropSound && audioSource)
    {
        AudioSource.PlayClipAtPoint(bookDropSound, transform.position);
    }

    Destroy(gameObject, 0.8f);
}
}
