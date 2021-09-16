using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public GameObject textPanel;
    public GameObject textPrefab;
    public InputField input;

    const int MAX = 20;

    List<Message> messages = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        input.DeactivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (!input.isFocused && Input.GetKeyDown(KeyCode.Return))
            input.ActivateInputField();

        if (input.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            AddMessage(input.text);
            input.text = "";
        }
    }

    void AddMessage(string text)
    {
        text = World.playerName + ": " + text;

        if (messages.Count >= MAX)
        {
            Destroy(messages[0].textObject.gameObject);
            messages.RemoveAt(0);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textPrefab, textPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messages.Add(newMessage);
    }
}

public class Message
{
    public string text;
    public Text textObject;
}