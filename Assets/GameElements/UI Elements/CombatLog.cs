using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CombatLog : MonoBehaviour
{
    public static CombatLog Instance;

    public TextMeshProUGUI logText;
    public int maxLines = 5;

    Queue<string> messages = new Queue<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Log(string message)
    {
        messages.Enqueue(message);

        if (messages.Count > maxLines)
            messages.Dequeue();

        logText.text = string.Join("\n", messages);
    }
}
