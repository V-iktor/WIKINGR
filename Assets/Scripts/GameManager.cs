using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public  delegate void UiUpdateJoinCode(string joinCode);
    public static event UiUpdateJoinCode OnUiUpdateJoinCode;
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
        var random = new Random().Next(1, 1000).ToString();
        var options = new InitializationOptions().SetProfile(random);
        await UnityServices.InitializeAsync(options);
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
                var relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log($"Host - Got join code: {relayCode}");
                OnUiUpdateJoinCode?.Invoke(relayCode);
                // Populate the new lobby with some data; use indexes so it's easy to search for
                var lobbyData = new Dictionary<string, DataObject>()
                {
                    ["Relay"] = new DataObject(DataObject.VisibilityOptions.Public, relayCode),
                };

                // Create a new lobby
                var currentLobby = await LobbyService.Instance.CreateLobbyAsync(
                    lobbyName: relayCode,
                    maxPlayers: 4,
                    options: new CreateLobbyOptions()
                    {
                        Data = lobbyData,
                    });

                Debug.Log($"Created new lobby {currentLobby.Name} ({currentLobby.Id})");
                break;
            case UiEvent.Join:
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in. Player ID: {AuthenticationService.Instance.PlayerId}");

                // NetworkManager.Singleton.StartClient();
                break;
        }
    }
}
