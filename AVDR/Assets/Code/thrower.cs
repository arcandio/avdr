using System.Linq.Expressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class thrower : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public AudioClip throwSound;
    public float accelerationThreshold = 2;
    public float rerollTime = 1;
    public float randomVectorSize = 1;
    public float throwForceMultiplier = 1;

    float squaredAccelerationThreshold;
    AudioSource audioSource;
    SingleDie[] dicePool;
    float lastRollTime;

    void Start()
    {
        squaredAccelerationThreshold = Mathf.Pow(accelerationThreshold, 2);
        audioSource = GetComponent<AudioSource>();
        GetDice();
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
            Random.Range(-1 * randomVectorSize, randomVectorSize), 
            Random.Range(-1 * randomVectorSize, randomVectorSize));
        AttemptThrow(randomDirection);
    }

    void AttemptThrow(Vector3 direction) {
        if(lastRollTime + rerollTime <= Time.unscaledTime) {
            DoThrow(direction);
        }
    }

    void DoThrow(Vector3 direction) {
        Debug.Log("Do Throw: " + direction.ToString() + " : " + direction.sqrMagnitude);
        audioSource.clip = throwSound;
        audioSource.Play();
        lastRollTime = Time.unscaledTime;
        Debug.Log(dicePool.Length);
        if(dicePool.Length == 0) return;
        foreach(SingleDie die in dicePool) {
            Rigidbody rigidbody = die.GetComponent<Rigidbody>();
            rigidbody.AddForce(direction * throwForceMultiplier, ForceMode.Impulse);
        }
    }

    void GetDice() {
        dicePool = GameObject.FindObjectsByType<SingleDie>(FindObjectsSortMode.None);
        Debug.Log(dicePool.Length);
    }
}
