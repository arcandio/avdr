using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Spawns and destroys particle effects.
/// </summary>
public class ParticleEffectManager : ManagerBehaviour
{
    public CharacterManager characterManager;
    public float minBounceSpeed = 1000;
    public float minDelay = .1f;
    [SerializeField] AKVPEffect effects;
    private List<GameObject> particleSystems = new List<GameObject>();
    
    public static ParticleEffectManager instance;
    
    override public void SetupInAwake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SetParticleEffectPrefabs(AKVPEffect inputEffects) {
        effects = inputEffects;
        ClearParticleEffects();
        AttachEnvironment();
        ResetTrails();
    }

    public void ClearParticleEffectPrefabs() {
        effects = null;
        ClearParticleEffects();
    }

    /// <summary>
    /// Spawns a splash effect where the die collides.
    /// </summary>
    public void OnHit(SingleDie die, Collision collision) {
        // Debug.Log("OnHit: " + collision.relativeVelocity.sqrMagnitude);
        if(effects == null) {
            return;
        }
        if(Time.time < die.lastCollisionTime + minDelay) {
            return;
        }
        if(collision.relativeVelocity.sqrMagnitude < minBounceSpeed) {
            return;
        }
        if(die == null || effects.onHitPrefab == null) {
            return;
        }
        GameObject instance = Instantiate(effects.onHitPrefab, die.transform.position, Quaternion.identity, transform);
        particleSystems.Add(instance);
        SetUnLooping(instance);
        die.lastCollisionTime = Time.time;
    }
    
    /// <summary>
    /// Spawns a finishing effect where the die finishes moving.
    /// </summary>
    public void OnFinish(SingleDie die) {
        if(effects == null || die == null || effects.onFinishPrefab == null) {
            return;
        }
        GameObject instance = Instantiate(effects.onFinishPrefab, die.transform.position, Quaternion.identity, transform);
        particleSystems.Add(instance);
        SetUnLooping(instance);
    }

    /// <summary>
    /// Spawns and attaches a trail particle effect to the die.
    /// </summary>
    public void AttachTrail(SingleDie die) {
        if(effects == null) {
            return;
        }
        if(effects.trailPrefab == null) {
            die.AttachTrailEffect(null);
        }
        die.AttachTrailEffect(effects.trailPrefab);
    }

    /// <summary>
    /// Attaches new trail to all existing dice.
    /// </summary>
    private void ResetTrails() {
        SingleDie[] singleDice = FindObjectsByType<SingleDie>(FindObjectsSortMode.None);
        foreach(SingleDie singleDie in singleDice) {
            AttachTrail(singleDie);
        }
    }

    /// <summary>
    /// Spawns an environmental particle effect, like rain or snow.
    /// </summary>
    void AttachEnvironment() {
        if(effects.environmentPrefab == null) {
            return;
        }
        GameObject instance = Instantiate(effects.environmentPrefab, Vector3.zero, Quaternion.identity, transform);
        particleSystems.Add(instance);
    }

    /// <summary>
    /// Destroys all the particle effects that are selectable, to prepare for spawning new ones.
    /// </summary>
    public void ClearParticleEffects() {
        foreach(GameObject particleSystem in particleSystems) {
            if(particleSystem == null) {
                continue;
            }
            particleSystem.SetActive(false);
            Destroy(particleSystem);
        }
    }

    private void SetUnLooping(GameObject gameObject){
        ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particleSystem in particleSystems) {
            var main = particleSystem.main;
            main.loop = false;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
}
