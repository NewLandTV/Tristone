using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scene
{
    Title,
    Loading,
    Game
}

public class Loading : MonoBehaviour
{
    [Header("UI Settings"), Space, SerializeField]
    private Image progressBar;
    [SerializeField]
    private TextMeshProUGUI percentText;

    private static Scene targetScene;

    private void Start()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    public static void LoadScene(Scene targetScene)
    {
        Loading.targetScene = targetScene;

        SceneManager.LoadScene((int)Scene.Loading);
    }

    private IEnumerator LoadSceneRoutine()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)targetScene);

        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            progressBar.fillAmount = asyncOperation.progress;
            percentText.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100f)}%";

            yield return null;
        }

        // Fake loading
        for (float timer = 0f; timer < 1f; timer += Time.unscaledDeltaTime)
        {
            float value = Mathf.Lerp(0.9f, 1f, timer);

            progressBar.fillAmount = value;
            percentText.text = $"{Mathf.RoundToInt(value * 100f)}%";

            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }
}
