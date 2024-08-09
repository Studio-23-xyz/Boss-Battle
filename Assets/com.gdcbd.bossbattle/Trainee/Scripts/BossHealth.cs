using DG.Tweening;

using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float colorChangeDuration = 0.05f;
    public float revertColorDelay = 0.5f;

    private Color originalColor;

    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    public void ColorChange()
    {
        
            GameObject parentObject = GameObject.Find("Boss");
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                Transform childTransform = parentObject.transform.GetChild(i);
                //GameObject childObject = childTransform.gameObject;
                var childspriteRenderer = childTransform.GetComponent<SpriteRenderer>();
                Debug.Log("Child" + childspriteRenderer);
                //Debug.Log($"Child {childspriteRenderer}");
                childspriteRenderer.DOColor(hitColor, colorChangeDuration)
                    .OnComplete(() => spriteRenderer.DOColor(originalColor, colorChangeDuration).SetDelay(revertColorDelay));
        }

    }
      
    
}
