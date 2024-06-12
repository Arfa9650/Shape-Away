using UnityEngine;
using System.Collections;

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

    float rotationDuration = 0.5f;

    private float[] allowedRotations = new float[] { 0f, 90f, 180f, 270f };

    //Difficulty Values

    int difficulty;

    bool canRotate = false;
    
    bool enableColor = false;

    bool failOneTime = true;

    SpriteRenderer sr;

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
        sr = GetComponent<SpriteRenderer>();
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
                sr.sortingOrder = -1;

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
        if (oneTimeVibration && Time.timeScale != 0 && IsRotationAllowed(transform.rotation.eulerAngles.z))
        {
            switch (shape)
            {
                case ShapeName.Triangle:
                case ShapeName.Semicircle:
                case ShapeName.Quadrant:
                case ShapeName.Diamond:
                case ShapeName.Trapzoid:
                case ShapeName.TrapzoidTwo:
                    StartCoroutine(RotateOverTime(90f));
                    break;

                case ShapeName.Crystal:
                case ShapeName.CrystalTwo:
                case ShapeName.Hexagon:
                    if (Mathf.Approximately(transform.rotation.eulerAngles.z, 0))
                    {
                        StartCoroutine(RotateOverTime(90f));
                    }
                    else
                    {
                        StartCoroutine(RotateOverTime(-90f));
                    }
                    break;

                default:
                    // Handle unexpected shape types
                    Debug.LogWarning("Unknown shape type: " + shape);
                    break;
            }
        }
    }

    private bool IsRotationAllowed(float currentRotation)
    {
        // Normalize the current rotation to the range [0, 360)
        currentRotation = currentRotation % 360f;
        if (currentRotation < 0)
            currentRotation += 360f;

        foreach (float allowedRotation in allowedRotations)
        {
            if (Mathf.Approximately(currentRotation, allowedRotation))
            {
                return true;
            }
        }
        return false;
    }

    private float GetNextAllowedRotation(float currentRotation)
    {
        // Normalize the current rotation to one of the allowed rotations
        currentRotation = Mathf.Round(currentRotation / 90f) * 90f;

        for (int i = 0; i < allowedRotations.Length; i++)
        {
            if (Mathf.Approximately(currentRotation, allowedRotations[i]))
            {
                // Get the next rotation in the sequence
                return allowedRotations[(i + 1) % allowedRotations.Length];
            }
        }

        // Default to 0 if no match is found (should not happen)
        return allowedRotations[0];
    }

    private IEnumerator RotateOverTime(float angle)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(Vector3.forward * angle);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is set at the end
        transform.rotation = endRotation;
    }

/*private void RotateObject()
{
    if (oneTimeVibration && Time.timeScale != 0)
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
}*/

private void DifficultyAdjuster(int localDifficulty)
    {
        switch(localDifficulty)
        {
            case <= 1:
                //nothing for now
                break;

            case >2:
                canRotate = true;
                break;
        }
    }

    #endregion
}
