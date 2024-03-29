using UnityEngine;

public class MovableShape : IntEventInvoker
{
    #region Fields

    public ShapeName shape;

    public ShapeColors color = ShapeColors.Null;

    Rigidbody2D rb;

    Vector2 ogPos;

    bool isPressed = false;

    bool canMerge = false;

    bool oneTimeVibration = true;

    float speed = 0.1f;

    float circularAreaRadius = 0.5f;

    //Difficulty Values

    int difficulty;

    bool canRotate = false;
    
    bool enableColor = false;

    bool failOneTime = true;

    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        unityEvents.Add(EventNames.FitShape, new FitShape());
        EventManager.AddInvoker(EventNames.FitShape, this);

        unityEvents.Add(EventNames.FailToFit, new FailToFit());
        EventManager.AddInvoker(EventNames.FailToFit, this);

        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        DifficultyAdjuster(difficulty);
        rb = GetComponent<Rigidbody2D>();
        ogPos = transform.position;

        if (enableColor)
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    //Red
                    color = ShapeColors.Red;
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 1:
                    //Green
                    color = ShapeColors.Green;
                    GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 2:
                    //Yellow
                    color = ShapeColors.Yellow;
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if(rb == null)
        {
            Debug.LogWarning("No Gravity");
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }

        if (isPressed && oneTimeVibration)
        {
            DragObject();
        }
        else
        {
            Vector2 smoothedPosition = Vector2.Lerp(transform.position, ogPos, speed);
            rb.MovePosition(smoothedPosition);
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        if(canRotate && Vector3.Distance(transform.position, ogPos) <= circularAreaRadius)
        {
            RotateObject();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isPressed)
            canMerge = true;
        //Changes = collision name
        if (!isPressed && (collision.CompareTag(gameObject.tag + "Trigger") || "Duplicate_" + collision.name + "(Clone)" == gameObject.name) && canMerge && collision.transform.rotation == transform.rotation)
        {
            if (oneTimeVibration)
            {
                unityEvents[EventNames.FitShape].Invoke(0);
                ogPos = collision.GetComponent<Transform>().position;
                AudioManager.Play(AudioClipNames.Success);

                oneTimeVibration = false;
                //collision.GetComponent<TriggersBehaviour>().Occupied = false;
                Destroy(collision.gameObject);
                gameObject.tag = "Untagged";

            }
        }

        else if (!isPressed && canMerge && (collision.tag.Contains("Trigger") || !collision.name.Contains("Duplicate")) && failOneTime)
        {
            unityEvents[EventNames.FailToFit].Invoke(0);
            AudioManager.Play(AudioClipNames.Fail);
            Handheld.Vibrate();
            failOneTime = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canMerge = false;
        failOneTime = true;
    }

    #endregion

    #region Custom Methods

    private void DragObject()
    {
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.position = position;
    }

    private void RotateObject()
    {
        if (oneTimeVibration)
        {
            switch (shape)
            {
                case ShapeName.Triangle:
                    transform.Rotate(Vector3.forward, 90f);
                    break;

                case ShapeName.Crystal:
                    if (transform.rotation.z == 0)
                    {
                        transform.Rotate(Vector3.forward, 90f);
                    }
                    else
                    {
                        transform.Rotate(Vector3.back, 90f);
                    }
                    break;
                
                case ShapeName.CrystalTwo:
                    if (transform.rotation.z == 0)
                    {
                        transform.Rotate(Vector3.forward, 90f);
                    }
                    else
                    {
                        transform.Rotate(Vector3.back, 90f);
                    }
                    break;

                case ShapeName.Hexagon:
                    if (transform.rotation.z == 0)
                    {
                        transform.Rotate(Vector3.forward, 90f);
                    }
                    else
                    {
                        transform.Rotate(Vector3.back, 90f);
                    }
                    break;

                case ShapeName.Semicircle:
                    transform.Rotate(Vector3.forward, 90f);
                    break;

                case ShapeName.Quadrant:
                    transform.Rotate(Vector3.forward, 90f);
                    break;

                case ShapeName.Diamond:
                    transform.Rotate(Vector3.forward, 90f);
                    break;

                case ShapeName.Trapzoid:
                    transform.Rotate(Vector3.forward, 90f);
                    break;

                case ShapeName.TrapzoidTwo:
                    transform.Rotate(Vector3.forward, 90f);
                    break;

                default:
                    // Handle unexpected shape types
                    Debug.LogWarning("Unknown shape type: " + shape);
                    break;
            }
        }
    }

    private void DifficultyAdjuster(int localDifficulty)
    {
        switch(localDifficulty)
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
