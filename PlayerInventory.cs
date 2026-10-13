using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform carryPosition;
    private Book heldBook = null;

    public bool HasBook() => heldBook != null;
    public BookColor HeldBookColor => heldBook.colorType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!HasBook() && other.CompareTag("Book"))
        {
            Book book = other.GetComponent<Book>();
            if (book != null)
            {
                PickUpBook(book);
            }
        }
    }

    private void PickUpBook(Book book)
    {
        heldBook = book;

        Collider2D col = heldBook.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            heldBook.transform.SetParent(carryPosition != null ? carryPosition : transform);
            heldBook.transform.localPosition = Vector3.up * 0.5f;
        }
    }

    public void DepositBook()
    {
        if (heldBook != null)
        {
            Destroy(heldBook.gameObject);
            heldBook = null;
        }
    }
    
}
