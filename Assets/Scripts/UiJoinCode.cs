using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiJoinCode : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnUpdateJoinCode += HandleUpdateJoinCode;
    }
    private void OnDisable()
    {
        GameManager.OnUpdateJoinCode -= HandleUpdateJoinCode;
    }

    void HandleUpdateJoinCode(string code)
    {
        GetComponent<TextMeshProUGUI>().text = code;
    }
}
