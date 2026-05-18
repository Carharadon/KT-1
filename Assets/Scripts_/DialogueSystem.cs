using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Message
    {
        public string characterName;
        [TextArea(2, 5)] public string text;
    }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("Settings")]
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private float autoAdvanceDelay = 3f;

    private List<Message> messageDatabase = new List<Message>()
    {
        new Message { characterName = "Рыцарь", text = "Привет! Кто ты такой?" },
        new Message { characterName = "Гоблин", text = "Я охраняю этот мост. Тебе не пройти!" },
        new Message { characterName = "Рыцарь", text = "У меня важная миссия, пропусти меня мирно." },
        new Message { characterName = "Гоблин", text = "Ха-ха! Только через бой!" },
        new Message { characterName = "Рыцарь", text = "Ладно, ты сам напросился. Обнажи свой меч!" },
        new Message { characterName = "Гоблин", text = "Мой топор быстрее твоего слова, чужестранец." },
        new Message { characterName = "Рыцарь", text = "Мы посмотрим, чья сталь крепче." },
        new Message { characterName = "Гоблин", text = "Защищайся! Сила камня пребудет со мной!" },
        new Message { characterName = "Рыцарь", text = "Это твой последний шанс отступить." },
        new Message { characterName = "Гоблин", text = "Никогда! В атаку-у-у!" }
    };

    private int currentMessageIndex = 0;
    private bool isTyping = false;
    private bool cancelTypingRequested = false;
    private bool isPaused = false;
    private string currentFullText = "";

    void Start()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(DialogueFlowRoutine());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping) cancelTypingRequested = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isPaused = !isPaused;
            Debug.Log(isPaused ? "Пауза диалога" : "Возобновление диалога");
        }
    }

    private IEnumerator DialogueFlowRoutine()
    {
        while (currentMessageIndex < messageDatabase.Count)
        {
            while (isPaused) yield return null;

            Message msg = messageDatabase[currentMessageIndex];
            currentFullText = $"<b>[{msg.characterName}]:</b> {msg.text}";

            yield return StartCoroutine(TypeTextRoutine(currentFullText));

            float timer = 0f;
            bool clickedNext = false;

            while (timer < autoAdvanceDelay && !clickedNext)
            {
                while (isPaused) yield return null;

                if (Input.GetMouseButtonDown(0) && !isTyping)
                {
                    clickedNext = true;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            currentMessageIndex++;
        }

        dialoguePanel.SetActive(false);
    }

    private IEnumerator TypeTextRoutine(string fullText)
    {
        isTyping = true;
        cancelTypingRequested = false;
        textDisplay.text = "";

        for (int i = 0; i <= fullText.Length; i++)
        {
            while (isPaused) yield return null;

            if (cancelTypingRequested)
            {
                textDisplay.text = fullText;
                break;
            }

            textDisplay.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }
}
