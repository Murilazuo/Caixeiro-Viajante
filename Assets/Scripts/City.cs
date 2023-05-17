using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer spr;

    public Vector2 Position { get => transform.position; }
    private void Start()
    {
        SetScale(CitySizeSeter.size);
        spr.sprite = sprites[Random.Range(0, sprites.Length)];
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
