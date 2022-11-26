using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Text highscoreLevel1Text;
    public Text highscoreLevel2Text;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("HighscoreLevel1"))
        {
            PlayerPrefs.SetInt("HighscoreLevel1", 0);
        }
        highscoreLevel1Text.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel1");

        if (!PlayerPrefs.HasKey("HighscoreLevel2"))
        {
            PlayerPrefs.SetInt("HighscoreLevel2", 0);
        }
        highscoreLevel2Text.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel2");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }
    public void onLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("SampleScene"));
    }

    public void onLevel2ButtonPressed()
    {
        StartCoroutine(StartGame("Level2"));
    }
}
