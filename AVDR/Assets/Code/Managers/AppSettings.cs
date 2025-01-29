using UnityEngine;

public class AppSettings : MonoBehaviour
{
    public static AppSettings instance;

    public D4Type d4Type = D4Type.Caltrop;
    public float globalVolume = 1;
    public float cameraHeight = 0.3629065f;
    public float tossThreshold = 1.5f;

    public bool autoRollOnLoad = false;

    private bool hasLoadedData = false;

    /// <summary>
    /// Unity-style singleton pattern
    /// </summary>
    void Awake(){
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Destroying Duplicate `AppSettings`");
            Destroy(this);
        }
    }
}
