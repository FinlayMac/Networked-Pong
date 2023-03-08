using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nakama;
using Nakama.TinyJson;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    private NakamaJoinMatch match;
    private NakamaConnection connection;
    private NakamaJoinMatch join;

    public Button WinGameButton;

    public UnityMainThreadDispatcher mainThread;

    private void Start()
    {
        mainThread = UnityMainThreadDispatcher.Instance();
        match = FindObjectOfType<NakamaJoinMatch>();

        if (match == null)
        {
            Debug.LogError("could not find Nakama Connection");
            return;
        }
        connection = match.gameObject.GetComponent<NakamaConnection>();
        join = match.gameObject.GetComponent<NakamaJoinMatch>();
        WinGameButton.onClick.AddListener(ForceWinGame);

        match.StartGame();

    }

    private async void ForceWinGame()
    {

        var matchId = join.GetMatchID();
        await connection.GetSocket().LeaveMatchAsync(matchId);


        // for RPCs that require payloads
        Dictionary<string, object> requestPayload = new Dictionary<string, object>()
           {
               { "prop1" , "value1" },
               { "prop2" , "value2" }
           };
        var payload = Nakama.TinyJson.JsonWriter.ToJson(requestPayload);

        //calls the custom rpc wingamestats
        string winGameRpcId = "wingamestats";
        IApiRpc responseData = await connection.GetSocket().RpcAsync(winGameRpcId, payload);
        //connection.GetSession(),

        mainThread.Enqueue(() =>
               {
                   SceneManager.LoadScene(0);
                   //navigate to main menu
               });
    }
}
