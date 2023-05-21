using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void StartHost()
    {
        GameObject.FindGameObjectWithTag("Network").gameObject.GetComponent<NetworkManager>().StartHost();
        Cursor.lockState = CursorLockMode.Locked;
        transform.parent.gameObject.SetActive(false);
    }
}
