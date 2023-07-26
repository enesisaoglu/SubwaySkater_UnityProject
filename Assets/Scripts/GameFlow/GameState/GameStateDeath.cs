using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDeath : GameState
{

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.playerMotor.PausePlayer();
    }


    public override void UpdateState()
    {
        if(InputManager.Instance.SwipeDown)
        {
            ToMenu();
        }
        if(InputManager.Instance.SwipeUp)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        GameManager.Instance.ChangeState(GetComponent<GameStateGame>());
        GameManager.Instance.playerMotor.RespawnPlayer();
    }

    public void ToMenu()
    {
        gameManager.ChangeState(GetComponent<GameStateInit>());
    }
}
