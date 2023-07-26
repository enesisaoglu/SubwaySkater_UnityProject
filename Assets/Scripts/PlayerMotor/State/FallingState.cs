using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : BaseState
{
    private string FALL_NAME = "Fall";
    public override void Construct()
    {
        motor.animator?.SetTrigger(FALL_NAME);
    }
    public override Vector3 ProcessMotion()
    {
        // Apply gravity...
        motor.ApplyGravity();

        // Create our return value...
        Vector3 move = Vector3.zero;

        move.x = motor.SnapToLane();
        move.y = motor.verticalVelocity;
        move.z = motor.baseRunSpeed;

        return move;
    }

    public override void Transition()
    {
        if(motor.isGrounded)
        {
            motor.ChangeState(GetComponent<RunningState>());
        }
    }
}
