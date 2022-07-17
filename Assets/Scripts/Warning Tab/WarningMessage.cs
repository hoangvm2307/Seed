using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningMessage : MonoBehaviour
{
    public static WarningMessage instance;
    public Text greaterSign;
    private void Awake()
    {
        instance = this;
    }
    public int maxMessages = 25;
    public GameObject chatPanel, textObject;
    public Material playerMessage, info;
    [SerializeField]
    List<Message> messageList = new List<Message>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageToChat("You pressed the space key!", Message.MessageType.info);
        }
    }
    public void SendMessageToChat(string _text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
             
        Message newMessage = new Message();
        newMessage.text = _text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.material = MessageTypeColor(messageType);
        messageList.Add(newMessage);
    }

    Material MessageTypeColor(Message.MessageType messageType)
    {
        Material material = info;
        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                material = playerMessage;
                break;
            case Message.MessageType.info:
                material = info;
                break;
        }
        return material;
    }
}//CLASS

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;
    public enum MessageType
    {
        playerMessage,
        info,
        lootInfo
    }
}