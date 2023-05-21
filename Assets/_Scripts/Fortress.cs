using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : MonoBehaviour
{
    [SerializeField] private List<Transform> blocks;

    void Start()
    {
        foreach (Transform child in transform)
            if (child.GetComponent<NetworkHealth>().GetHealth() >= 0)
                blocks.Add(child);
    }

    public Transform GetClosest(Vector3 position)
    {
        Transform closest = null;
        float minimumDistance = Mathf.Infinity;

        foreach (var t in blocks)
        {
            if (!t.gameObject.activeSelf) continue;
            float dist = Vector3.Distance(t.position, position);
            if (!(dist < minimumDistance)) continue;
            closest = t;
            minimumDistance = dist;
        }

        return closest;
    }
}