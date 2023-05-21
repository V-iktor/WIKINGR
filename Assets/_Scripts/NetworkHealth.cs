using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> m_Health = new();
    [SerializeField] private int _initialValue = 100;

    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text debugHealthText;

    [SerializeField] private Weapon _weapon;

    public LayerMask DamageLayers;

    public bool IsAlive()
    {
        return m_Health.Value > 0;
    }
    public int GetHealth()
    {
        return m_Health.Value;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            m_Health.Value = _initialValue;
        }
        else
        {
            UpdateUI(m_Health.Value.ToString());
        }

        if (!_animator) TryGetComponent(out _animator);
        m_Health.OnValueChanged += ClientSyncHealth;
    }

    private void UpdateUI(string value)
    {
        if (debugHealthText) debugHealthText.text = value;
    }

    public override void OnNetworkDespawn()
    {
        m_Health.OnValueChanged -= ClientSyncHealth;
    }

    private void ClientSyncHealth(int previous, int current)
    {
        UpdateUI(current.ToString());
        if (m_Health.Value > 0)
        {
            if (_animator) _animator.SetTrigger("Impact");

            return;
        }

        if (_animator) _animator.SetTrigger("Die");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsServer) return;
        if (!IsAlive()) return;
        collider.gameObject.TryGetComponent(out _weapon);
        if (_weapon == null) return;
        if (!IsInLayerMask(collider.gameObject.layer, DamageLayers)) return;
        m_Health.Value -= _weapon.Damage;
    }

    private static bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }
}