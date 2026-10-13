using UnityEditor.Build.Content;
using UnityEngine;

public class Bookshelf : MonoBehaviour
{
    public BookColor requiredColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.HasBook())
            {
                if (inventory.HeldBookColor == requiredColor)
                {
                    inventory.DepositBook();
                    GameManager.Instance.BookReturned();
                }
                else
                {
                    Debug.Log("Wrong bookshelf for this color!");
                }
            }
        }
    }
    
}
