using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiJoinCode : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnUiUpdateJoinCode += HandleUiUpdateJoinCode;
    }
    private void OnDisable()
    {
        GameManager.OnUiUpdateJoinCode -= HandleUiUpdateJoinCode;
    }

    void HandleUiUpdateJoinCode(string code)
    {
        GetComponent<TextMeshProUGUI>().text = code;
    }
}
