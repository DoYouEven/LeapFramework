using UnityEngine;

using UnityEngine.UI;
public class UIManager :MonoBehaviour
{


    private Text gameScore;


    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        gameScore = GameObject.Find("Score(Code)").GetComponent<Text>();

    }

    void Update()
    {
        gameScore.text = gameManager.GetScore().ToString();
    }

}