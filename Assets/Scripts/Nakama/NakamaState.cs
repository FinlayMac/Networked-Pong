using UnityEngine;
using Nakama;
public class NakamaState : MonoBehaviour
{
    private NakamaConnection connection;
    private NakamaJoinMatch joinMatch;
    private void Awake()
    {
        joinMatch = GetComponent<NakamaJoinMatch>();
        connection = GetComponent<NakamaConnection>();
    }

    ISocket socket;
    string matchID = "";

    public void SendMatchState(long opcode, string MatchDataJson)
    {
        if (matchID == "")
        {
            socket = connection.GetSocket();
            matchID = joinMatch.GetMatchID();
        }
        // Debug.Log("updated match state" + MatchDataJson);
        socket.SendMatchStateAsync(matchID, opcode, MatchDataJson);
    }

    public async void OnReceivedmatchState(IMatchState matchState)
    {
        //Nothing yet

    }
}
