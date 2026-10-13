using UnityEngine;

public class BookwormAI : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public AudioClip eatSound;
    private Transform targetBook;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        FindNearestBook();

        if (targetBook != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetBook.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetBook.position) < 0.3f)
            {
                EatBook();
            }
        }
    }

    private void FindNearestBook()
    {
        var books = GameManager.Instance.ActiveBooks;
        float minDistance = float.MaxValue;
        Transform closest = null;

        for (int i = books.Count - 1; i >= 0; i--)
        {
            if (books[i] == null)
            {
                books.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(transform.position, books[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = books[i];
            }
        }

        targetBook = closest;
    }

    private void EatBook()
    {
        if (targetBook != null)
        {
            GameManager.Instance.UnregisterActiveBook(targetBook);
            Destroy(targetBook.gameObject);
        }

        if (eatSound) AudioSource.PlayClipAtPoint(eatSound, transform.position);

        GameManager.Instance.BookDestroyedByWorm();
        Destroy(gameObject);
    }
}
