using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : BaseState
{
    // Defining the name of falling situation to manipulate in animator controller...
    private const string FALL_NAME = "Fall";

    // This state has been entered, firstly do needed...
    public override void Construct()
    {
        // Trigger the falling animation...
        motor.animator?.SetTrigger(FALL_NAME);
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
        // IF player has been grounded...
        if(motor.isGrounded)
        {
            // Change state to RunningState...
            motor.ChangeState(GetComponent<RunningState>());
        }
    }
}
