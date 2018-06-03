using System.Collections;
using Prime31.TransitionKit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int maxLives = 3;

    [SerializeField]
    private float speedBoostPeriod = 10f;

    /// <summary>
    /// Amount of time that a ghost is vulnerable once a power pellet has been eaten
    /// </summary>
    [SerializeField]
    private int ghostVulnerabilityPeriod = 15;

    [Header("Scoring")]
    [SerializeField]
    private int pelletValue = 10;

    [SerializeField]
    private int powerValue = 25;

    [SerializeField]
    private int fruitValue = 100;

    [SerializeField]
    private int ghostValue = 200;

    [Header("Game Mode")]
    [SerializeField]
    private WinMode winMode = WinMode.CollectAllCoins;

    [SerializeField]
    private int itemsNeededToWin = 0;


    private int lives;
    private int score;
    private int pelletsRemaining;
    private int pelletsCollected;
    private int ghostsEaten;
    private int powerPelletsInEffect = 0;

    private Autowalk playerAutowalk;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private MusicManager mm;
    private UIController ui;


    void Start()
    {
        mm = MusicManager.Instance;
        ui = UIController.Instance;

        playerAutowalk = player.GetComponent<Autowalk>();

        // Player's starting position will be the future spawn position
        spawnPosition = player.transform.position;
        spawnRotation = player.transform.rotation;

        NewGame();
    }


    public void CollectObject(string tag)
    {
        switch (tag)
        {
            case "pellet":
                CollectPellet();
                break;
            case "power":
                CollectPowerPellet();
                break;
            case "fruit":
                CollectFruit();
                break;
        }

        ui.UpdateScore(score);
        CheckWinCondition();
    }


    private void CollectPellet()
    {
        score += pelletValue;
        pelletsRemaining--;
        pelletsCollected++;
    }


    private void CollectPowerPellet()
    {
        PlayerController.Instance.HasPowerBall = true;
        GhostManager.Instance.StartVulnerability();
        StartCoroutine(TimeoutVulnerability());
        StartCoroutine(IncreaseSpeed());
        score += powerValue;
    }


    /// <summary>
    /// Waits the vulnerability period and then ends vulnerability.
    /// </summary>
    private IEnumerator TimeoutVulnerability()
    {
        yield return new WaitForSeconds(ghostVulnerabilityPeriod);

        if (powerPelletsInEffect <= 0)
            GhostManager.Instance.EndVulnerability();
    }


    private IEnumerator IncreaseSpeed()
    {
        powerPelletsInEffect++;
        playerAutowalk.IsRunning = true;

        yield return new WaitForSeconds(speedBoostPeriod);

        if (--powerPelletsInEffect <= 0)
            playerAutowalk.IsRunning = false;

        PlayerController.Instance.HasPowerBall = false;
    }


    private void CollectFruit()
    {
        fruitValue = (int)System.Math.Ceiling(((fruitValue * 1.1f) / 10)) * 10;
        score += fruitValue;
    }


    public void KillPlayer()
    {
        ui.UpdateLives(--lives);

        if (lives == 0)
        {
            StartCoroutine(Lose());
            return;
        }

        mm.PlayOneShot(Sound.PlayerDied);

        // Fade out and back in
        var fader = new FadeTransition() { fadeToColor = Color.white };
        TransitionKit.instance.transitionWithDelegate(fader);
        
        // Resets ghosts to their starting positions and pauses
        GhostManager.Instance.RespawnGhosts();

        RespawnPlayer();
    }


    public void EatGhost()
    {
        score += ghostValue;
        ghostsEaten++;
        ui.UpdateScore(score);
    }


    public void RegisterPellet()
    {
        pelletsRemaining++;
    }


    public void NewGame()
    {
        lives = maxLives;
        score = 0;
        pelletsCollected = 0;
        ghostsEaten = 0;
        ui.Reset(lives);
        RespawnPlayer();
        ApplicationModel.GameState = GameState.Playing;
    }


    void CheckWinCondition()
    {
        switch (winMode)
        {
            case WinMode.CollectAllCoins:
                if (pelletsRemaining <= 0)
                    StartCoroutine(Win());
                break;

            case WinMode.CollectXCoins:
                if (pelletsCollected >= itemsNeededToWin)
                    StartCoroutine(Win());
                break;

            case WinMode.ScoreXPoints:
                if (score >= itemsNeededToWin)
                    StartCoroutine(Win());
                break;

            case WinMode.EatXGhosts:
                if (ghostsEaten >= itemsNeededToWin)
                    StartCoroutine(Win());
                break;
        }
    }


    IEnumerator Lose()
    {
        ApplicationModel.GameState = GameState.LevelLost;
        yield return GameOver(false);
    }


    IEnumerator Win()
    {
        ApplicationModel.GameState = GameState.LevelWon;
        yield return GameOver(true);
    }


    IEnumerator GameOver(bool win)
    {
        ui.GameOver(win);
        yield return new WaitForSeconds(5);
        ApplicationModel.GameState = GameState.Playing;
        SceneManager.LoadScene("Menu");
    }


    void RespawnPlayer()
    {
        player.transform.position = spawnPosition;
        player.transform.rotation = spawnRotation;
    }
}
