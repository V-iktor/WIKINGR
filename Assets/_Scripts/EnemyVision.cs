using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private GameObject target = null;
    
    public GameObject GetTarget()
    {
        return target;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3) target = other.gameObject;
    }
}
