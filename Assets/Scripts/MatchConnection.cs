using UnityEngine;
using Nakama;

public class MatchConnection
{
    public string MatchId { get; set; }
    public string HostId { get; set; }
    public string OpponentId { get; set; }
    public IMatchmakerMatched Matched { get; set; }
    // More properties can be stored here!

    public MatchConnection(IMatchmakerMatched matched)
    {
        Matched = matched;
    }
   
}
