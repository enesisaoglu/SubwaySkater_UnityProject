using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private string ISGROUNDED_NAME = "IsGrounded";
    private string SPEED_NAME = "Speed";
    private string DEATHLAYER_NAME = "Death";

    [HideInInspector] public Vector3 moveVector; // How far away are we, we are going to move exactly this far away...
    [HideInInspector] public float verticalVelocity; // Velocity When we are jumping, falling...
    [HideInInspector] public bool isGrounded; // Whether or not we are touching floor...
    [HideInInspector] public int  currentLane; // Lanes on floor (-1,0,1)...

    public float distanceBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSideawaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    public CharacterController characterController;
    public Animator animator;
    
    private BaseState state;
    private bool isPaused;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = GetComponent<RunningState>();
        state.Construct();

        isPaused = true;
    }

    private void Update()
    {
        if (!isPaused)
        {
            UpdateMotor();
        }
    }

    private void UpdateMotor()
    {
        // check if we are grounded...
        isGrounded = characterController.isGrounded;

        // How should we be moving right now? based on state...
        moveVector = state.ProcessMotion();

        // Are we trying to change state?...
        state.Transition();

        // Feed our animator some values...
        animator?.SetBool(ISGROUNDED_NAME, isGrounded);
        animator?.SetFloat(SPEED_NAME, Mathf.Abs(moveVector.z));

        // Move the player...
        characterController.Move(moveVector * Time.deltaTime);

    }

    public float SnapToLane()
    {
        float returnValue = 0.0f;

        // If we are not directly on top of a lane...
        if (transform.position.x != (currentLane * distanceBetweenLanes))
        {
            // Calculate the current position of the player...
            float deltaToDesiredPosition = (currentLane * distanceBetweenLanes) - transform.position.x;
            returnValue = (deltaToDesiredPosition > 0) ? 1 : -1;
            returnValue *= baseSideawaySpeed;

            float actualDistance = returnValue * Time.deltaTime;
            if(Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
            {
                returnValue = deltaToDesiredPosition * (1 / Time.deltaTime);
            }
        }
        else
        {
            returnValue = 0.0f;
        }

        return returnValue;
    }

    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);

    }

    public void ChangeState(BaseState state)
    {
        this.state.Destruct();
        this.state = state;
        this.state.Construct();
    }

    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;

        if (verticalVelocity < -terminalVelocity)
        {
            verticalVelocity = -terminalVelocity;
        }
    }

    public void PausePlayer()
    {
        isPaused = true;
    }

    public void ResumePlayer()
    {
        isPaused = false;
    }

    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Respawn);
    }


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if(hitLayerName == DEATHLAYER_NAME)
        {
            ChangeState(GetComponent<DeathState>());
        }
    }
}
