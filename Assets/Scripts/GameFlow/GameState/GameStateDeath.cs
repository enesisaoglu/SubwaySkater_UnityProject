using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDeath : GameState
{

    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;
    // Completion circle fields...
    [SerializeField] private Image completeCircle;

    public float timeToDecision = 2.5f;
    private float deathTime;

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.playerMotor.PausePlayer();

        deathTime = Time.time;
        deathUI.SetActive(true);
        completeCircle.gameObject.SetActive(true);

        // Prior to saving, set the highscore if needed...
        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score;
            currentScore.color = Color.green;
        }
        else
        {
            currentScore.color = Color.white;    
        }

        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instance.Save();

        highScore.text = "Highscore : " + SaveManager.Instance.save.Highscore;
        currentScore.text = GameStats.Instance.ScoreToText();
        fishTotal.text = "Total Fish : " + SaveManager.Instance.save.Fish;
        currentFish.text = GameStats.Instance.FishToText();
    }


    public override void Destruct()
    {
        deathUI.SetActive(false);
    }

    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision;
        completeCircle.fillAmount = 1 - ratio;

        if(ratio > 1)
        {
            completeCircle.gameObject.SetActive(false);
        }
    }
    
    public void ResumeGame()
    {
        GameManager.Instance.playerMotor.RespawnPlayer();
        GameManager.Instance.ChangeState(GetComponent<GameStateGame>());
    }

    public void ToMenu()
    {
        gameManager.ChangeState(GetComponent<GameStateInit>());

        GameManager.Instance.playerMotor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();
    }
}
