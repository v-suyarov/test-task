using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    private float noise = 0.0f;

    public NoiseController()
    {
        noise = 0.0f;
    }
    public float GetNoise()
    {
        return noise;
    }
    public void AddNoise(float addNoise)
    {
        if (noise + addNoise >= 0)
            noise += addNoise;


    }
}

