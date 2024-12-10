using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A class to hold behind-the-scenes adjustments to the physics
/// and setup of the app.
/// For user-facing app settings, see `AppSettings`.
/// </summary>
public class GlobalAdjustments : MonoBehaviour
{
    public static GlobalAdjustments instance;
    public float angularSleepThreshold = 300;
    public float groundedVelocityThreshold = .01f;
    public float velocityThreshold = .1f;
    public float minCompletionTime = .1f;
    public AudioClip[] diceBumpClips;
    public float volumeMin = .9f;
    public float volumeMax = 1f;
    public float pitchMin = .9f;
    public float pitchMax = 1.1f;

    /// <summary>
    /// Unity-style singleton pattern
    /// </summary>
    void Awake() {
        if(instance == null){
            instance = this;
        }
        else {
            Debug.LogWarning("Destroying duplicate GlobalAdjustments");
            Destroy(this);
        }
    }
}
