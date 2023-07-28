using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private const string OTHERCOLLIDER_NAME = "Player";
    private const string PICKUP_NAME = "Pickup";
    private const string IDLE_NAME = "Idle";

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == OTHERCOLLIDER_NAME)
        {
            PickUpFish();
        }
    }

    private void PickUpFish()
    {
        animator.SetTrigger(PICKUP_NAME);

        GameStats.Instance.CollectFish();
        // Increment the fish count...
        // Increment the score...
        // Play sound...
        // Trigger a animation...

    }
}
