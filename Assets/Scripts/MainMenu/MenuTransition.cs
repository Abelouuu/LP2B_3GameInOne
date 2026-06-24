using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransition : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
    private string sceneToLoad;

    public void LoadSceneWithFade(string sceneName)
    {
        sceneToLoad = sceneName;
        fadeAnimator.SetTrigger("FadeOut");
    }

    public void OnFadeOutFinished()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}