using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    // Defining the name of jumping situation to manipulate in animator controller...
    public const string JUMP_NAME = "Jump";
    public float jumpForce = 8.5f; // Jump force to describe velocity in air...

    // This state has been entered, firstly do needed...
    public override void Construct()
    {
        motor.verticalVelocity = jumpForce;
        // Trigger the Jumping animation...
        motor.animator?.SetTrigger(JUMP_NAME);
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
        // If the character has been falling after jumping...
        if(motor.verticalVelocity < 0)
        {
            // Change state to FallingState...
            motor.ChangeState(GetComponent<FallingState>());
        }

        // If the screen has been swiped left...
        if (InputManager.Instance.SwipeLeft)
        {
            // Change lane, go left...
            motor.ChangeLane(-1);
        }
        // If the screen has been swiped right...
        if (InputManager.Instance.SwipeRight)
        {
            // Change lane, go right...
            motor.ChangeLane(1);
        }
    }

}
