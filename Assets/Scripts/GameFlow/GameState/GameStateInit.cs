using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI fishcountText;


    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Init);

        highScoreText.text = "Highscore : " + SaveManager.Instance.save.Highscore.ToString();
        fishcountText.text = "Fish : " + SaveManager.Instance.save.Fish.ToString();

        menuUI.SetActive(true);
    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        gameManager.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
    }

    public void OnShopClick()
    {
        gameManager.ChangeState(GetComponent<GameStateShop>());
    }
}
