using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] private Weapon _weapon;
    // Start is called before the first frame update
    public void Damage(int points)
    {
        health -= points;
        if (text) text.text = health.ToString();
        if(health<=0) Die();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        collider.gameObject.TryGetComponent(out _weapon);
        if (_weapon == null) return;
        Debug.Log(_weapon.Damage);
        Damage(_weapon.Damage);
    }

    static void Die()
    {
        Debug.Log("Dead");
    }
}
