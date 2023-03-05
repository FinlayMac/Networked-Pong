using UnityEngine;
using Nakama;
using UnityEngine.UI;
using TMPro;
using System;


public class NakamaConnection : MonoBehaviour
{
    //make sure you use a key for actual deployment
    private readonly IClient client = new Client("http", "127.0.0.1", 7350, "defaultkey");
    private ISession session;
    private ISocket socket;

    //UI
    public Button LoginButton;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI ErrorLabel;

    private NakamaChat chat;
    private NakamaUI ui;
    private void Awake()
    {
        chat = GetComponent<NakamaChat>();
        ui = GetComponent<NakamaUI>();
        ui.ShowLogin();

        LoginButton.onClick.AddListener(SubmitLogin);
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
            email = "example@mail.com";
            password = "password";
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
        socket.ReceivedChannelMessage += message => mainThread.Enqueue(() => chat.UpdateChat(message));

        //actually connects to the socket
        await socket.ConnectAsync(session);

        //swaps the UI
        ui.ShowChatRooms();
    }

    public ISocket GetSocket() { return socket; }

    //closes the connection when the game quits
    private void OnApplicationQuit()
    {
        //if socket is not null, close connection
        socket?.CloseAsync();
    }
}
