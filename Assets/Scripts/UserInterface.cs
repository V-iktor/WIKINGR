using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UiEvent
{
    Host,
    Join,
}
public class UserInterface : MonoBehaviour
{
    delegate void UiDelegate(UiEvent type);

    private UiDelegate _thisUiDelegate;

    public void OnHost()
    {
        _thisUiDelegate.Invoke(UiEvent.Host);
    }
    
    public void OnJoin()
    {
        _thisUiDelegate.Invoke(UiEvent.Host);
    }
}
