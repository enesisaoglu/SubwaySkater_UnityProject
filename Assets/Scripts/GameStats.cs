using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get { return instance; } }
    public static GameStats instance;

    // Score...
    public float score;
    public float Highscore;
    public float distanceModifier = 1.5f;

    // Fish...
    public int totalFish;
    public int fishCollectedThisSession;
    public float pointsPerFish = 10.0f;

    // Internal Cooldown...
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f;


    // Action...
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        float score = GameManager.Instance.playerMotor.transform.position.z * distanceModifier;
        score += fishCollectedThisSession * pointsPerFish;

        if(score > Highscore)
        {
            this.score = score;
            if(Time.time - lastScoreUpdate > scoreUpdateDelta)
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(this.score);
            }
        }
    }

    public void CollectFish()
    {
        fishCollectedThisSession++;
        OnCollectFish?.Invoke(fishCollectedThisSession);
    }

    public void ResetSession()
    {
        score = 0;
        fishCollectedThisSession = 0;

        OnCollectFish?.Invoke(fishCollectedThisSession);
        OnScoreChange?.Invoke(score);
    }

    public string ScoreToText()
    {
        return score.ToString("0000000");
    }

    public string FishToText()
    {
        return fishCollectedThisSession.ToString("000");
    }
}
