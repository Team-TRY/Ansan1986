using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    
    [SerializeField] private GameObject loaderUI;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Animator animator;
    
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

        DontDestroyOnLoad(gameObject);
    }

    public enum Scene
    {
        KNW_TestScene,
        MainMenu,
        Lobby,
        Stage01,
        Stage02,
        Stage03,
        Ending
    }
    
    //기본 씬 로드
    public void LoadScene(Scene scene)
    {
        string sceneName = scene.ToString();
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    //현재 씬 로드
    public void LoadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsync(currentSceneName));
    }
    
    //게임 종료
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    //비동기 씬 로딩, 로당 화면
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Time.timeScale = 1;
        progressSlider.value = 0;
        
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        
        loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;

            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
                loaderUI.SetActive(false);
                
                animator.SetTrigger("FadeIn");
            }

            yield return null;
        }
    }
}
