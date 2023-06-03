using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string func;

    public string scene;

    public TextMeshPro tm;

    public void loadScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void SelectColor(TextMeshPro tm)
    {
        tm.color = new Color32(255, 255, 255, 255);
    }

    public void UnSelectColor(TextMeshPro tm)
    {
        tm.color = new Color32(255, 25, 0, 255);
    }
}
