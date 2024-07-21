using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float elapsedTime;
    int minutes;
    int seconds;
    public bool isActive;
    public Button pause;
    public Button resume;
    public TextMeshProUGUI gameover;
    public TextMeshProUGUI levelCompleted;
    public bool search;

    private void Start()
    {
        isActive = true;
        elapsedTime = 121;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = "Time Left :" + string.Format(" {0:00}:{1:00}", minutes, seconds);
    }

    private void Update()
    {
        elapsedTime -= Time.deltaTime;
        if(elapsedTime > 0)
        {
            CDown(elapsedTime);
        }
        else
        {
            isActive = false;
            GameOver();
        }
    }

    public void CDown(float elapsedTime)
    {
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = "Time Left :" + string.Format(" {0:00}:{1:00}", minutes, seconds);
    }

    public void GameOver()
    {
        if(isActive)
        {
            pause.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            levelCompleted.gameObject.SetActive(true);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        else
        {
            pause.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            gameover.gameObject.SetActive(true);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    public void Pause()
    {
        pause.gameObject.SetActive(false);
        resume.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pause.gameObject.SetActive(true);
        resume.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
