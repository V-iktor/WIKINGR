using UnityEngine;

public enum UiEvent
{
    Host,
    Join,
}
public class UserInterface : MonoBehaviour
{
   public  delegate void UiDelegate(UiEvent type);
   public static event UiDelegate OnUiEvent;

    public void OnHost()
    {
        OnUiEvent.Invoke(UiEvent.Host);
    }
    
    public void OnJoin()
    {
        OnUiEvent.Invoke(UiEvent.Join);
    }
}
