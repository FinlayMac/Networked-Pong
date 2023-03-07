using UnityEngine;
using Nakama;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Nakama.TinyJson;

public class NakamaLobby : MonoBehaviour
{
    public Button LookForGameButton;
    public Button CancelSearchButton;
    public Button ClaimDailyRewardButton;
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
        ClaimDailyRewardButton.onClick.AddListener(ClaimDailyReward);
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

    private async void ClaimDailyReward()
    {
        //calls the custom rpc canclaimdailyreward
        string canClaimRpcId = "canclaimdailyreward";
        IApiRpc responseData = await connection.GetSocket().RpcAsync(canClaimRpcId);

        Debug.Log(responseData.Payload);

        //receives the payload from the server and decodes the result
        ServerBoolMessage decoded = JsonParser.FromJson<ServerBoolMessage>(responseData.Payload);
        if (!decoded.result)
        {
            Debug.Log("You've already had yours");
            return;
        }

        Debug.Log("can get daily reward ");

        string ClaimRpcId = "claimdailyreward";
        IApiRpc responseData2 = await connection.GetSocket().RpcAsync(ClaimRpcId);

        Debug.Log(responseData2.Payload);
        ClaimDailyRewardButton.gameObject.SetActive(false);

    }

    public async void CanClaimDailyReward()
    {
        //calls the custom rpc canclaimdailyreward
        string canClaimRpcId = "canclaimdailyreward";
        IApiRpc responseData = await connection.GetSocket().RpcAsync(canClaimRpcId);

        Debug.Log(responseData.Payload);

        //receives the payload from the server and decodes the result
        ServerBoolMessage decoded = JsonParser.FromJson<ServerBoolMessage>(responseData.Payload);
        if (!decoded.result)
        {
            Debug.Log("You've already had yours");
            ClaimDailyRewardButton.gameObject.SetActive(false);
        }
    }

    //for RPCs that require payloads
    // //         Dictionary<string, object> requestPayload = new Dictionary<string, object>()
    // //    {
    // //        { "lbId" , lbId },
    // //        { "score" , score }
    // //    };
    // //         var payload = Nakama.TinyJson.JsonWriter.ToJson(requestPayload);

    //         IApiRpc responseData = await nakamaClient.RpcAsync(session, rpcId, payload);
    //         Debug.Log(responseData.Payload);

}
