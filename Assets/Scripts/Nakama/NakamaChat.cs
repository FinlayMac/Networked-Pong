using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using UnityEngine.UI;
using TMPro;
using Nakama.TinyJson;
public class NakamaChat : MonoBehaviour
{
    private const string RoomName = "heroes";
    private IChannel channel;
    public Button JoinRoomButton;
    public Button SendMessageButton;
    public TMP_InputField MessageInput;

    private NakamaConnection connection;
    private NakamaUI ui;
    private void Awake()
    {
        connection = GetComponent<NakamaConnection>();
        ui = GetComponent<NakamaUI>();
        JoinRoomButton.onClick.AddListener(JoinChatRoom);
        SendMessageButton.onClick.AddListener(SendMessage);
    }

    private async void JoinChatRoom()
    {
        //swaps the UI
        ui.ShowMessages();

        //joins a chat channel - chat rooms dont need created, they just need a room name to work
        channel = await connection.GetSocket().JoinChatAsync(RoomName, ChannelType.Room);
        Debug.LogFormat("Join chat channel: {0} ", channel);

    }

    private async void SendMessage()
    {

        string message = MessageInput.text;

        //converts the message to a string
        var content = new Dictionary<string, string> { { "Hello", message } }.ToJson();

        //when sending chat messages to a socket, it returns the state of the socket
        //using _ basically discards the returned variable
        _ = connection.GetSocket().WriteChatMessageAsync(channel, content);

        MessageInput.text = "";
    }

    public Transform ChatLogsUI;
    public GameObject ChatMessagePrefab;

    public void UpdateChat(IApiChannelMessage _message)
    {
        string senderID = _message.SenderId.Substring(0, 10);

        GameObject newMessage = Instantiate(ChatMessagePrefab, ChatLogsUI);
        newMessage.GetComponent<ChatMessage>().SetText(senderID, _message.Content);
        Debug.Log(_message);
    }
}
