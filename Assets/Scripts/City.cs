using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public Vector2 Position { get => transform.position; }
    private void Start()
    {
        SetScale(CitySizeSeter.size);
    }
    void SetScale(float size)
    {
        transform.localScale = size * Vector3.one;
    }
    private void OnEnable()
    {
        CitySizeSeter.OnSetCitySize += SetScale;
    }
    private void OnDisable()
    {
        CitySizeSeter.OnSetCitySize -= SetScale;
    }
}
