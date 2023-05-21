using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponState : MonoBehaviour
{
    public enum Type
    {
        None,
        Sword,
        Axe,
        Hammer,
    }
    public Type Active = Type.Sword;

    public void SetWeapon(Type weapon)
    {
        Active = weapon;
    }

    public void OnOne(InputValue value)
    {
        SetWeapon(Type.Sword);
    }    
    
    public void OnTwo(InputValue value)
    {
        SetWeapon(Type.Axe);
    }
    
    public void OnThree(InputValue value)
    {
        SetWeapon(Type.Hammer);
    }
}
