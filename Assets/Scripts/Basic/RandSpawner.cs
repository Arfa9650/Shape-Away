using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandSpawner : IntEventInvoker
{
    #region Fields

    [SerializeField]
    GameObject hand;

    public static int currentShape;

    Dictionary<ShapeName, GameObject> shapes = new Dictionary<ShapeName, GameObject>();
    Dictionary<ShapeName, GameObject> triggers = new Dictionary<ShapeName, GameObject>();

    List<int> shapesToSpawn = new List<int>();

    float[] rotations = new float[]{ 0f, 90f, 180f, -90f };

    int shapeNum = 0;

    List<Vector3> gridPositions = new List<Vector3>();

    int gridSize = 3; // Assuming a 3x3 grid
    
    float cellSize = 1.5f; // Distance between cells

    //Difficulty Values

    int difficulty;

    bool canRotate = false;

    bool enableColor = false;

    #endregion

    #region Methods

    private void Awake()
    {
        EventManager.Initialize();
    }

    private void Start()
    {

        Application.targetFrameRate = 61;
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);

        DifficultyAdjuster(difficulty);

        shapes.Add(ShapeName.Circle, Resources.Load(@"Prefabs\Shapes\Circle") as GameObject);
        shapes.Add(ShapeName.Square, Resources.Load(@"Prefabs\Shapes\Square") as GameObject);
        shapes.Add(ShapeName.Triangle, Resources.Load(@"Prefabs\Shapes\Triangle") as GameObject);
        shapes.Add(ShapeName.Crystal, Resources.Load(@"Prefabs\Shapes\Crystal") as GameObject);
        shapes.Add(ShapeName.Semicircle, Resources.Load(@"Prefabs\Shapes\Semi-Circle") as GameObject);
        shapes.Add(ShapeName.Hexagon, Resources.Load(@"Prefabs\Shapes\Hexagon") as GameObject);
        shapes.Add(ShapeName.Quadrant, Resources.Load(@"Prefabs\Shapes\Quadrant") as GameObject);
        shapes.Add(ShapeName.Diamond, Resources.Load(@"Prefabs\Shapes\Diamond") as GameObject);
        shapes.Add(ShapeName.CrystalTwo, Resources.Load(@"Prefabs\Shapes\Crystal Two") as GameObject);
        shapes.Add(ShapeName.Trapzoid, Resources.Load(@"Prefabs\Shapes\Trapzoid") as GameObject);
        shapes.Add(ShapeName.TrapzoidTwo, Resources.Load(@"Prefabs\Shapes\Trapzoid Two") as GameObject);

        triggers.Add(ShapeName.Circle, Resources.Load(@"Prefabs\Shapes\Circle Trigger") as GameObject);
        triggers.Add(ShapeName.Square, Resources.Load(@"Prefabs\Shapes\Square Trigger") as GameObject);
        triggers.Add(ShapeName.Triangle, Resources.Load(@"Prefabs\Shapes\Triangle Trigger") as GameObject);
        triggers.Add(ShapeName.Crystal, Resources.Load(@"Prefabs\Shapes\Crystal Trigger") as GameObject);
        triggers.Add(ShapeName.Semicircle, Resources.Load(@"Prefabs\Shapes\Semicircle Trigger") as GameObject);
        triggers.Add(ShapeName.Hexagon, Resources.Load(@"Prefabs\Shapes\Hexagon Trigger") as GameObject);
        triggers.Add(ShapeName.Quadrant, Resources.Load(@"Prefabs\Shapes\Quadrant Trigger") as GameObject);
        triggers.Add(ShapeName.Diamond, Resources.Load(@"Prefabs\Shapes\Diamond Trigger") as GameObject);
        triggers.Add(ShapeName.CrystalTwo, Resources.Load(@"Prefabs\Shapes\Crystal Two Trigger") as GameObject);
        triggers.Add(ShapeName.Trapzoid, Resources.Load(@"Prefabs\Shapes\Trapzoid Trigger") as GameObject);
        triggers.Add(ShapeName.TrapzoidTwo, Resources.Load(@"Prefabs\Shapes\Trapzoid Two Trigger") as GameObject);

        SpawnShapes(difficulty <= 9 ? difficulty : 9);
        SpawnShape(shapeNum);
        EventManager.AddListener(EventNames.FitShape, SpawnShape);
    }

    void SpawnShape(int num)
    {
        if (shapeNum < shapesToSpawn.Count)
        {
            if (difficulty < 10)
            {
                hand.SetActive(true);
            }
            currentShape = shapesToSpawn[shapeNum];
            Instantiate(shapes[(ShapeName)currentShape], new Vector2(0, -3.9f), Quaternion.identity, transform);
        }
        else
            hand.SetActive(false);
        shapeNum++;
    }

    void InitializeGridPositions()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Calculate cell positions centered around (0,0)
                Vector3 cellPosition = new Vector3((x - gridSize / 2) * cellSize, (y - gridSize / 2) * cellSize + 0f, 0);
                gridPositions.Add(cellPosition);
            }
        }
    }

    void SpawnShapes(int numToSpawn)
    {
        // Make sure we don't try to spawn more than the grid can hold
        numToSpawn = Mathf.Min(numToSpawn, gridSize * gridSize);

        InitializeGridPositions(); // Prepare grid positions

        // Spawn the first trigger in the middle
        Vector3 middlePosition = gridPositions[gridSize * gridSize / 2]; // Middle cell for a 3x3 grid

        //Vector3 middlePosition = new Vector3(0, -0.34f, 0);

        InstantiateTriggerAtPosition(middlePosition);
        gridPositions.Remove(middlePosition); // Remove middle position from available spots

        // Spawn the rest of the triggers randomly
        for (int i = 1; i < numToSpawn; i++) // Start from 1 since we already spawned one
        {
            if (gridPositions.Count > 0)
            {
                int randomIndex = Random.Range(0, gridPositions.Count);
                Vector3 randomPosition = gridPositions[randomIndex];
                InstantiateTriggerAtPosition(randomPosition);
                gridPositions.RemoveAt(randomIndex); // Remove the selected position
            }
        }
    }

    void InstantiateTriggerAtPosition(Vector3 position)
    {
        int rand = Random.Range(0, triggers.Count); 

        int rotateIn = 1;
        switch((ShapeName)rand)
        {
            case ShapeName.Triangle:
                rotateIn = 4;
                break;
            case ShapeName.Semicircle:
                rotateIn = 4;
                break;
            case ShapeName.Crystal:
                rotateIn = 2;
                break;
            case ShapeName.Hexagon:
                rotateIn = 2;
                break;
            case ShapeName.Quadrant:
                rotateIn = 4;
                break;
            case ShapeName.Diamond:
                rotateIn = 4;
                break;
            case ShapeName.CrystalTwo:
                rotateIn = 2;
                break;
            case ShapeName.Trapzoid:
                rotateIn = 4;
                break;
            case ShapeName.TrapzoidTwo:
                rotateIn = 4;
                break;
        }

        Instantiate(triggers[(ShapeName)rand], position, canRotate ? Quaternion.Euler(0, 0, rotations[Random.Range(0, rotateIn)]) : Quaternion.identity, transform);
        shapesToSpawn.Add(rand);
    }

    private void DifficultyAdjuster(int difficulty)
    {
        switch (difficulty)
        {
            case <= 1:
                //nothing for now
                break;

            case >3:
                canRotate = true;
                break;
        }
    }

    #endregion


}
