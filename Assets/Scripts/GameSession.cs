using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("Lives and Death")]
    [SerializeField] int playerLives = 3;
    [SerializeField] GameObject defeatOverlay = default;
    [SerializeField] float deathTime = 2f;
    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] GameObject restartButton = default;
    [SerializeField] GameObject livesDisplay = default;

    [Header("Success")]
    [SerializeField] GameObject successOverlay = default;
    [SerializeField] string winString = "Fin!!";
    [SerializeField] float loadSceneDelay = 2f;


    Text defeatOverlayText;
    Text livesDisplayText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;


        if (numGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesDisplayText = livesDisplay.GetComponent<Text>();
    }

    private void Update()
    {
        livesDisplayText.text = UpdateLivesText();
    }

    private string UpdateLivesText()
    {
        string livesRemainingText = ("Lives: " + playerLives); 
        return livesRemainingText;
    }

    public void processPlayerDeath()
    {
        if (playerLives > 1)
        {
            DeductALife();
        }
        else
        {
            StartCoroutine(DeathOverlay());
        }
        
    }

    private void DeductALife()
    {
        playerLives--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DeathOverlay()
    {
        yield return new WaitForSeconds(deathTime);
        defeatOverlay.SetActive(true);
        defeatOverlayText = defeatOverlay.GetComponentInChildren<Text>();
        StartCoroutine(FadeInDeathTextAndSpawnButton(defeatOverlayText));
    }

    private IEnumerator FadeInDeathTextAndSpawnButton(Text c)
    {
        c.color = new Color(c.color.r, c.color.g, c.color.b, 0);
        while (c.color.a < 1.0f)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a + (Time.deltaTime / fadeSpeed));
            yield return null;
        }
        restartButton.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LevelComplete()
    {
        successOverlay.SetActive(true);
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(loadSceneDelay);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        int numOfScenes = SceneManager.sceneCountInBuildSettings;
        if (nextScene < numOfScenes)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            successOverlay.GetComponent<Text>().text = winString;
        }
    }

    public void ResetOverlays()
    {
        successOverlay.SetActive(false);
        defeatOverlay.SetActive(false);
        restartButton.SetActive(false);
    }
}
