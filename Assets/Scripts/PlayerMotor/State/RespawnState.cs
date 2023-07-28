using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RespawnState : BaseState
{
    // Defining the name of respawn situation to manipulate in animator controller...
    private const string RESPAWN_NAME = "Respawn";

    [SerializeField] private float verticalDistance = 25f; // Spawning point in y direction...
    [SerializeField] private float immunityTime = 1f; 

    private float startTime; // when to exactly respawn start...

    // This state has been entered, firstly do needed...
    public override void Construct()
    {
        // store the time this respawnTime has been created in startTime...
        startTime = Time.time;

        // Set disable characterController...
        motor.characterController.enabled = false;
        // Set the position of player that it is going to be spawned...
        motor.transform.position = new Vector3(0, verticalDistance, motor.transform.position.z);
        // Set enable characterController...
        motor.characterController.enabled = true;
        // Set downward speed as 0...
        motor.verticalVelocity = 0.0f;
        // Set currentLane to 0...
        motor.currentLane = 0;
        // Trigger the Respawn animation...
        motor?.animator.SetTrigger(RESPAWN_NAME);
    }

    // Put them back to what they were, when this class has destructed...
    public override void Destruct()
    {
        // Set camera to back which was Game...
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Game);
    }

    // Return the movement depends on the behaviour of this class...
    public override Vector3 ProcessMotion()
    {
        // Apply gravity...
        motor.ApplyGravity();

        // Set initially movement to zero in all directions...
        Vector3 move = Vector3.zero;

        move.x = motor.SnapToLane(); // movement in x direction...
        move.y = motor.verticalVelocity; // movement in y direction...
        move.z = motor.baseRunSpeed; // movement in z direction...

        return move;
    }

    // Determine the states and lanes depends on input entered...
    public override void Transition()
    {
        // If player is on ground and determined time is completed...
        if (motor.isGrounded && (Time.time - startTime) > immunityTime )
        {
            // Change the state to the back which was RunningState...
            motor.ChangeState(GetComponent<RunningState>());
        }

        if (InputManager.Instance.SwipeLeft)
        {
            // Change lane, go left...
            motor.ChangeLane(-1);
        }

        if (InputManager.Instance.SwipeRight)
        {
            // Change lane, go right...
            motor.ChangeLane(1);
        }
    }
}
