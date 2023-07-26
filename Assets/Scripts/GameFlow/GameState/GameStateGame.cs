using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateGame : GameState
{
    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.playerMotor.ResumePlayer();

        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Game);
    }
}


