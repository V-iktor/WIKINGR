using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        UserInterface.OnUiEvent += HandleUiEvent;
    }
    
    private void OnDisable()
    {
        UserInterface.OnUiEvent -= HandleUiEvent;
    }

    static void HandleUiEvent(UiEvent type)
    {
        switch (type)
        {
            case UiEvent.Host:
                NetworkManager.Singleton.StartHost();
                break;
            case UiEvent.Join:
                NetworkManager.Singleton.StartClient();
                break;
        }
    }
}
