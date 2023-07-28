using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    // Defining the name of Death situation to manipulate animation controller...
    private string DEATH_NAME = "Death";

    // A force that will get player back...
    [SerializeField] private Vector3 knockBackForce = new Vector3 (0f, 4f, -3f);
    private Vector3 currentKnockBack;

    // This state has been entered, firstly do needed...
    public override void Construct()
    {
        // Trigger death animation...
        motor.animator?.SetTrigger(DEATH_NAME);
       
        currentKnockBack = knockBackForce;
    }

    // Return the movement depends on the behaviour of this class...
    public override Vector3 ProcessMotion()
    {
        // set move to the currentKnockBack as initial...
        Vector3 move = currentKnockBack;
        // set the movement of player in all direction as going back after hit...
        currentKnockBack = new Vector3(0,
            currentKnockBack.y -= motor.gravity * Time.deltaTime,
            currentKnockBack.z += 2.0f * Time.deltaTime );

        // If so...
        if (currentKnockBack.z > 0 )
        {
            currentKnockBack.z = 0;
            // change GameState to the GameStateDeath...
            GameManager.Instance.ChangeState(GameManager.Instance.GetComponent<GameStateDeath>());
        }

        return currentKnockBack;
    }
}
