using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : BaseState
{

    private string SLIDE_NAME = "Slide";
    private string RUNNING_NAME = "Running";
    public float slideDuration = 1.0f;

    // Collider logic...
    private Vector3 initialCenter; // Where is the center of our character controller...
    private float initialSize; // Originial size that how big is the radius origanlly...
    private float slideStart; // When to exactly slide start...

    public override void Construct()
    {
        motor.animator?.SetTrigger(SLIDE_NAME);
        // that way we get time at the point we enter that state...
        slideStart = Time.time;
        // before sliding, store the height of characterController  as initial value...
        initialSize = motor.characterController.height;
        // before sliding as initial, store the center of characterController as initial value...
        initialCenter = motor.characterController.center;

        // manipulate height and center values, when the state enter to sliding state...
        motor.characterController.height = initialSize * 0.5f;
        motor.characterController.center = initialCenter * 0.5f;
    }

    // Put them back to what they were...
    public override void Destruct()
    {
        motor.characterController.height = initialSize;
        motor.characterController.center = initialCenter;
        motor.animator?.SetTrigger(RUNNING_NAME);
    }

    public override void Transition()
    {
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

        if(!motor.isGrounded)
        {
            motor.ChangeState(GetComponent<FallingState>());
        }

        if (InputManager.Instance.SwipeUp)
        {
            motor.ChangeState(GetComponent<JumpingState>());
        }

        if (Time.time - slideStart > slideDuration)
        {
            motor.ChangeState(GetComponent<RunningState>());
        }


    }

    public override Vector3 ProcessMotion()
    {
        Vector3 move = Vector3.zero;

        move.x = motor.SnapToLane();
        move.y = -1.0f; // A small force to keep player on the floor...
        move.z = motor.baseRunSpeed;

        return move;
    }
}
