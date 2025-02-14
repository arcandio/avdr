using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the rolling and prefab settings of a single die.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshCollider))]
[DisallowMultipleComponent]
public class SingleDie : MonoBehaviour
{
    /// <summary>
    /// The type of die this SingleDie is. This is used by the DiceManager to
    /// put the prefab of this die in the right slot for instantiation.
    /// </summary>
    public DieSize dieSize = DieSize.d4;

    /// <summary>
    /// The type of d4 this is, if it is one. If it's not, this is ignored.
    /// </summary>
    public D4Type d4Type = D4Type.Caltrop;

    public DieSizeAndType dieSizeAndType {
        get {
            return ExtensionMethods.CombineDieSizeAndType(dieSize, d4Type);
        }
    }

    public SingleDie pairedDie = null;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform[] faces;
    [SerializeField] private int rollOutcome;
    private bool hasCheckedRollOutcome = false;

    public bool HasCheckedRollOutcome {
        get => hasCheckedRollOutcome;
        set {
            hasCheckedRollOutcome = value;
        }
    }

    private GlobalAdjustments gai;
    private float lastRollTime;

    [SerializeField] private GameObject trailParticleEffect;

    [SerializeField] private float sqrAngularVelocity;
    [SerializeField] private float sqrLinearVelocity;
    [SerializeField] private float verticalVelocity;
    [SerializeField] private bool isTouchingGround = false;
    [SerializeField] private bool stoppedMoving = false;
    [SerializeField] private bool stoppedFalling = false;
    [SerializeField] private bool stoppedTurning = false;
    [SerializeField] private bool notTooEarly = false;

    public float lastCollisionTime = 0;

    void Start() {
        if(faces.Length == 0) {
            Debug.LogError(name + " is missing faces");
        }
        gai = GlobalAdjustments.instance;
    }

    /// <summary>
    /// Performs die actions when the die is thrown, such as 
    /// resetting the roll status.
    /// </summary>
    public void DoThrow(Vector3 force, Vector3 torque) {
        RollOutput.instance.RegisterDie(this);
        RecenterDie();
        rb.constraints = RigidbodyConstraints.None;
        hasCheckedRollOutcome = false;
        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(torque, ForceMode.Impulse);
        lastRollTime = Time.unscaledTime;
    }

    void OnCollisionEnter(Collision collision) {
        // Debug.Log("OnCollisionEnter");
        PlayBump(collision);
        if(collision.collider.tag == "Floor") {
            isTouchingGround = true;
        }
    }
    void OnCollisionExit(Collision collision) {
        if(collision.collider.tag == "Floor") {
            isTouchingGround = false;
        }
    }

    void FixedUpdate() {
        /* check if the die is grounded. */
        sqrAngularVelocity = rb.angularVelocity.sqrMagnitude;
        verticalVelocity = Mathf.Abs(rb.linearVelocity.y);
        sqrLinearVelocity = rb.linearVelocity.sqrMagnitude;

        stoppedFalling = verticalVelocity < gai.groundedVelocityThreshold;
        stoppedMoving = sqrLinearVelocity < gai.velocityThreshold;
        stoppedTurning = sqrAngularVelocity < gai.angularSleepThreshold;
        notTooEarly = Time.unscaledTime > lastRollTime + gai.minCompletionTime;

        IsGrounded();

        /* This block will check to see if we've somehow made it out of the tray through
        the physics colliders and reset us if we do. */
        if(transform.position.y < 0) {
            // Debug.Log("we broke out!");
            RecenterDie();
        }
    }

    /// <summary>
    /// Stops dice from spinning on the table, and checks if the die is grounded.
    /// </summary>
    void IsGrounded() {
        if(isTouchingGround &&
            stoppedFalling &&
            stoppedMoving &&
            stoppedTurning &&
            notTooEarly
        ) {
            // OnGrounded();
            // Debug.LogWarning("IsGrounded check passed.");
            // Debug.Break();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            if(!hasCheckedRollOutcome) {
                FindRolledFace();
            }
            hasCheckedRollOutcome = true;
        }
    }

    /// <summary>
    /// Gets the roll outcome of the die by sorting the faces by highest on the Y axis.
    /// This will get more complicated with compound dice like d%.
    /// </summary>
    /// <returns>Integer value of the die</returns>
    void FindRolledFace() {
        float height = float.NegativeInfinity;
        Transform highestFace = null;
        foreach(Transform face in faces) {
            if(face.position.y > height) {
                height = face.position.y;
                highestFace = face;
            }
        }
        rollOutcome = int.Parse(highestFace.name);
        // Debug.Log(rollOutcome + " on " + name);
        // Debug.Break();
        RollOutput.instance.ReturnDie(this, rollOutcome);
        ParticleEffectManager.instance.OnFinish(this);
    }

    
    /// <summary>
    /// Plays a sound from `GlobalAdjustments` when the die contacts something.
    /// </summary>
    /// <param name="collision"></param>
    void PlayBump(Collision collision) {
        audioSource.volume = Random.Range(GlobalAdjustments.instance.volumeMin,
            GlobalAdjustments.instance.volumeMax) * collision.relativeVelocity.magnitude;
        audioSource.pitch = Random.Range(GlobalAdjustments.instance.pitchMin,
            GlobalAdjustments.instance.pitchMax);
        audioSource.clip = GlobalAdjustments.instance.diceBumpClips[Random.Range(0,
            GlobalAdjustments.instance.diceBumpClips.Length)];
        audioSource.Play();
        ParticleEffectManager.instance.OnHit(this, collision);
    }

    /// <summary>
    /// Moves the die back to a known location, with jitter.
    /// Primarily used for when the physics engine kicks a die out of the viewable area.
    /// Requires a GameObject tagged `Respawn` in the scene.
    /// </summary>
    public void RecenterDie() {
        // Debug.Log("RespawningDie");
        GameObject respawner = GameObject.FindGameObjectWithTag("Respawn");
        // Debug.Log(respawner);
        transform.position = respawner.transform.position;
        /* the following randomizes spawn locaton a little so rigibodies don't
        respawn on top of each other. */
        transform.position += Random.insideUnitSphere * respawner.transform.localScale.x;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        hasCheckedRollOutcome = false;
    }

    public void AttachTrailEffect(GameObject trailPrefab) {
        if(trailParticleEffect != null) {
            trailParticleEffect.SetActive(false);
            Destroy(trailParticleEffect);
        }
        if(trailPrefab != null) {
            trailParticleEffect = Instantiate(trailPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    public bool IsPaired() {
        return pairedDie != null;
    }

    public override string ToString()
    {
        return gameObject.name + ": " + dieSize;
    }

    /* Equality comparison for RollOuput's dictionary */

    /// <summary>
    /// Returns equality if the other object is a SingleDie and
    /// the same as this one.
    /// </summary>
    public override bool Equals(object other)
    {
        if (other == null) {
            return false;
        }
        if (other is not SingleDie otherSD) {
            return false;
        }
        return Equals(other as SingleDie);
    }

    /// <summary>
    /// Returns quality if the other SingleDie is the same as this one.
    /// Uses Unity's game object instance id.
    /// </summary>
    public bool Equals(SingleDie other) {
        if(other == null) {
            return false;
        }
        return GetInstanceID() == other.GetInstanceID();
    }

    public override int GetHashCode()
    {
        return GetInstanceID();
    }
#if UNITY_EDITOR
    void OnDrawGizmos() {
        if(pairedDie != null) {
            Gizmos.color = new Color(0, 0, 1);
            Gizmos.DrawLine(transform.position, pairedDie.transform.position);
        }
    }
#endif
}
