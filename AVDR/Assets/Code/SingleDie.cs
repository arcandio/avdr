using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshCollider))]
public class SingleDie : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float angularSleepThreshold = 300;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float volumeMax = 1f;
    [SerializeField] private float volumeMin = 0.9f;
    [SerializeField] private float pitchMax = 1.1f;
    [SerializeField] private float pitchMin = .9f;
    [SerializeField] private float groundedVelocityThreshold = .01f;


    void OnCollisionEnter(Collision collision) {
        // Debug.Log("OnCollisionEnter");
        PlayBump(collision);
    }
    void FixedUpdate() {
        /* this block stops dice from spinning on the table. */
        if(rb.angularVelocity.sqrMagnitude < angularSleepThreshold &&
        Mathf.Abs(rb.linearVelocity.y) < groundedVelocityThreshold) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else {
            /* uncomment below to debug dice spinning on the table. */
            // Debug.Log(rb.angularVelocity.sqrMagnitude);
            // Debug.Log(rb.linearVelocity.y);
        }

        /* This block will check to see if we've somehow made it out of the tray through
        the physics colliders and reset us if we do. */
        if(transform.position.y < 0) {
            // Debug.Log("we broke out!");
            RespawnDie();
        }
    }

    void PlayBump(Collision collision) {
        audioSource.volume = Random.Range(volumeMin, volumeMax) * collision.relativeVelocity.magnitude;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.clip = audioClips[Random.Range(0,audioClips.Length)];
        audioSource.Play();
    }

    /// <summary>
    /// Moves the die back to a known location, with jitter.
    /// Primarily used for when the physics engine kicks a die out of the viewable area.
    /// Requires a GameObject tagged `Respawn` in the scene.
    /// </summary>
    public void RespawnDie() {
        // Debug.Log("RespawningDie");
        GameObject respawner = GameObject.FindGameObjectWithTag("Respawn");
        // Debug.Log(respawner);
        transform.position = respawner.transform.position;
        /* the following randomizes spawn locaton a little so rigibodies don't
        respawn on top of each other. */
        transform.position += Random.insideUnitSphere * .01f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
