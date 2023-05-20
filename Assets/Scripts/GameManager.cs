using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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

    async void HandleUiEvent(UiEvent type)
    {
        switch (type)
        {
            case UiEvent.Host:
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in. Player ID: {AuthenticationService.Instance.PlayerId}");
                var allocation = await RelayService.Instance.CreateAllocationAsync(4);
                Debug.Log($"Host Allocation ID: {allocation.AllocationId}, region: {allocation.Region}");
                var relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log($"Host - Got join code: {relayCode}");
                OnUiUpdateJoinCode?.Invoke(relayCode);

                var lobbyData = new Dictionary<string, DataObject>()
                {
                    ["Relay"] = new(DataObject.VisibilityOptions.Public, relayCode),
                };

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
                var response = await LobbyService.Instance.QueryLobbiesAsync();

                var foundLobbies = response.Results;

                if (foundLobbies.Any()) // Try to join a random lobby if one exists
                {
                    Debug.Log("Found lobbies:\n" + JsonConvert.SerializeObject(foundLobbies));

                    var randomLobby = foundLobbies[0];

                    currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId: randomLobby.Id);

                    Debug.Log($"Joined lobby {currentLobby.Name} ({currentLobby.Id})");

                    StartCoroutine(StartClientLoop());
                }
                break;
            case UiEvent.Start:
                NetworkManager.Singleton.StartHost();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    IEnumerator StartClientLoop()
    {
        while(!NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Start client attempt...");
            yield return new WaitForSeconds(1);
        }
    }
}
