using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    SpriteRenderer sr;

    public float threshold = 0.5f;

    GameObject reference;

    Piece piece;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        reference = GameObject.Find("Spawn Area");
        //sprite = Resources.Load(@"Sprites\1x\bg12") as Sprite;
    }

    private void Start()
    {
        if (transform.parent == null)
        {
            sr.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            //ConvertToGrayscale(sr.sprite);
            sr.sortingOrder = -1;
            gameObject.AddComponent<Piece>();
        }
        else
        {
            if(TryGetComponent<Piece>(out piece))
            {
                piece.enabled = false;
            }
            sr.color = new Color(1f, 1f, 1f, 1f);
            ScaleObjectToFit(gameObject, reference, 0.5f);
            sr.sortingOrder = 0;
            EventManager.AddListener(EventNames.FitShape, EventLerpToOriginalScale);
        }

    }

    private void OnMouseUp()
    {
        ScaleObjectToFit(gameObject, reference, 0.5f);
    }

    private void OnMouseDown()
    {
        LerpToOriginalScale(0);
    }

    public void LerpToOriginalScale(int zero)
    {
        StartCoroutine(LerpScale(new Vector3(1f, 1f, 1f), 0.5f));
    }
    
    public void EventLerpToOriginalScale(int zero)
    {
        StartCoroutine(LerpScale(new Vector3(1f, 1f, 1f), 0.5f));
        GetComponent<BoxCollider2D>().size = Vector2.zero;
    }

    // Coroutine to lerp the scale
    private IEnumerator LerpScale(Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the scale is exactly set to the target scale after the lerp
        transform.localScale = targetScale;
    }

    public static void ScaleObjectToFit(GameObject target, GameObject reference, float sizeAdjustmentFactor = 1f)
    {
        if (target == null || reference == null) return;

        SpriteRenderer targetRenderer = target.GetComponent<SpriteRenderer>();
        SpriteRenderer referenceRenderer = reference.GetComponent<SpriteRenderer>();

        if (targetRenderer == null || referenceRenderer == null)
        {
            Debug.LogError("ScaleObjectToFit requires both target and reference to have SpriteRenderer components.");
            return;
        }

        float targetAspect = targetRenderer.sprite.bounds.size.x / targetRenderer.sprite.bounds.size.y;
        float referenceAspect = referenceRenderer.bounds.size.x / referenceRenderer.bounds.size.y;

        Vector3 scale = target.transform.localScale;

        if (targetAspect > referenceAspect)
        {
            // Target is wider than the reference
            scale *= (referenceRenderer.bounds.size.x / targetRenderer.sprite.bounds.size.x) * sizeAdjustmentFactor;
        }
        else
        {
            // Target is taller or equal in aspect to the reference
            scale *= (referenceRenderer.bounds.size.y / targetRenderer.sprite.bounds.size.y) * sizeAdjustmentFactor;
        }

        target.transform.localScale = new Vector3(scale.x, scale.x, scale.z); // Maintaining aspect ratio
    }

    void ConvertToGrayscale(Sprite sprite)
    {
        Texture2D originalTexture = sprite.texture;
        Color[] originalColors = originalTexture.GetPixels();
        Color[] grayscaleColors = new Color[originalColors.Length];

        for (int i = 0; i < originalColors.Length; i++)
        {
            float luminance = 0.299f * originalColors[i].r + 0.587f * originalColors[i].g + 0.114f * originalColors[i].b;
            grayscaleColors[i] = new Color(luminance, luminance, luminance, originalColors[i].a);
        }

        // Apply the grayscale colors to a new texture
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);
        newTexture.SetPixels(grayscaleColors);
        newTexture.Apply();

        // Create a new sprite from the texture
        Sprite newSprite = Sprite.Create(newTexture, sprite.rect, new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit);

        // Assign the new sprite to the SpriteRenderer
        sr.sprite = newSprite;
    }

}
