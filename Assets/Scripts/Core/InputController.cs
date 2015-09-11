using UnityEngine;


/*
 * The input controller is a singleton class which interperates the input (from a keyboard or touch) and passes
 * it to the player controller
 */
public class InputController : MonoBehaviour
{

    static public InputController instance;
    private GameManager gameController;
    // if true the player is not bound to slot positions
    public bool freeHorizontalMovement = false;
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8
    private float stationaryTime = 0;
    // move horizontally with a swipe (true) or the accelerometer (false)
    public bool swipeToMoveHorizontally = false;
    // The number of pixels you must swipe in order to register a horizontal or vertical swipe
    public Vector2 swipeDistance = new Vector2(40, 40);
    // How sensitive the horizontal and vertical swipe are. The higher the value the more it takes to activate a swipe
    public float swipeSensitivty = 2;
    // More than this value and the player will move into the rightmost slot.
    // Less than the negative of this value and the player will move into the leftmost slot.
    // The accelerometer value in between these two values equals the middle slot.
    public float accelerometerRightSlotValue = 0.25f;
    // the higher the value the less likely the player will switch slots
    public float accelerometerSensitivity = 0.1f;
    private Vector2 touchStartPosition;
    private bool acceptInput; // ensure that only one action is performed per touch gesture
#endif
#if UNITY_EDITOR || !(UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
    public bool sameKeyForTurnHorizontalMovement = false;
    public bool useMouseToMoveHorizontally = true;
    // if freeHorizontalMovement is enabled, this value will specify how much movement to apply when a key is pressed
    public float horizontalMovementDelta = 0.2f;
    // how sensitive the horizontal movement is with the mouse. The higher the value the more it takes to move
    public float horizontalMovementSensitivity = 100;
    // Allow slot changes by moving the mouse left or right
    public float mouseXDeltaValue = 100f;
    private float mouseStartPosition;
    private float mouseStartTime;


#endif

    private PlayerController playerController;

    public void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        playerController = PlayerController.instance;
        gameController = GameManager.instance;
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
        touchStartPosition = Vector2.zero;
#else
        mouseStartPosition = Input.mousePosition.x;
        mouseStartTime = Time.time;
#endif

        enabled = true;
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
        acceptInput = true;
#endif
    }

    public void GameOver()
    {
        enabled = false;
    }
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
    private void determineSwipe(Touch touch)
    {

        Vector2 diff = touch.position - touchStartPosition;

        float diffX = touch.position.x - touchStartPosition.x;

        if (diff.x == 0f)

            diff.x = 1f; // avoid divide by zero

        float verticalPercent = Mathf.Abs(diff.y / diff.x);



        if (verticalPercent > swipeSensitivty && Mathf.Abs(diff.y) > swipeDistance.y)
        {


            touchStartPosition = touch.position;

        }
        else if (verticalPercent < (1 / swipeSensitivty) && Mathf.Abs(diffX) > swipeDistance.x)
        {

            // turn if above a turn, otherwise move horizontally

            Debug.Log("swiped");

            if (swipeToMoveHorizontally)
            {

                if (freeHorizontalMovement)
                {

                    touchStartPosition = touch.position;

                    playerController.MoveHorizontally(diffX);


                }
                else if (diffX != 0)
                {
                    bool hasTurned = false;

            

                       hasTurned =  playerController.Turn(diff.x > 0 ? true : false, true);
                    if(!hasTurned)
       
                    {

                        touchStartPosition = touch.position;
                        Debug.Log("Changing Slot");
                        playerController.ChangeSlots(diffX > 0 ? true : false);
                    }
                    acceptInput = false;

                }

            }

            //			 stationaryTime = 0;

        }

    }
#endif

    public void Update()
    {
        if (gameController != null && (gameController.gameState == GameState.Playing))
        {
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
        if (Input.touchCount == 1)
        {

            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {

                if (touch.phase == TouchPhase.Began)
                {

                    acceptInput = true;
                    Debug.Log("touch began");

                    touchStartPosition = touch.position;

                    stationaryTime = 1;

                }

                else if (touch.phase == TouchPhase.Moved && stationaryTime > 0)
                {

                    stationaryTime = 1;

                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    acceptInput = false;
                    stationaryTime = 0;

                }

            }

            if (stationaryTime > 0)
            {

                stationaryTime -= Time.deltaTime;
                if (acceptInput)
                    determineSwipe(touch);

            }
        }
#else
            //  if (!swipeToMoveHorizontally)
            //  CheckHorizontalPosition(Input.acceleration.x);


            if (Input.GetKeyDown(KeyCode.A))
            {
                playerController.ChangeSlots(false);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerController.ChangeSlots(true);
            }




            // Move horizontally if the player moves their mouse more than mouseXDeltaValue within a specified amount of time




#endif
        }
    }
}

    




