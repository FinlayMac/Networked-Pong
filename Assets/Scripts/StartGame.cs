using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private NakamaJoinMatch match;
    private void Start()
    {
        match = FindObjectOfType<NakamaJoinMatch>();

        if (match == null)
        {
            Debug.LogError("could not find Nakama Connection");
            return;
        }

        match.StartGame();
    }
}
