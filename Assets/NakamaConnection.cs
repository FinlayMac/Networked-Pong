using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using Nakama.TinyJson;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;



using System.Linq;
using System.Threading.Tasks;


public class NakamaConnection : MonoBehaviour
{
    //UI Swaps
    public GameObject LoginPanel;
    public GameObject ChatRoomPanel;
    public GameObject MessagePanel;


    private const string RoomName = "heroes";

    //make sure you use a key for actual deployment
    private readonly IClient client = new Client("http", "127.0.0.1", 7350, "defaultkey");
    private ISession session;
    private ISocket socket;
    private IChannel channel;

    //UI
    public Button LoginButton;
    public Button JoinRoomButton;
    public Button SendMessageButton;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI ErrorLabel;
    public TMP_InputField MessageInput;

    private void Awake()
    {
        LoginButton.onClick.AddListener(SubmitLogin);
        JoinRoomButton.onClick.AddListener(JoinChatRoom);
        SendMessageButton.onClick.AddListener(SendMessage);
    }

    private UnityMainThreadDispatcher mainThread;
    private async void Start() { mainThread = UnityMainThreadDispatcher.Instance(); }

    public bool Testing = true;
    async void SubmitLogin()
    {
        string email;
        string password;
        if (Testing)
        {
            email = "";
            password = "";
        }
        else
        {
            email = emailInput.text;
            password = passwordInput.text;
        }

        try
        {
            session = await client.AuthenticateEmailAsync(email, password);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            ErrorLabel.text = e.Message;
            return;
        }
        Debug.Log("Started session " + session);

        //creates a new socket and events
        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket Connected");
        socket.Closed += () => Debug.Log("Socket Closed");

        // Get a reference to the UnityMainThreadDispatcher.
        // We use this to queue event handler callbacks on the main thread.
        // If we did not do this, we would not be able to instantiate objects or manipulate things like UI.
        socket.ReceivedChannelMessage += message => mainThread.Enqueue(() => UpdateChat(message));

        //actually connects to the socket
        await socket.ConnectAsync(session);

        //swaps the UI
        LoginPanel.SetActive(false);
        ChatRoomPanel.SetActive(true);
    }


    async void JoinChatRoom()
    {
        //swaps the UI
        ChatRoomPanel.SetActive(false);
        MessagePanel.SetActive(true);

        //joins a chat channel - chat rooms dont need created, they just need a room name to work
        channel = await socket.JoinChatAsync(RoomName, ChannelType.Room);
        Debug.LogFormat("Join chat channel: {0} ", channel);

    }

    async void SendMessage()
    {

        string message = MessageInput.text;

        //converts the message to a string
        var content = new Dictionary<string, string> { { "Hello", message } }.ToJson();

        //when sending chat messages to a socket, it returns the state of the socket
        //using _ basically discards the returned variable
        _ = socket.WriteChatMessageAsync(channel, content);

        MessageInput.text = "";
    }

    public Transform ChatLogsUI;
    public GameObject ChatMessagePrefab;

    private void UpdateChat(IApiChannelMessage _message)
    {
        string senderID = _message.SenderId.Substring(0, 10);

        GameObject newMessage = Instantiate(ChatMessagePrefab, ChatLogsUI);
        newMessage.GetComponent<ChatMessage>().SetText(senderID, _message.Content);
        Debug.Log(_message);
    }

    //closes the connection when the game quits
    private void OnApplicationQuit()
    {
        //if socket is not null, close connection
        socket?.CloseAsync();
    }
}
