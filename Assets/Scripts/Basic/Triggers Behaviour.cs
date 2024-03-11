using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersBehaviour : IntEventInvoker
{
    public ShapeColors color = ShapeColors.Null;

    bool isOccupied = true;

    bool enableColor = true;

    int colorRotation = 0;

    public bool Occupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    private void OnMouseUpAsButton()
    {
        if(enableColor)
        {
            ColorChange(colorRotation);
            colorRotation++;
            if(colorRotation > 2)
            {
                colorRotation = 0;
            }
        }
    }

    void ColorChange(int colour)
    {
            switch (colour)
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
