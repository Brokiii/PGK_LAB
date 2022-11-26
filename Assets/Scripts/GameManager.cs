using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_GAME_OVER,
    GS_LEVELCOMPLETED
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager instance;
    public Canvas pauseMenuCanvas;
    public Canvas inGameCanvas;
    public Canvas gameOverCanvas;
    public Canvas levelCompletedCanvas;
    public Text coalText;
    public int coals = 0;
    public Image[] keysTab;
    public Image[] heartTab;
    private int Keys = 0;
    public int Hearts = 2;
    float timer = 0;
    public Text timerText;
    public Text enemykilledText;
    private int enemykilled = 0;
    public int maxKeyNumber = 3;
    public bool keysCompleted = false;
    Scene currentScene;
    public Text scoreText;
    public Text bestScoreText;
    private int maxSecsToHihtScore = 120;

    void Awake()
    {
        instance = this;
        currentScene = SceneManager.GetActiveScene();
        InGame();
        if (currentScene.name == "SampleScene")
        {
            foreach (Image image in keysTab)
            {
                image.color = Color.grey;
            }
        }

        if(!PlayerPrefs.HasKey("HighscoreLevel1"))
        {
            PlayerPrefs.SetInt("HighscoreLevel1", 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && currentGameState == GameState.GS_PAUSEMENU)
        {
           InGame();
        }
        else if(Input.GetKey(KeyCode.Escape) && currentGameState == GameState.GS_GAME)
        {
            PauseMenu();
        }
        timer += Time.deltaTime;
        float timertemp = timer;
        int minutes = 0;
        while(timertemp > 60)
        {
            timertemp -= 60;
            minutes += 1;
        }
        int seconds = (int)timertemp;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentScene.name == "Level2")
        {
            if (timer > LevelGenerator.instance.maxTimeGame)
            {
                LevelGenerator.instance.Finish();
            }
        }
    }
    public void ResetTimer()
    {
        timer = 0;
    }
    public void addEnemyKilled()
    {
        enemykilled++;
        enemykilledText.text = enemykilled.ToString();
    }
    public void addKey(int number, Color color)
    {
        Keys += 1;
        keysTab[number].color = color;
        if (Keys == maxKeyNumber)
            keysCompleted = true;
    }
    public void removeHeart()
    {
        if (Hearts < 0)
        {
            GameOver();
            return;
        }

        heartTab[Hearts].enabled = false;
        Hearts--;
    }
    public void addHeart()
    {
        if (Hearts >= 2)
            return;

        Hearts += 1;
        heartTab[Hearts].enabled = true;
    }
    public void AddCoins(int value)
    {
        coals += value;
        coalText.text = coals.ToString() + "kg";
    }

    public void ResetCoins()
    {
        coals = 0;
        coalText.text = coals.ToString() + "kg";
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;


        if (newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "SampleScene")
            {
                int score = Hearts * 100 + coals + enemykilled * 50 + (maxSecsToHihtScore - (int)timer) * 20 + 1500;
                if (PlayerPrefs.GetInt("HighscoreLevel1") < score)
                {
                    PlayerPrefs.SetInt("HighscoreLevel1", score);
                }
                Debug.Log("test");
                bestScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel1");
                scoreText.text = "Score: " + score;
            } else if (currentScene.name == "Level2")
            {
                int score = Hearts * 100 + coals + enemykilled * 50 + (maxSecsToHihtScore - (int)timer) * 20 + 1900;
                if (PlayerPrefs.GetInt("HighscoreLevel2") < score)
                {
                    PlayerPrefs.SetInt("HighscoreLevel2", score);
                }
                Debug.Log("test");
                bestScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel2");
                scoreText.text = "Score: " + score;
            }
        }
        /*if(newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        } else
        {
            inGameCanvas.enabled = false;
        }*/
        inGameCanvas.enabled = (currentGameState == GameState.GS_GAME);
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        gameOverCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
    }

    void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene("Level2");
    }

}
