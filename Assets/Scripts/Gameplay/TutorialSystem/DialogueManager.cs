using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class Dialogue
{
    public string[] text;
    public List<ActionName> actionName;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Action Manager")]
    public ActionManager actionManager; 

    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText; 
    public GameObject dialogueBox; 

    [Header("Dialogue Settings")]
    public Dialogue[] dialogues; 
    Dialogue currentDialogue;
    private int currentDialogueIndex = 0; 
    private int currentTextIndex = 0;


    private Coroutine typingCoroutine; 
    private bool isTyping = false;

    private void Start()
    {
        dialogueBox.SetActive(false); 
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopTyping(); // Hiển thị toàn bộ nội dung ngay lập tức
            }
            else
            {   
                DisplayNextDialogueText(); // Chuyển sang đoạn hội thoại tiếp theo
            }
        }
    }

    public void StartDialogue()
    {
        currentDialogueIndex = 0;

        dialogueBox.SetActive(true); // Hiển thị hộp thoại
        currentDialogue = dialogues[0];
        DisplayNextDialogueText(); // Bắt đầu hiển thị đoạn đầu tiên
    }

    private void DisplayNextDialogueText()
    {
        typingCoroutine = StartCoroutine(TypeMultipleSentences(currentDialogue.text[currentTextIndex]));
        foreach (var action in currentDialogue.actionName)
        {
            actionManager.PerformAction(action);
        }
        
    }

    private IEnumerator TypeMultipleSentences(string sentence)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Tốc độ gõ chữ
        }


        isTyping = false;
        StopTyping();
    }

    private void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping= false;
        // Hiển thị toàn bộ đoạn hội thoại hiện tại
        dialogueText.text = currentDialogue.text[currentTextIndex];
        currentTextIndex++;
        if (currentTextIndex+1 > currentDialogue.text.Length)
        {
            currentTextIndex = 0;
            currentDialogueIndex++;
            if (currentDialogueIndex +1> dialogues.Length)
            {
                EndDialogue();
                return;
            }
            currentDialogue = dialogues[currentDialogueIndex];
        }

    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false); // Ẩn hộp thoại
        Debug.Log("Dialogue ended.");
    }
}
