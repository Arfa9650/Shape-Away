using UnityEngine;
using System.Collections.Generic;

public class SpriteSlicer : MonoBehaviour
{
    public Sprite spriteToSlice;
    public int piecesX = 2; // Number of horizontal pieces
    public int piecesY = 2; // Number of vertical pieces

    List<GameObject> pieces = new List<GameObject>();

    private void Awake()
    {
        EventManager.Initialize();
    }

    void Start()
    {
        SliceSprite(spriteToSlice, piecesX, piecesY);
        SpawnPiece(0);
        EventManager.AddListener(EventNames.FitShape, SpawnPiece);
    }

    void SliceSprite(Sprite sprite, int piecesX, int piecesY)
    {
        Texture2D originalTexture = sprite.texture;
        int pieceWidth = originalTexture.width / piecesX;
        int pieceHeight = originalTexture.height / piecesY;

        for (int y = 0; y < piecesY; y++)
        {
            for (int x = 0; x < piecesX; x++)
            {
                Texture2D pieceTexture = new Texture2D(pieceWidth, pieceHeight);
                pieceTexture.SetPixels(originalTexture.GetPixels(x * pieceWidth, y * pieceHeight, pieceWidth, pieceHeight));
                pieceTexture.Apply();

                Sprite newSprite = Sprite.Create(pieceTexture, new Rect(0, 0, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f));

                GameObject newGameObject = new GameObject("Piece_" + x + "_" + y);
                SpriteRenderer renderer = newGameObject.AddComponent<SpriteRenderer>();
                renderer.sprite = newSprite;

                // Position the new GameObject appropriately
                newGameObject.transform.position = GetPiecePosition(x, y, pieceWidth, pieceHeight, sprite.pixelsPerUnit);

                // Add a BoxCollider2D to the piece
                BoxCollider2D boxCollider = newGameObject.AddComponent<BoxCollider2D>();
                boxCollider.size /= 2;
                boxCollider.isTrigger = true;

                pieces.Add(newGameObject);

                newGameObject.AddComponent<Puzzle>();

                // Create and position duplicate piece
                /*GameObject duplicatePiece = Instantiate(newGameObject, newGameObject.transform.parent);
                duplicatePiece.name = "Duplicate_Piece_" + x + "_" + y; // Rename to distinguish from the original
                                                                        // Adjust position for duplicate, moving it down based on the specified offset
                duplicatePiece.transform.position = new Vector3(xLinePiece, -4f, newGameObject.transform.position.z);

                // Add a MovableObject script to the duplicate piece
                MovableShape movableObject = duplicatePiece.AddComponent<MovableShape>();

                // Add a BoxCollider2D to the duplicate piece
                BoxCollider2D duplicateBoxCollider = duplicatePiece.AddComponent<BoxCollider2D>();
                duplicateBoxCollider.isTrigger = true;

                Rigidbody2D rb = duplicatePiece.AddComponent<Rigidbody2D>();
                rb.isKinematic = true;

                xLinePiece += 1.5f;*/
            }
        }
    }

    Vector3 GetPiecePosition(int x, int y, int pieceWidth, int pieceHeight, float pixelsPerUnit)
    {
        // Calculate the position to place the piece at, with (0,0) at the center of the original sprite
        float posX = (x + 0.5f) * pieceWidth / pixelsPerUnit - spriteToSlice.bounds.size.x / 2;
        float posY = (y + 0.5f) * pieceHeight / pixelsPerUnit - spriteToSlice.bounds.size.y / 2;
        return new Vector3(posX, posY, 0);
    }

    void SpawnPiece(int zero)
    {
        if (pieces.Count > 0)
        {
            // Get a random index within the range of available pieces
            int randomIndex = Random.Range(0, pieces.Count);

            // Instantiate the selected piece
            GameObject spawnedPiece = Instantiate(pieces[randomIndex], new Vector2(0, -3.9f), Quaternion.identity, transform);
            spawnedPiece.name = "Duplicate_" + spawnedPiece.name;
            spawnedPiece.AddComponent<MovableShape>();


            // Remove the spawned piece from the list
            pieces.RemoveAt(randomIndex);
        }
        else
        {
            Debug.LogWarning("No more pieces to spawn!");
        }

    }
}
