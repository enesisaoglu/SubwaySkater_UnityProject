using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;


    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.playerMotor.currentLane = 0;
        GameManager.Instance.playerMotor.ResumePlayer();

        GameManager.Instance.ChangeCamera(GameManager.GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFish;
        GameStats.Instance.OnScoreChange += OnScoreChanege;


        gameUI.SetActive(true);
    }

    private void OnCollectFish(int amountCollected)
    {
        fishCount.text = GameStats.Instance.FishToText();
    }

    private void OnScoreChanege(float score)
    {
        scoreCount.text = GameStats.Instance.ScoreToText();
    }

    public override void Destruct()
    {
        gameUI.SetActive(false);
        GameStats.Instance.OnCollectFish -= OnCollectFish;
        GameStats.Instance.OnScoreChange -= OnScoreChanege;

        
    }

    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}


