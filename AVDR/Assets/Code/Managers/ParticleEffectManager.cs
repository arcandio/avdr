using UnityEngine;

/// <summary>
/// Stub: stores particle effect set and eventually spawns and destroys particle effects
/// </summary>
public class ParticleEffectManager : MonoBehaviour
{
    public CharacterManager characterManager;
    [SerializeField] AKVPEffect effects;

    public void SetParticleEffectPrefabs(AKVPEffect inputEffects) {
        effects = inputEffects;
    }

    public void ClearParticleEffectPrefabs() {
        effects = null;
    }
}
