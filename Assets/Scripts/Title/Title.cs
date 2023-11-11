using System.Collections;
using TMPro;
using UnityEngine;

public enum VersionType
{
    Development,
    Alpha,
    Beta,
    Release
}

public class Title : MonoBehaviour
{
    [Header("Version Settings"), Space, SerializeField]
    private TextMeshProUGUI versionText;
    [SerializeField]
    private VersionType versionType;

    private Canvas canvas;

    private void Awake()
    {
        Application.targetFrameRate = 30;

        versionText.text = $"v{Application.version} {versionType.ToString().Substring(0, 3)}";

        canvas = GetComponent<Canvas>();
    }

    private IEnumerator Start()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        canvas.enabled = false;

        GongiBOX.Instance.Power(true);
    }
}
