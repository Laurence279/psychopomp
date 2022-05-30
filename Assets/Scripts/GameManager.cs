 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] GameObject gameOverOverlay = null;
    [SerializeField] GameObject gameWinOverlay = null;
    [SerializeField] GameObject instructionsOverlay = null;

    private bool isSpedup = false;
    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1.0f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowInstructions()
    {
        instructionsOverlay.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsOverlay.SetActive(false);
    }
    public void GameOver()
    {
        gameOverOverlay.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinGame()
    { 
        gameWinOverlay.SetActive(true);
        Time.timeScale = 0;
    }
    public void SpeedUp(int val)
    {
        if(isSpedup)
        {
            isSpedup = false;
            Time.timeScale = 1;
            return;
        }
        isSpedup = true;
        Time.timeScale = val;
    }

}
