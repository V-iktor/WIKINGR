using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LoadFirstScene : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsServer) NetworkManager.Singleton.SceneManager.LoadScene("Playground", LoadSceneMode.Single);
    }
}
