﻿using System.Collections;
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
    [SerializeField] AudioClip deathSFX = default;
    [SerializeField] float deathVol = 1f;

    [Header("Score")]
    [SerializeField] GameObject scoreDisplay = default;

    int score = 0;

    [Header("Success")]
    [SerializeField] GameObject successOverlay = default;
    [SerializeField] float loadSceneDelay = 2f;
    [SerializeField] float successSlowMotionFactor = 0.1f;

    //cache
    Text defeatOverlayText;
    Text scoreDisplayText;
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
        livesDisplayText.text = UpdateLivesText();
        scoreDisplayText = scoreDisplay.GetComponent<Text>();
    }

    private void Update()
    {
        
        
    }

    private string UpdateLivesText()
    {
        return ("Lives: " + playerLives);
    }

    private string UpdateScoreText()
    {
        return ("Score: " + score);
    }

    public void processPlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(DeductALife());
        }
        else
        {
            StartCoroutine(DeathOverlay());
        }
        
    }

    private IEnumerator DeductALife()
    {
        playerLives--;
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVol);
        livesDisplayText.text = UpdateLivesText();
        yield return new WaitForSecondsRealtime(deathTime);
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

    public void ReturnToStart()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelComplete()
    {
        successOverlay.SetActive(true);
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        Time.timeScale = successSlowMotionFactor;
        yield return new WaitForSecondsRealtime(loadSceneDelay);
        Time.timeScale = 1f;
        Destroy(FindObjectOfType<ScenePersist>());
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        int numOfScenes = SceneManager.sceneCountInBuildSettings;
        if (nextScene < numOfScenes)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            ReturnToStart();
        }
    }

    public void ResetOverlays()
    {
        successOverlay.SetActive(false);
        defeatOverlay.SetActive(false);
        restartButton.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreDisplayText.text = UpdateScoreText();
    }
}
