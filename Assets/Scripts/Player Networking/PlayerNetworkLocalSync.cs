using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkLocalSync : MonoBehaviour
{
    public float StateFrequency = 0.05f;
    private float stateSyncTimer;

    NakamaState gameState;
    private void Start()
    {
        gameState = FindObjectOfType<NakamaState>();
    }
    private void LateUpdate()
    {
        if (stateSyncTimer <= 0)
        {
            //send a packet containing the players information
            gameState.SendMatchState(OpCodes.YPosition, transform.position.y.ToString());
            stateSyncTimer = StateFrequency;
        }

        stateSyncTimer -= Time.deltaTime;
    }


}
