using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Text;

public class PlayerNetworkRemoteSync : MonoBehaviour
{

    public RemotePlayerNetworkData NetworkData;

    private NakamaConnection connection;
    private void Start()
    {
        connection = FindObjectOfType<NakamaConnection>();
        connection.GetSocket().ReceivedMatchState += OnReceivedmatchState;
    }

    public async void OnReceivedmatchState(IMatchState matchState)
    {
        Debug.Log("Getting Data");
        // if the incoming data does not match the data attached to this game object, ignore
        if (matchState.UserPresence.SessionId != NetworkData.User.SessionId)
        {
            Debug.Log("match state not for this object");
            return;
        }

        Debug.Log("For this object");
        if (matchState.OpCode == OpCodes.YPosition)
        {

            float positionY = float.Parse(Encoding.UTF8.GetString(matchState.State));
            Debug.Log("Moving player " + positionY);
            transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
        }

    }
}
