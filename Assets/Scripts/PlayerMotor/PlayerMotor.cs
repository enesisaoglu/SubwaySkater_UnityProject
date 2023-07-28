using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    // Defining the names of situations player might have during game to manipulate animation controller...
    private const string ISGROUNDED_NAME = "IsGrounded";
    private const string SPEED_NAME = "Speed";
    private const string DEATHLAYER_NAME = "Death";
    private const string IDLE_NAME = "Idle";

    [HideInInspector] public Vector3 moveVector; // How far away are we, we are going to move exactly this far away...
    [HideInInspector] public float verticalVelocity; // Velocity When we are jumping, falling... (upward and downward) velocity of the player character...
    [HideInInspector] public bool isGrounded; // Whether or not we are touching floor...
    [HideInInspector] public int  currentLane; // Lanes on floor (-1,0,1)...

    public float distanceBetweenLanes = 3.0f; 
    public float baseRunSpeed = 5.0f; 
    public float baseSideawaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f; // threshold... is a maximum downward speed that the player can reach when falling...

    // Creating instances of classes that is going to be needed in that class as public and private...
    public CharacterController characterController;
    public Animator animator;
    
    private BaseState state;

    // boolean to check is game paussed? if it is, stop game play; if not, keep going...
    private bool isPaused;
    
    private void Start()
    {
        // Getting components in player component to manipulate them...
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = GetComponent<RunningState>();
        // Enter the construct based on the state comes firstly which is RunningState...
        state.Construct();

        // When the game start, player should be in paused as default...
        isPaused = true;
    }

    private void Update()
    {
        // If the game is not paused start states to keep going in game...
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

        // Feed our animator with some values...
        animator?.SetBool(ISGROUNDED_NAME, isGrounded);
        // If speed is greater than 1 in z axis as turned to absolute value, running animation will be played...
        animator?.SetFloat(SPEED_NAME, Mathf.Abs(moveVector.z));

        // Move the player...
        characterController.Move(moveVector * Time.deltaTime);

    }

    public float SnapToLane()
    {
        float moveDirection = 0.0f;

        // If the player's current x-coordinate is not already on top of the desired lane...
        if (transform.position.x != (currentLane * distanceBetweenLanes))
        {
            // This difference represents how far the player is from the center of the desired lane...
            float deltaToDesiredPosition = (currentLane * distanceBetweenLanes) - transform.position.x;
            //  If deltaToDesiredPosition is greater than 0 , move to the right (1); if not, move left (-1)
            moveDirection = (deltaToDesiredPosition > 0) ? 1 : -1;
            // Scales the movement speed for the player's lateral (sideways) movement towards the desired lane...
            moveDirection *= baseSideawaySpeed;
            // distance that is going to be taken depends on time and speed...
            float actualDistance = moveDirection * Time.deltaTime;
            // It ensures that the player doesn't overshoot the desired lane and stops exactly at the center of the lane...
            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
            {
                //  A value that would bring the player exactly to the center of the desired lane within one frame...
                moveDirection = deltaToDesiredPosition * (1 / Time.deltaTime);
            }
        }
        else
        {
            //  Indicating that the player is already on the center of the desired lane, so no lateral movement is needed...
            moveDirection = 0.0f;
        }

        return moveDirection;
    }

    public void ChangeLane(int direction)
    {
        // Set currentLane within -1,1...
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);

    }

    // Function to change the State depends on given state as paramater...
    public void ChangeState(BaseState state)
    {
        this.state.Destruct();
        this.state = state;
        this.state.Construct();
    }

    public void ApplyGravity()
    {
        // The verticalVelocity decreases over time, simulating the effect of gravity pulling the character downward...
        verticalVelocity -= gravity * Time.deltaTime;
        //  If the verticalVelocity has exceeded a certain negative threshold (-terminalVelocity). 
        if (verticalVelocity < -terminalVelocity)
        {
            // Downward velocity to the specified maximum value, preventing the player character from falling too fast.
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

    // If the player dead and want it to be respawn at the position it dead...
    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Respawn);
    }


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // hitLayerName will be set to the gameObject that has been hit...
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        // If the hitLayerName is "Death" change state to DeathState...
        if(hitLayerName == DEATHLAYER_NAME)
        {
            ChangeState(GetComponent<DeathState>());
        }
    }

    // Reset the player's attributes to default in game play...
    public void ResetPlayer()
    {
      currentLane = 0;
      transform.position = Vector3.zero;
      animator?.SetTrigger(IDLE_NAME);
      PausePlayer();
      ChangeState(GetComponent<RunningState>());
    }
}
