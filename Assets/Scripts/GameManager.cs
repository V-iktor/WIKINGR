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

    void HandleUiEvent(UiEvent type)
    {
        Debug.Log(type.ToString());
    }
}
