using System;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  delegate void UiUpdateJoinCode(string joinCode);
    public static event UiUpdateJoinCode OnUpdateJoinCode;
    private void OnEnable()
    {
        UserInterface.OnUiEvent += HandleUiEvent;
    }
    
    private void OnDisable()
    {
        UserInterface.OnUiEvent -= HandleUiEvent;
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    static async void HandleUiEvent(UiEvent type)
    {
        switch (type)
        {
            case UiEvent.Host:
                // NetworkManager.Singleton.StartHost();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in. Player ID: {AuthenticationService.Instance.PlayerId}");
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
                Debug.Log($"Host Allocation ID: {allocation.AllocationId}, region: {allocation.Region}");
                Debug.Log("Host - Got join code: " + await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId));
                OnUpdateJoinCode?.Invoke(await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId));
                break;
            case UiEvent.Join:
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in. Player ID: {AuthenticationService.Instance.PlayerId}");

                // NetworkManager.Singleton.StartClient();
                break;
        }
    }
}
