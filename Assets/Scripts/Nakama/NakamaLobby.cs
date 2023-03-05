using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NakamaLobby : MonoBehaviour
{
    public Button LookForGameButton;
    public Button CancelSearchButton;
    private NakamaConnection connection;
    private NakamaUI ui;
    private NakamaJoinMatch join;
    private IMatchmakerTicket currentMatchmakingTicket;
    private string ticket;
    private void Awake()
    {
        connection = GetComponent<NakamaConnection>();
        ui = GetComponent<NakamaUI>();
        join = GetComponent<NakamaJoinMatch>();
        LookForGameButton.onClick.AddListener(SearchForGame);
        CancelSearchButton.onClick.AddListener(CancelSearch);
    }

    private async void SearchForGame()
    {
        //swaps the UI
        ui.ShowGameSearch();

        currentMatchmakingTicket = await connection.GetSocket().AddMatchmakerAsync("*", 2, 2);
        ticket = currentMatchmakingTicket.Ticket;
    }

    public async void OnReceivedMatchmakerMatched(IMatchmakerMatched newMatch)
    {
        //Debug.Log("found match");
        //creates match details to load when the scene changes
        MatchConnection newMatchConnection = new MatchConnection(newMatch);
        connection.SetMatchDetails(newMatchConnection);

         SceneManager.LoadScene(1);
        //  join.StartGame();
        //  ui.HideUI();
    }

    private async void CancelSearch()
    {
        await connection.GetSocket().RemoveMatchmakerAsync(currentMatchmakingTicket);
        currentMatchmakingTicket = null;
        ui.ShowMainMenu();
    }
}
