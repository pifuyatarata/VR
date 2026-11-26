using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(2, 4)] public string text;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueLine[] lines;
    public float typingSpeed = 0.03f;   // seconds per letter
    public bool startOnPlay = true;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public TMP_Text nextButtonText;
    public GameObject talkButton;

    private int index = 0;
    private bool typing = false;

    void Start()
    {
        if (startOnPlay) StartDialogue();
        else CloseDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);
        talkButton.SetActive(false);
        ShowLine();
    }

    public void Next()
    {
        // First click while typing: show the whole line at once
        if (typing)
        {
            StopAllCoroutines();
            dialogueText.maxVisibleCharacters = 99999;
            typing = false;
            return;
        }

        index++;
        if (index < lines.Length) ShowLine();
        else CloseDialogue();
    }

    void ShowLine()
    {
        speakerText.text = lines[index].speaker;
        dialogueText.text = lines[index].text;
        nextButtonText.text = index == lines.Length - 1 ? "Close" : "Next";
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        typing = true;
        int total = dialogueText.text.Length;
        for (int i = 0; i <= total; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }
        typing = false;
    }

    void CloseDialogue()
    {
        StopAllCoroutines();
        typing = false;
        dialoguePanel.SetActive(false);
        talkButton.SetActive(true);
    }
}