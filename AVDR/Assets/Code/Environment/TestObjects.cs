using UnityEngine;

/// <summary>
/// Removes dummy objects left in scene for the purpose of doing visual development
/// </summary>
public class TestObjects : MonoBehaviour
{
    public bool destroy = true;
    void Awake() {
        if(destroy || !Application.isEditor) {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }
        foreach(Transform child in transform) {
            SingleDie sd = child.GetComponent<SingleDie>();
            if(sd != null) {
                sd.enabled = false;
                Destroy(sd);
            }
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if(rb != null) {
                Destroy(rb);
            }
            MeshCollider mc = child.GetComponent<MeshCollider>();
            if(mc != null) {
                Destroy(mc);
            }
            AudioSource audio = child.GetComponent<AudioSource>();
            if(audio != null) {
                Destroy(audio);
            }
        }
    }
}
