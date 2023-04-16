using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVStatic : MonoBehaviour
{
    private Material _material;
    private MeshRenderer _renderer;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
    }

    private void Update()
    {
        _material.SetTextureOffset(MainTex, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
        _renderer.material = _material;
    }
}
