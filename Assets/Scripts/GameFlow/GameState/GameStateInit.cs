using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInit : GameState
{

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Init);
    }
    public override void UpdateState()
    {
        if (InputManager.Instance.Tap)
        {
            gameManager.ChangeState(GetComponent<GameStateGame>());
        }
    }
}
