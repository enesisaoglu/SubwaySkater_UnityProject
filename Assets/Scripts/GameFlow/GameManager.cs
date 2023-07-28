using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameCamera
    {
        Init = 0,
        Game = 1,
        Shop = 2,
        Respawn = 3
    }

    public static GameManager Instance { get {  return instance; } }
    public static GameManager instance;

    public PlayerMotor playerMotor;
    public WorldGeneration worldGeneration;
    public SceneChunkGeneration sceneChunkGeneration;
    public GameObject[] cameras;

    private GameState state;


    private void Start()
    {
        instance = this;
        state = gameObject.GetComponent<GameStateInit>();
        state.Construct(); 
    }

    private void Update()
    {
        state.UpdateState();
    }

    public void ChangeState(GameState state)
    {
        this.state.Destruct();
        this.state = state;
        this.state.Construct();
    }

    public void ChangeCamera(GameCamera camera)
    {
        foreach (GameObject go in cameras)
        {
            go.SetActive(false);
        }

        cameras[(int)camera].SetActive(true);
    }
}
