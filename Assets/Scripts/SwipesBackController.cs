using UnityEngine;

public class SwipesBackController : MonoBehaviour
{
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private Collider2D leftEdgeCollider;
    [SerializeField] private Collider2D rightEdgeCollider;

    private bool isSwiping = false;
    private float swipeStartX;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (leftEdgeCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)))
            {
                isSwiping = true;
                swipeStartX = Input.GetTouch(0).position.x;
            }
            else if (rightEdgeCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)))
            {
                isSwiping = true;
                swipeStartX = Input.GetTouch(0).position.x;
            }
        }

        var uiController = FindObjectOfType<UIController>();

        if (isSwiping && !uiController.inGame)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                float swipeEndX = Input.GetTouch(0).position.x;
                float swipeDistance = swipeEndX - swipeStartX;

                if (Mathf.Abs(swipeDistance) >= swipeThreshold)
                {
                    if (swipeDistance > 0 && leftEdgeCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)))
                    {
                        uiController.BackToStartMenu();
                    }
                    else if (swipeDistance < 0 && rightEdgeCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)))
                    {
                        uiController.BackToStartMenu();
                    }
                }

                isSwiping = false;
            }
        }
    }
}
