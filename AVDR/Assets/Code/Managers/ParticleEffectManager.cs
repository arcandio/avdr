using UnityEngine;

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
