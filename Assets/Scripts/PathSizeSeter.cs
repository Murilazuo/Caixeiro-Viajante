using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSizeSeter : MonoBehaviour
{
    public static Action<float> OnSetPathSize;
    public static float size = 1;
    public void SetCitySize(float size)
    {
        CitySizeSeter.size = size;
        OnSetPathSize?.Invoke(size);
    }
}
