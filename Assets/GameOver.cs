using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text Score;
    [SerializeField] TMPro.TMP_Text HighScore;
    [SerializeField] GameObject GameOverScreen;
    private float currentTime;
    private bool record;
    private float hi;

    public void Start()
    {
       
        record = true;
        currentTime = 0.0f;

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            hi = 0;
            PlayerPrefs.SetString("HighScore", 0.ToString());

        }
        else
        {
            HighScore.text = PlayerPrefs.GetString("HighScore");
            hi = float.Parse(PlayerPrefs.GetString("HighScore"));
        }
    }

    public void Update()
    {
        if (record)
        {
            currentTime += 1 * Time.deltaTime;
        }
        Score.text = currentTime.ToString();
        if (hi < currentTime)
        {
            PlayerPrefs.SetString("HighScore", currentTime.ToString());
            HighScore.text = PlayerPrefs.GetString("HighScore");
        }
    }

    public void SetGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }

    public void ResetGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
