using TMPro;
using UnityEngine;

public class UiPlayers : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnUiUpdatePlayerList += HandleUiUpdateJoinCode;
    }
    private void OnDisable()
    {
        GameManager.OnUiUpdatePlayerList -= HandleUiUpdateJoinCode;
    }

    void HandleUiUpdateJoinCode(string playersListText)
    {
        GetComponent<TextMeshProUGUI>().text = playersListText;
    }
}
