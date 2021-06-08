using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public void OnPlayButtonClick(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
