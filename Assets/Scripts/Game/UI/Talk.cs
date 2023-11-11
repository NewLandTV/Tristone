using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class Talk : MonoBehaviour
{
    [Header("Character Components"), Space, SerializeField]
    private Character character;
    [SerializeField]
    private CameraController cameraController;

    [Space, Header("UI Settings"), Space, SerializeField]
    private Canvas gameCanvas;
    private Canvas talkCanvas;

    [Space, SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI messageText;

    [Space, Header("Talk Settings"), Space, SerializeField]
    private int cps;

    // Use for Typing coroutine
    private bool isTyping;

    private Action callback;

    private WaitForSeconds waitTime;

    private void Awake()
    {
        talkCanvas = GetComponent<Canvas>();

        waitTime = new WaitForSeconds(1f / cps);
    }

    public void StartTalk(TalkData data, Action callback = null)
    {
        // If data is null or is talking, return
        if (data == null || talkCanvas.enabled)
        {
            return;
        }

        this.callback = callback;

        character.Movable = false;
        cameraController.Controllable = false;

        // Enable canvas
        gameCanvas.enabled = false;
        talkCanvas.enabled = true;

        StartCoroutine(TalkRoutine(data.groups));
    }

    private IEnumerator TalkRoutine(TalkData.Group[] groups, int index = 0)
    {
        nameText.text = groups[index].name;
        messageText.text = string.Empty;

        Coroutine typingCoroutine = StartCoroutine(Typing(groups[index].message));

        yield return null;

        // Touch to skip
        while (isTyping && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        StopCoroutine(typingCoroutine);

        messageText.text = groups[index].message;

        yield return null;

        // Touch to continue
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        // Continue if the talk data is not over
        if (index < groups.Length - 1)
        {
            StartCoroutine(TalkRoutine(groups, index + 1));

            yield break;
        }

        // Disable canvas
        gameCanvas.enabled = true;
        talkCanvas.enabled = false;
        character.Movable = true;
        cameraController.Controllable = true;

        callback?.Invoke();
    }

    private IEnumerator Typing(string message)
    {
        isTyping = true;

        StringBuilder msg = new StringBuilder(message.Length);

        for (int i = 0; i < message.Length; i++)
        {
            yield return waitTime;

            msg.Append(message[i]);

            messageText.text = $"{msg}";
        }

        isTyping = false;
    }
}
