using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
public class NakamaJoinMatch : MonoBehaviour
{

    //TODO
    //Then try and move a player and updat to server

    private NakamaConnection connection;
    private IMatchmakerUser localUser;

    public GameObject playerPrefab;
    public GameObject networkedPlayerPrefab;

    private async void Start()
    {
        connection = FindObjectOfType<NakamaConnection>();

        if (connection == null)
        {
            Debug.LogError("could not find Nakama Connection");
            return;
        }

        if (connection.GetMatchDetails() == null)
        {
            Debug.LogError("No current game");
            return;
        }

        var matchDetails = connection.GetMatchDetails();
        //actually joins the match
        var match = await connection.GetSocket().JoinMatchAsync(matchDetails.Matched);
        //localUser = Matched.Self;
        foreach (var player in match.Presences)
        {
            Debug.Log("connected user session id " + player.SessionId);
            // SpawnPlayer(match.Id, player);
        }

    }

    private void SetupPlayers()
    {
        Instantiate(playerPrefab);
        Instantiate(networkedPlayerPrefab);
    }

}
