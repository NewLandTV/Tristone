using System.Collections;
using TMPro;
using UnityEngine;

public class GongiBOX : Singleton<GongiBOX>
{
    [Header("UI Settings"), Space, SerializeField]
    private GameObject logo;
    [SerializeField]
    private TMP_InputField usernameInputField;
    private Canvas canvas;

    public string Username => usernameInputField.text;

    private WaitForSeconds waitTime2f;

    private void Awake()
    {
        Setup(this);

        canvas = GetComponent<Canvas>();

        waitTime2f = new WaitForSeconds(2f);
    }

    public void Power(bool on)
    {
        canvas.enabled = on;

        if (on)
        {
            StartCoroutine(PowerOnRoutine());
        }
    }

    private IEnumerator PowerOnRoutine()
    {
        // Display logo and account management window on screen.
        yield return null;

        logo.SetActive(true);

        yield return waitTime2f;

        logo.SetActive(false);
        usernameInputField.gameObject.SetActive(true);
    }

    public void OnGameStartButtonClick()
    {
        usernameInputField.gameObject.SetActive(false);

        Power(false);

        Loading.LoadScene(Scene.Game);
    }
}
