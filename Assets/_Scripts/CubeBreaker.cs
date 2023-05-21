using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBreaker : MonoBehaviour
{
    [SerializeField] private NetworkHealth _health;
    [SerializeField] private GameObject cubes;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _health);
        TryGetComponent(out _meshRenderer);
        TryGetComponent(out boxCollider);
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.GetHealth() <= 0)
        {
            gameObject.SetActive(false);
        }
        else if (_health.GetHealth() <= 50)
        {
            _meshRenderer.enabled = false;
            boxCollider.enabled = true;

            cubes.SetActive(true);
        }

        else
        {
            boxCollider.enabled = true;

            _meshRenderer.enabled = true;
            cubes.SetActive(false);
        }
    }
}