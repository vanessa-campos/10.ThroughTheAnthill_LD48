using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField] Color[] colors;
    MeshRenderer meshRenderer;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        int r = Random.Range(0, colors.Length);
        meshRenderer.material.color = colors[r];
    }
}
