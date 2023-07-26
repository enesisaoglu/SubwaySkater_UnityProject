using UnityEngine;
using UnityEngine.WSA;

public class InputManager : MonoBehaviour
{

    //There should be only one InputManager in the scene...
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    //Action schemes...
    private RunnerInputAction actionScheme;

    //Configuration...
    // sqrSwipeDeadzone is a threshold that defines how much movement must occur for it to be considered a swipe...
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;

    #region public properties
    public bool Tap { get { return tap; } }
    public Vector2 TouchPosition { get { return touchPosition; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion


    #region private properties
    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    #endregion



    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }

    //Specifically designed to be executed after all other "Update()" methods have been called for all objects in the scene. 
    private void LateUpdate()
    {
        ResetInput();
    }

    private void ResetInput()
    {
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

    }

    private void SetupControl()
    {
        actionScheme = new RunnerInputAction();

        //Register different actions...
        actionScheme.Gameplay.Tap.performed += Tap_performed;
        actionScheme.Gameplay.TouchPosition.performed += TouchPosition_performed;
        actionScheme.Gameplay.StartDrag.performed += StartDrag_performed;
        actionScheme.Gameplay.EndDrag.performed += EndDrag_performed;
    }

    private void EndDrag_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        /* 
         * delta represents the difference between the starting touch/cursor position
         *(startDrag) and the ending touch/cursor position (touchPosition-(released)) during a drag input...
         *
         */
        Vector2 delta = touchPosition - startDrag;
        // Calculate the square distance of the delta vector...
        float sqrDistance = delta.sqrMagnitude;

        //Confirmed swipe...
        // If the magnitude of the sqrDistance is enough to be considered then swipe has occured...
        if (sqrDistance > sqrSwipeDeadzone)
        {
            // Calculate absolute values of delta.x and delta.y to determine the direction of the swipe...
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            // If the x-component is larger than the y-component, it means the swipe is more horizontal...
            if (x > y) //Left or Right...
            {
                if(delta.x > 0)
                {
                    swipeRight = true;
                }
                else
                {
                    swipeLeft = true;
                }
            }
            // else, the y-component is larger, it means the swipe is more vertical...
            else //Up or Down
            {
                if (delta.y > 0)
                {
                    swipeUp = true;
                }
                else
                {
                    swipeDown = true;
                }
            }
        }

        startDrag = Vector2.zero;
    }

    private void StartDrag_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // Assigned the first touchPosition to the startDrag...
        startDrag = touchPosition;
    }

    private void TouchPosition_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // Read the value of the touchPosition in the screen...
        touchPosition = obj.ReadValue<Vector2>();
    }

    private void Tap_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Tap is performed...
        tap = true;
    }


    public void OnEnable()
    {
        actionScheme.Enable();
    }

    public void OnDisable()
    {
        actionScheme.Disable();
    }
}


/*  
 *      Dynamic Update
 *      InputManager that processes the inputs...
 *      PlayerMotor uses these inputs to move...
 *
 *      Late Update
 *      InputManager resets these inputs...
 */
