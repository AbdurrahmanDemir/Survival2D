using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Update()
    {
        spriteRenderer.sortingOrder = -((int)transform.position.y * 10);
    }
}
