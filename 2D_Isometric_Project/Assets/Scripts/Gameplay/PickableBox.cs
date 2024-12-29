using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color highlightedColor = new Color(184, 255, 109, 255);
    private Color unhighlightedColor = Color.white;

    public void HighlightBox(bool isHighlighted)
    {
        spriteRenderer.color = isHighlighted ? Color.yellow : Color.white;
    }
}
