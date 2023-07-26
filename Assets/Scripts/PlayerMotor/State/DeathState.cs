using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{

    private string DEATH_NAME = "Death";

    [SerializeField] private Vector3 knockBackForce = new Vector3 (0f, 4f, -3f);
    private Vector3 currentKnockBack;
    public override void Construct()
    {
        motor.animator?.SetTrigger(DEATH_NAME);
        currentKnockBack = knockBackForce;
    }

    public override Vector3 ProcessMotion()
    {
        Vector3 move = currentKnockBack;

        currentKnockBack = new Vector3(0,
            currentKnockBack.y -= motor.gravity * Time.deltaTime,
            currentKnockBack.z += 2.0f * Time.deltaTime );

        if (currentKnockBack.z > 0 )
        {
            currentKnockBack.z = 0;
            GameManager.Instance.ChangeState(GameManager.Instance.GetComponent<GameStateDeath>());
        }

        return currentKnockBack;
    }
}
