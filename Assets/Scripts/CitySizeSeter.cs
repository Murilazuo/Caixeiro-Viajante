using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySizeSeter : MonoBehaviour
{
    public static Action<float> OnSetCitySize;
    public static float size = 1;
    public void SetCitySize(float size)
    {
        CitySizeSeter.size = size;
        OnSetCitySize?.Invoke(size);
    }
}
