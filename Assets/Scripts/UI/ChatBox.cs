using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    #region Singleton
    public static ChatBox instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

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
            AddMessage(input.text, true);
            input.text = "";
        }
    }

    public void AddMessage(string text, bool fromPlayer)
    {
        if (messages.Count >= MAX)
        {
            Destroy(messages[0].textObject.gameObject);
            messages.RemoveAt(0);
        }

        if (fromPlayer)
            text = World.playerName + ": " + text;

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textPrefab, textPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        if (!fromPlayer)
            newMessage.textObject.color = Color.yellow;

        messages.Add(newMessage);
    }
}

public class Message
{
    public string text;
    public Text textObject;
}