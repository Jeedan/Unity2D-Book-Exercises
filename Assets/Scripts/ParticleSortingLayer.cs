using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleSortingLayer : MonoBehaviour
{
    void Awake()
    {
        var particleRenderer = (Renderer)GetComponent(typeof(Renderer));
        particleRenderer.sortingLayerName = "GUI";
    }
}
