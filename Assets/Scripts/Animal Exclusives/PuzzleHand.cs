using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHand : IntEventInvoker
{
    [SerializeField]
    GameObject handText;

    Animator anim;

    string shapeName = "None";  

    private GameObject nearestShape;

    private float moveSpeed = 3f;

    private float deactivationDistance = 0.54f;

    private float rotationTolerance = 10f;

    private Vector2 defaultPosition = new Vector2(0.53f, -4.3f);

    private bool translate;

    private bool canDisppear = true;

    private void Start()
    {
        translate = true;
        anim = GetComponent<Animator>();
        transform.position = defaultPosition;
        EventManager.AddListener(EventNames.FitShape, UpdateShape);
        UpdateShape(0);
    }

    void Update()
    {
        GameObject currentObject = GameObject.Find("Duplicate_" + shapeName);

        // Find all GameObjects with the specified tag
        GameObject shape = GameObject.Find(shapeName.Replace("(Clone)",""));

        // Initialize variables to track the nearest shape and its distance
        float minDistance = Mathf.Infinity;
        //GameObject closestShape = null;

        // Loop through each shape to find the nearest one
        if( shape!= null)
        {
            // Calculate distance to the current shape
            float distance = Vector3.Distance(transform.position, shape.transform.position);

            // Check if the current shape is closer than the previous closest one
            //if (distance < minDistance)
            {
                minDistance = distance;
                //closestShape = shape;
            }
        }

        // Update reference to the nearest shape
        nearestShape = shape;

        // Move towards the nearest shape if one is found
        if (nearestShape != null)
        {
            Vector3 tempNearPos = nearestShape.transform.position;
            tempNearPos.x += 0.53f;

            // Calculate direction towards the nearest shape
            Vector3 direction = (tempNearPos - transform.position).normalized;

            // Get the rotations of the current object and the nearest shape
            Quaternion currentRotation = currentObject.transform.rotation;
            Quaternion nearestShapeRotation = nearestShape.transform.rotation;


            // Check if the difference in rotation angles is within a tolerance
            if (Quaternion.Angle(currentRotation, nearestShapeRotation) < rotationTolerance && translate)
            {
                if (handText)
                    handText.SetActive(false);

                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            else if (translate)
            {
                handText.SetActive(true);
                transform.position = defaultPosition;
            }

            if (minDistance < deactivationDistance)
            {
                // Deactivate the hand object
                translate = false;
                Disappear();
            }
        }
    }

    void UpdateShape(int num)
    {
        canDisppear = false;
        transform.position = defaultPosition;
        translate = true;
        if (shapeName != null)
            shapeName = SpriteSlicer.pieceName;
        Debug.Log(shapeName);
    }

    public void Disappear()
    {
        canDisppear = true;
        anim.SetTrigger("Disappear");
    }

    public void Deactivate()
    {
        translate = true;
        anim.ResetTrigger("Disappear");
        handText.SetActive(true);
        if (canDisppear)
        {
            transform.position = defaultPosition;
            gameObject.SetActive(false);
        }
    }
}
