using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{

    public string JUMP_NAME = "Jump";
    public float jumpForce = 7.0f;


    public override void Construct()
    {
       motor.verticalVelocity = jumpForce;
        motor.animator?.SetTrigger(JUMP_NAME);
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
        if(motor.verticalVelocity < 0)
        {
            motor.ChangeState(GetComponent<FallingState>());
        }
    }

}
