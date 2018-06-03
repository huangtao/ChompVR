using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text debugText;

    [SerializeField]
    private bool debugEnabled;

    [SerializeField]
    private GameObject livesContainer;

    [SerializeField]
    private GameObject livesPrefab;

    [SerializeField]
    private GameObject gameOverDialog;

    [SerializeField]
    private GameObject winMessage;

    [SerializeField]
    private GameObject loseMessage;


    private List<GameObject> livesIcons = new List<GameObject>();


    public void Reset(int lives)
    {
        gameOverDialog.SetActive(false);
        CreateLivesIcons(lives);
        UpdateLives(lives);
        UpdateScore(0);
    }


    public void GameOver(bool win)
    {
        gameOverDialog.SetActive(true);

        if (win)
            winMessage.SetActive(true);
        else
            loseMessage.SetActive(true);
    }


    public void CreateLivesIcons(int lives)
    {
        livesIcons.Clear();

        // Rebuild the list of lives icons
        for (int i = livesIcons.Count; i < lives; i++)
        {
            livesIcons.Add((GameObject)Instantiate(livesPrefab, livesContainer.transform, false));
        }
    }


    public void UpdateLives(int lives)
    {
        // Display the appropriate number of lives icons
        for (int i = lives; i < livesIcons.Count; i++)
        {
            livesIcons[i].SetActive(false);
        }
    }


    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }


    public void Debug(string message)
    {
        if (debugEnabled)
        {
            debugText.text = message;
        }
    }
}
