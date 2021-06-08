using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    int mainMenuIndex = 0;

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
