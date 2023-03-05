using UnityEngine;

public class NakamaUI : MonoBehaviour
{

    public GameObject ShowMessagePanel;
    public GameObject ShowMainMenuPanel;
    public GameObject ShowLoginPanel;
    public GameObject ShowGameSearchPanel;

    public void ShowLogin()
    {
        ShowMessagePanel.SetActive(false);
        ShowMainMenuPanel.SetActive(false);
        ShowLoginPanel.SetActive(true);
        ShowGameSearchPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        ShowMessagePanel.SetActive(false);
        ShowMainMenuPanel.SetActive(true);
        ShowLoginPanel.SetActive(false);
        ShowGameSearchPanel.SetActive(false);
    }

    public void ShowMessages()
    {
        ShowMessagePanel.SetActive(true);
        ShowMainMenuPanel.SetActive(false);
        ShowLoginPanel.SetActive(false);
        ShowGameSearchPanel.SetActive(false);
    }

    public void ShowGameSearch()
    {
        ShowMessagePanel.SetActive(false);
        ShowMainMenuPanel.SetActive(false);
        ShowLoginPanel.SetActive(false);
        ShowGameSearchPanel.SetActive(true);
    }

    public void HideUI()
    {
        ShowMessagePanel.SetActive(false);
        ShowMainMenuPanel.SetActive(false);
        ShowLoginPanel.SetActive(false);
        ShowGameSearchPanel.SetActive(false);
    }
}
