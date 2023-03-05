using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChatMessage : MonoBehaviour
{
    public TextMeshProUGUI author;
    public TextMeshProUGUI message;

    public void SetText(string authorString, string messageString)
    {
        author.text = authorString;
        message.text = messageString;
    }
}
