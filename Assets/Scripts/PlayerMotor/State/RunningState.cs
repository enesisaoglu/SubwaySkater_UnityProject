using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{
    // This state has been entered, firstly do needed...
    public override void Construct()
    {
        motor.verticalVelocity = 0f;
    }

    // Return the movement depends on the behaviour of this class...
    public override Vector3 ProcessMotion()
    {
        // Set initially movement to zero in all directions...
        Vector3 move = Vector3.zero;

        move.x = motor.SnapToLane(); // movement in x direction... 
        move.y = -1.0f; // movement in y direction, A small force to keep player on the floor...
        move.z = motor.baseRunSpeed; // movement in z direction...

        return move;
    }

    // Determine the states and lanes depends on input entered...
    public override void Transition()
    {
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
        // If the screen has been swiped up and the player was on ground...
        if (InputManager.Instance.SwipeUp && motor.isGrounded)
       {
            // Change to jumping state...
           motor.ChangeState(GetComponent<JumpingState>());
       }
        // If the player was not on ground...
       if(!motor.isGrounded)
        {
            // Change state to FallingState, generally after JumpState...
            motor.ChangeState(GetComponent<FallingState>());
        }
       // If the screen has been swiped down...
        if (InputManager.Instance.SwipeDown)
        {
            // Change state to SlidingState...
            motor.ChangeState(GetComponent<SlidingState>());
        }

    }


}
