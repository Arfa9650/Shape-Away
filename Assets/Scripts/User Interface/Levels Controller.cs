using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    RectTransform screens;

    [SerializeField]
    GameObject transition;

    // Define variables for animation
    private Vector3 startPosition;

    private Vector3 targetPosition;
    
    private float animationDuration = 1.0f; // Duration of the animation in seconds
    
    private float elapsedTime = 0f; // Elapsed time since animation started

    float animTime = 0.5f;

    private Vector2 touchStartPos;

    private float swipeThreshold = 15f;

    public int numOfItems = 1;

    public int distanceBetweenItem = 2500;

    #endregion

    #region Methods

    void Update()
    {

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check for the beginning of a touch
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }

            // Check for the end of a touch
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeDelta = touch.position - touchStartPos;

                // Check if the swipe distance exceeds the threshold
                if (Mathf.Abs(swipeDelta.x) > swipeThreshold)
                {
                    // Swipe is horizontal
                    if (swipeDelta.x > 0)
                    {
                        // Swipe left, call back function
                        Back();
                    }
                    else
                    {
                        // Swipe right, call next function
                        Next();
                    }
                }
            }
        }
    }

    public void ShapesLevel()
    {
        AudioManager.Play(AudioClipNames.Button);
        StartCoroutine(LoadScene(2));
        
    }

    IEnumerator Transition(int level)
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(1.1f); 
        SceneManager.LoadScene(level);
    }

    public void Next()
    {
        if (screens.anchoredPosition.x % distanceBetweenItem == 0 && screens.anchoredPosition.x > -distanceBetweenItem * numOfItems)
        {
            // Calculate target position
            targetPosition = screens.anchoredPosition - new Vector2(distanceBetweenItem, 0); // Adjust position

            // Store current position as the start position
            startPosition = screens.anchoredPosition;

            // Start the animation
            StartCoroutine(MoveUIObject());
        }
    }

    public void Back()
    {
        if (screens.anchoredPosition.x % distanceBetweenItem == 0 && screens.anchoredPosition.x < 0)
        {
            // Calculate target position for moving backwards
            targetPosition = screens.anchoredPosition + new Vector2(distanceBetweenItem, 0); // Adjust position

            // Store current position as the start position
            startPosition = screens.anchoredPosition;

            // Start the animation
            StartCoroutine(MoveUIObject());
        }
    }

    public void Menu()
    {
        AudioManager.Play(AudioClipNames.Button);
        StartCoroutine(LoadScene(0));
    }

    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(animTime);
        SceneManager.LoadScene(index);
    }

    private IEnumerator MoveUIObject()
    {
        AudioManager.Play(AudioClipNames.Button);
        // Reset elapsed time
        elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate interpolation factor
            float t = elapsedTime / animationDuration;

            // Interpolate between start and target position
            screens.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t*2);

            yield return null; // Wait for the next frame
        }

        // Ensure final position is exactly the target position
        screens.anchoredPosition = targetPosition;
    }

    #endregion
}
