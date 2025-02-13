using System;
using UnityEngine;

/// <summary>
/// A pair for effects. Holds named variables for each intended effect.
/// </summary>
[Serializable]
public class AKVPEffect : AssetKeyValuePair
{
    public GameObject onHitPrefab;
    public GameObject onFinishPrefab;
    public GameObject trailPrefab;
    public GameObject environmentPrefab;

    /// <summary>
    /// How long in seconds it takes for the onHitPrefab particle effect to
    /// reach its most interesting point.
    /// Used for thumbnails.
    /// </summary>
    public float hitPeakTime = 0;
    /// <summary>
    /// How long in seconds it takes for the onFinishPrefab particle effect to
    /// reach its most interesting point.
    /// Used for thumbnails.
    /// </summary>
    public float finishPeakTime = 0;

    public override string ToString() {
        string output = key;
        output += ": ";
        output += ", " + onHitPrefab.name;
        output += ", " + onFinishPrefab.name;
        output += ", " + trailPrefab.name;
        output += ", " + environmentPrefab.name;
        return output;
    }

    /// <summary>
    /// checks the peak times of the particle effects to see if they're valid.
    /// The peak time cannot be 0 or less, and cannot be greater than the
    /// maximum lifetime of a particle from the system.
    /// </summary>
    /// <returns>boolean: Is the effect system valid.</returns>
    public bool CheckValidity() {
        return CheckDuration(onHitPrefab, hitPeakTime, "hitPeakTime") &&
        CheckDuration(onFinishPrefab, finishPeakTime, "finishPeakTime");
    }

    /// <summary>
    /// Checks a a prefab and associated peak time for validity.
    /// </summary>
    private bool CheckDuration(GameObject prefab, float peakTime, string variableName) {
        if(prefab != null) {
            ParticleSystem onHitPS = prefab.GetComponent<ParticleSystem>();
            float maxDuration = onHitPS.main.duration + onHitPS.main.startLifetime.constantMax;
            if(peakTime == 0) {
                Debug.LogError($"{key} has an invalid {variableName}: 0");
                return false;
            }
            else if(peakTime >= maxDuration) {
                Debug.LogError($"{key} has an invalid {variableName}: longer than particle system max duration. {peakTime} >= {maxDuration}");
                return false;
            }
        }
        return true;
    }
}