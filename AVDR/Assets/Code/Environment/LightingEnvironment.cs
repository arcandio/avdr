using UnityEngine;

[ExecuteInEditMode]
public class LightingEnvironment : MonoBehaviour
{
    public Material skybox;
    public float lightingMultiplier = 1;
    public float reflectionMultiplier = 1;

    public ReflectionProbe reflectionProbe;

    void OnEnable() {
        UpdateLightingEnvironment();
        DisableOthers();
    }

    void UpdateLightingEnvironment() {
        RenderSettings.skybox = skybox;
        RenderSettings.ambientIntensity = lightingMultiplier;
        RenderSettings.reflectionIntensity = reflectionMultiplier;
        DynamicGI.UpdateEnvironment();
        reflectionProbe.RenderProbe();
    }

    void DisableOthers() {
        LightingEnvironment[] others = FindObjectsByType<LightingEnvironment>(FindObjectsSortMode.None);
        foreach(LightingEnvironment env in others) {
            if(env != this) {
                env.gameObject.SetActive(false);
            }
        }
    }
}
