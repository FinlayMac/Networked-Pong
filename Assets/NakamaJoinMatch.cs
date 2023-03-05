using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
public class NakamaJoinMatch : MonoBehaviour
{

    private NakamaConnection connection;
    private string localUserSessionId;
    public GameObject localPlayerPrefab;
    public GameObject networkedPlayerPrefab;
    private IDictionary<string, GameObject> players;

    private void Start()
    {
        connection = GetComponent<NakamaConnection>();
        players = new Dictionary<string, GameObject>();
    }

    public async void StartGame()
    {

        if (connection.GetMatchDetails() == null)
        {
            Debug.LogError("No current game");
            return;
        }

        var matchDetails = connection.GetMatchDetails();

        //must be set before joining the match
        //when join match async is called, the event OnReceivedMatchPresence is called straight away
        localUserSessionId = matchDetails.Matched.Self.Presence.SessionId;

        //actually joins the match
        var match = await connection.GetSocket().JoinMatchAsync(matchDetails.Matched);

        //spawn players
        foreach (var user in match.Presences)
        {
            // Debug.Log("user session id " + user.SessionId);
            SpawnPlayer(match.Id, user);
        }
    }

    private void SpawnPlayer(string matchId, IUserPresence user)
    {
        // Debug.Log("spawning user");
        // If the player has already been spawned, return early.
        if (players.ContainsKey(user.SessionId)) { return; }

        GameObject player;

        // Debug.Log("user session id " + user.SessionId);
        //Debug.LogWarning("local session id " + localUserSessionId);

        if (user.SessionId == localUserSessionId)
        {
            Vector3 position = new Vector3(-12, 0, 0);
            player = Instantiate(localPlayerPrefab, position, Quaternion.identity);
            // Debug.Log("spawning local");
        }
        else
        {
            Vector3 position = new Vector3(12, 0, 0);
            player = Instantiate(networkedPlayerPrefab, position, Quaternion.identity);
            // Debug.Log("spawning network");
        }
        // Add the player to the players array.
        players.Add(user.SessionId, player);


        // Setup the appropriate network data values if this is a remote player.
        // if (!isLocal)
        // {
        //     player.GetComponent<PlayerNetworkRemoteSync>().NetworkData = new RemotePlayerNetworkData
        //     {
        //         MatchId = matchId,
        //         User = user
        //     };
        // }

    }


    //people might take different times to join or leave the game
    /// <summary>
    /// Called when a player/s joins or leaves the match.
    /// </summary>
    /// <param name="matchPresenceEvent">The MatchPresenceEvent data.</param>
    public void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
    {
        // // For each new user that joins, spawn a player for them.
        foreach (var user in matchPresenceEvent.Joins)
        {
            // Debug.LogWarning("new user joining");
            SpawnPlayer(matchPresenceEvent.MatchId, user);
        }


        // For each player that leaves, despawn their player.
        //This works
        foreach (var user in matchPresenceEvent.Leaves)
        {
            if (players.ContainsKey(user.SessionId))
            {
                Destroy(players[user.SessionId]);
                players.Remove(user.SessionId);
            }
        }
    }

}
