using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponState _weaponState;

    [SerializeField] private AnimationState _animationState;

    [SerializeField] private WeaponState.Type _weapon;

    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private MeshRenderer _meshRenderer;

    public int Damage = 0;

    void Start()
    {
        TryGetComponent(out _boxCollider);
        TryGetComponent(out _meshRenderer);
    }
    
    void Update()
    {
        bool active = !_weaponState || _weaponState.Active == _weapon;
        if(_meshRenderer) _meshRenderer.enabled = active;
        _boxCollider.enabled = active && _animationState.Current == AnimationState.AnimationStates.Damaging;
    }
}