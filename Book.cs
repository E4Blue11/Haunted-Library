using UnityEngine;

public enum BookColor {Red, Blue, Green}

public class Book : MonoBehaviour
{

    public BookColor colorType;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColorVisual();
    }

    public void SetColor(BookColor color)
    {
        colorType = color;
        UpdateColorVisual();
    }

    private void UpdateColorVisual()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        switch (colorType)
        {
            case BookColor.Red:
                spriteRenderer.color = Color.red;
                break;

            case BookColor.Blue:
                spriteRenderer.color = Color.blue;
                break;

            case BookColor.Green:
                spriteRenderer.color = Color.green;
                break;
        }
    }
    
}
