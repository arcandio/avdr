using System.Linq.Expressions;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the rolling of dice and sends commands to SingleDie
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Thrower : ManagerBehaviour
{
    public static Thrower instance;
    public TextMeshProUGUI tmp;
    public ParticleEffectManager particleEffectManager;
    public AudioClip[] rollClips;
    public float accelerationThreshold = 2;
    public float rerollTime = 1;
    public float randomVectorSize = 1;
    public float throwForceMultiplier = 1;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float torqueMultiplier = 3;

    float squaredAccelerationThreshold;
    float lastRollTime;

    override public void SetupInAwake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Duplicate Thrower instance destroyed.");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        squaredAccelerationThreshold = Mathf.Pow(accelerationThreshold, 2);
    }

    void Update() {
        Vector3 dir = Input.acceleration;
        tmp.text = dir.ToString();
        float squaredMagnitude = dir.sqrMagnitude;
        if(squaredMagnitude >= squaredAccelerationThreshold) {
            AttemptThrow(dir);
        }

    }

    public void AttemptThrow() {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1 * randomVectorSize, randomVectorSize), 
            Random.Range(0, randomVectorSize * 2), 
            Random.Range(-1 * randomVectorSize, randomVectorSize));
        AttemptThrow(randomDirection);
    }

    void AttemptThrow(Vector3 direction) {
        if(lastRollTime + rerollTime <= Time.unscaledTime) {
            DoThrow(direction);
        }
    }

    void DoThrow(Vector3 direction) {
        RollOutput.instance.ResetOutcomePool();
        // Debug.Log("Do Throw: " + direction.ToString() + " : " + direction.sqrMagnitude);
        audioSource.volume = Random.Range(GlobalAdjustments.instance.volumeMin,
            GlobalAdjustments.instance.volumeMax);
        audioSource.pitch = Random.Range(GlobalAdjustments.instance.pitchMin,
            GlobalAdjustments.instance.pitchMax);
        audioSource.clip = rollClips[Random.Range(0,
            rollClips.Length)];
        audioSource.Play();
        lastRollTime = Time.unscaledTime;
        // Debug.Log(dicePool.Length);
        if(DiceManager.instance.DiceInstances.Length == 0) return;
        foreach(SingleDie die in DiceManager.instance.DiceInstances) {
            Vector3 force = direction * throwForceMultiplier;
            Vector3 torque = Random.insideUnitSphere * torqueMultiplier;
            die.DoThrow(force, torque);
        }
    }
}
