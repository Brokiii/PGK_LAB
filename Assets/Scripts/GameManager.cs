using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_GAME_OVER,
    GS_LEVELCOMPLETED
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public Text coalText;
    private int coals = 0;
    public Image[] keysTab;
    public Image[] heartTab;
    private int Keys = 0;
    private int Hearts = 2;
    float timer = 0;
    public Text timerText;
    public Text enemykilledText;
    private int enemykilled = 0;

    void Awake()
    {
        instance = this;
        foreach(Image image in keysTab)
        {
            image.color = Color.grey;
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
        if(currentGameState == GameState.GS_PAUSEMENU)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Escape))
            {
                InGame();
            }
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
    }
    public void removeHeart()
    {
        if (Hearts < 0)
            return;

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

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if(newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        } else
        {
            inGameCanvas.enabled = false;
        }
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

    void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }
}
