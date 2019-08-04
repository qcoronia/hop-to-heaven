using UnityEngine;

public static class AudioHelper {
    public static float DecibelToMagnitude(float decibel) {
        return Mathf.Sqrt(decibel);
    }

    public static float MagnitudeToDecibel(float magnitude) {
        return Mathf.Pow(magnitude, 2);
    }
}