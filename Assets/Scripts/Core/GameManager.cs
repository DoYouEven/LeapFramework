using UnityEngine;
using System.Collections;
public enum GameState{


    None,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{


    public int currentScore = 0;
    public static GameManager instance;
    public GameState gameState = GameState.None;
    InfiniteObjectHistory objectHistory;
    ObjectManager objectManager;
    ObjectGenerator objectGenerator;
    InputController inputController;
    void Awake()
    {
        instance = this;
     }

    // Use this for initialization
    void Start()
    {
        objectHistory = InfiniteObjectHistory.instance;
        objectManager = ObjectManager.instance;
        objectGenerator = ObjectGenerator.instance;
        inputController = InputController.instance;
        objectManager.Init();
        objectHistory.Init();
        objectGenerator.Init();
        objectGenerator.StartGame();
        inputController.StartGame();
    }
    public bool IsPlaying()
    {

        return (gameState == GameState.Playing);
    }
   
    public void StartGame()
    {
        gameState = GameState.Playing;
    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            objectGenerator.SpawnObjectRun(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            objectGenerator.MoveObjects(0.01f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = GameState.Playing;
        }

    }

    public int GetScore()
    {
        return currentScore;
    }
    public void IncrementScore()
    {
        currentScore++;
    }
   
}
