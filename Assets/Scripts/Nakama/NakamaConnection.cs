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

    //Nakama
    private NakamaChat chat;
    private NakamaUI ui;
    private NakamaLobby lobby;
    private NakamaJoinMatch join;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Nakama");
        if (objs.Length > 1) { Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);

        chat = GetComponent<NakamaChat>();
        lobby = GetComponent<NakamaLobby>();
        join = GetComponent<NakamaJoinMatch>();
        ui = GetComponent<NakamaUI>();
        ui.ShowLogin();

        LoginButton.onClick.AddListener(SubmitLogin);
    }

    // Get a reference to the UnityMainThreadDispatcher.
    // We use this to queue event handler callbacks on the main thread.
    // If we did not do this, we would not be able to instantiate objects or manipulate things like UI.
    public UnityMainThreadDispatcher mainThread;
    private async void Start() { mainThread = UnityMainThreadDispatcher.Instance(); }

    public int Testing = 0;
    async void SubmitLogin()
    {
        string email;
        string password;
        if (Testing == 1)
        {
            email = "example@mail.com";
            password = "password";
        }
        else if (Testing == 2)
        {
            email = "dave@mail.com";
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
        // Debug.Log("Started session " + session);

        //creates a new socket and events
        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket Connected");
        socket.Closed += () => Debug.Log("Socket Closed");

        socket.ReceivedChannelMessage += message => mainThread.Enqueue(() => chat.UpdateChat(message));
        socket.ReceivedMatchPresence += match => mainThread.Enqueue(() => join.OnReceivedMatchPresence(match));
        //activates when a match has been found 
        socket.ReceivedMatchmakerMatched += match => mainThread.Enqueue(() => lobby.OnReceivedMatchmakerMatched(match));

        //actually connects to the socket
        await socket.ConnectAsync(session);

        //swaps the UI
        ui.ShowMainMenu();
    }

    public ISocket GetSocket() { return socket; }

    //closes the connection when the game quits
    private void OnApplicationQuit()
    {
        //if socket is not null, close connection
        socket?.CloseAsync();
    }

    private MatchConnection matchDetails;
    public void SetMatchDetails(MatchConnection _matchDetails) { matchDetails = _matchDetails; }
    public MatchConnection GetMatchDetails() { return matchDetails; }
}
