using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RespawnState : BaseState
{

    private string RESPAWN_NAME = "Respawn";

    [SerializeField] private float verticalDistance = 25f;
    [SerializeField] private float immunityTime = 1f;

    private float startTime;

    public override void Construct()
    {
        // store the time this respawnTime has been created in startTime...
        startTime = Time.time;

        motor.characterController.enabled = false;
        motor.transform.position = new Vector3(0, verticalDistance, motor.transform.position.z);
        motor.characterController.enabled = true;

        motor.verticalVelocity = 0.0f;
        motor.currentLane = 0;
        motor?.animator.SetTrigger(RESPAWN_NAME);
    }

    public override void Destruct()
    {
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Game);
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
        if (motor.isGrounded && (Time.time - startTime) > immunityTime )
        {
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
