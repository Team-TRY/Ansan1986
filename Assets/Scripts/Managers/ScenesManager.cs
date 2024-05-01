using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    public static ScenesManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }
    
    public enum Scene
    {
        MainMenu,
        MainGame
    }

    public void LoadScene(Scene scene)
    {
        StartCoroutine(LoadSceneAsyncRoutine(scene));
    }

    IEnumerator LoadSceneAsyncRoutine(Scene scene)
    {
        screenFader.FadeOut();

        string sceneName = scene.ToString();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0;
        while(timer <= screenFader.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
