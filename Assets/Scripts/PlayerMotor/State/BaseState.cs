using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected PlayerMotor motor;

    // Will usually be called once we first enter the state...
    public virtual void Construct() { }
    // Will usually be called once we leave...
    public virtual void Destruct() { }
    // Will be called in the update loop constantly...
    public virtual void Transition() { }

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Will determine how to move depending on which state we are in...
    public virtual Vector3 ProcessMotion()
    {
        Debug.Log("Process motion is not implemented in " + this.ToString());
        return Vector3.zero;
    }

}
