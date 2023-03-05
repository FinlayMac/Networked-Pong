using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using UnityEngine.Events;
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
    public Button SubmitButton;
    public TextMeshProUGUI emailInput;
    public TextMeshProUGUI passwordInput;
    public TextMeshProUGUI ErrorLabel;

    private void Awake() { SubmitButton.onClick.AddListener(SubmitLogin); }

    async void SubmitLogin()
    {

        string email = "MO02FM@uhi.ac.uk";
        string password = "password";

        // string email = emailInput.text;
        // string password = passwordInput.text;

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

        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket Connected");
        socket.Closed += () => Debug.Log("Socket Closed");

    }

}
