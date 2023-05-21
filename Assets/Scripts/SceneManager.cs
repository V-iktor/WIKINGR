using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer) NetworkManager.Singleton.SceneManager.LoadScene("Playground", LoadSceneMode.Single);
    }
}
