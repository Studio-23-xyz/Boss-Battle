using DG.Tweening;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
   [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private void Start()
    {
        
        originalColor = spriteRenderer.color;
    }

    public void Dead()
    {
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            spriteRenderer.DOColor(originalColor, 0.1f);
        });
    }
    
    public void PowerUP()
    {
        spriteRenderer.DOColor(Color.green, 0.1f).OnComplete(() =>
        {
            spriteRenderer.DOColor(originalColor, 0.1f);
        });
    }
}
