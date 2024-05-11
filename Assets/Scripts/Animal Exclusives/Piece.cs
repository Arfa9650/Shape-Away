using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    SpriteRenderer sr;
    public Color outlineColor = Color.black;
    public float originalScale = 0.98f;  // Scale of the original sprite
    public float outlineScale = 1.03f;
    private GameObject outlineObject;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CreateOutline();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(sr != null)
            sr.color = new Color(0.52f, 0.52f, 0.52f, 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sr != null)
            sr.color = new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    void CreateOutline()
    {
        if (outlineObject != null) return;  // Prevent multiple outlines

        // Create a new GameObject for the outline
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform, false);

        SpriteRenderer originalSpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer outlineSpriteRenderer = outlineObject.AddComponent<SpriteRenderer>();

        // Set the outline properties
        outlineSpriteRenderer.sprite = originalSpriteRenderer.sprite;
        outlineSpriteRenderer.material = originalSpriteRenderer.material;
        outlineSpriteRenderer.color = outlineColor;
        outlineSpriteRenderer.sortingLayerID = originalSpriteRenderer.sortingLayerID;
        outlineSpriteRenderer.sortingOrder = originalSpriteRenderer.sortingOrder - 1;  // Render behind the original sprite

        // Scale and position the outline object
        outlineObject.transform.localScale = new Vector3(outlineScale, outlineScale, outlineScale);

        // Scale down the original sprite
        transform.localScale = new Vector3(originalScale, originalScale, originalScale);
    }

    void OnDestroy()
    {
        // Clean up to avoid memory leak
        if (outlineObject != null)
            Destroy(outlineObject);
    }
}
