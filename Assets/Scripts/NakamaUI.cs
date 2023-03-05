using UnityEngine;

public class NakamaUI : MonoBehaviour
{

    public GameObject ShowMessagePanel;
    public GameObject ShowChatRoomsPanel;
    public GameObject ShowLoginPanel;

    public void ShowLogin()
    {
        ShowMessagePanel.SetActive(false);
        ShowChatRoomsPanel.SetActive(false);
        ShowLoginPanel.SetActive(true);
    }

    public void ShowChatRooms()
    {
        ShowMessagePanel.SetActive(false);
        ShowChatRoomsPanel.SetActive(true);
        ShowLoginPanel.SetActive(false);
    }

    public void ShowMessages()
    {
        ShowMessagePanel.SetActive(true);
        ShowChatRoomsPanel.SetActive(false);
        ShowLoginPanel.SetActive(false);
    }

}
