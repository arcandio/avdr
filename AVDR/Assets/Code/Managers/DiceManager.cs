using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the physical dice on the table.
/// </summary>
public class DiceManager : ManagerBehaviour
{
    /* Managers */
    public static DiceManager instance;
    public AssetManager assetManager;
    public CharacterManager characterManager;
    public Transform dicePoolParent;
    public RollOutput rollOutput;

    /* diceprefabs */
    public string defaultDiceName = "Humble Commoner";
    // public GameObject d4CaltropPrefab;
    // public GameObject d4CrystalPrefab;
    // public GameObject d4PendantPrefab;
    // public GameObject d6Prefab;
    // public GameObject d8Prefab;
    // public GameObject d10Prefab;
    // public GameObject d12Prefab;
    // public GameObject d20Prefab;
    // public GameObject d100Prefab;

    private Dictionary<DieSizeAndType, GameObject> dicePrefabs = new Dictionary<DieSizeAndType, GameObject>();

    /* private variables */

    /// <summary>
    /// The currently "loaded" set of dice being rolled by the app.
    /// Stored in the `DiceManager` until the roll is complete.
    /// This gives us someplace to store modifiers as well, for 
    /// when the roll completes.
    /// </summary>
    [SerializeField] private DicePool dicePoolData;

    [SerializeField] private SingleDie[] diceInstances;
    public SingleDie[] DiceInstances {
        get => diceInstances;
    }
    private List<SingleDie> tempInstances = new List<SingleDie>();

    override public void SetupInAwake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Duplicate DiceManager instance destroyed.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Cleans up the scene by removing the current dice.
    /// </summary>
    void ClearInstances() {
        // Debug.LogWarning("clear instances " + diceInstances.Length);
        foreach(SingleDie die in diceInstances) {
            /* disable the instances so they don't try any last-minute calls
            on the frame they're destroyed. */
            die.enabled = false;
            die.gameObject.SetActive(false);
            Destroy(die.gameObject);
        }
        tempInstances = new List<SingleDie>();
        diceInstances = new SingleDie[0];
    }

    /// <summary>
    /// Clears the scene and creates a new set of dice in the scene.
    /// </summary>
    /// <param name="inputDicePool">The roll to generate dice for.</param>
    public void CreateDice(DicePool inputDicePool) {
        D4Type d4Type = characterManager.GetCurrentD4Type();
        ClearInstances();
        // Debug.Log("create dice");
        dicePoolData = inputDicePool;
        /* create one die for each one in the pool */
        tempInstances = new List<SingleDie>();
        if(d4Type == D4Type.Caltrop) InstanceDie(dicePrefabs[DieSizeAndType.d4Caltrop], dicePoolData.d4s);
        if(d4Type == D4Type.Crystal) InstanceDie(dicePrefabs[DieSizeAndType.d4Crystal], dicePoolData.d4s);
        if(d4Type == D4Type.Pendant) InstanceDie(dicePrefabs[DieSizeAndType.d4Pendant], dicePoolData.d4s);
        InstanceDie(dicePrefabs[DieSizeAndType.d6], dicePoolData.d6s);
        InstanceDie(dicePrefabs[DieSizeAndType.d8], dicePoolData.d8s);
        InstanceDie(dicePrefabs[DieSizeAndType.d10], dicePoolData.d10s);
        InstanceDie(dicePrefabs[DieSizeAndType.d12], dicePoolData.d12s);
        InstanceDie(dicePrefabs[DieSizeAndType.d20], dicePoolData.d20s);
        // InstanceDie(d100Prefab, dicePoolData.d100s);
        // InstanceDie(d10Prefab, dicePoolData.d100s);
        InstancePairedDice(dicePrefabs[DieSizeAndType.d10], dicePrefabs[DieSizeAndType.d100], dicePoolData.d100s);
        // GetDiceInstances();
        diceInstances = tempInstances.ToArray();
        rollOutput.SetDicePool(dicePoolData);
    }

    /// <summary>
    /// Creates a number of instances of a die.
    /// </summary>
    /// <param name="prefab">Which object to instantiate.</param>
    /// <param name="count">How many to make.</param>
    void InstanceDie(GameObject prefab, int count) {
        for(int i = 0; i < count; i++) {
            GameObject temp = Instantiate(prefab, dicePoolParent);
            // temp.name += UnityEngine.Random.Range(0, 1000000);
            SingleDie singleDie = temp.GetComponent<SingleDie>();
            tempInstances.Add(singleDie);
            /* attach particle effects */
            ParticleEffectManager.instance.AttachTrail(singleDie);
        }
    }
    void InstancePairedDice(GameObject prefabA, GameObject prefabB, int count) {
        for(int i = 0; i < count; i++) {
            GameObject tempA = Instantiate(prefabA, dicePoolParent);
            GameObject tempB = Instantiate(prefabB, dicePoolParent);
            // temp.name += UnityEngine.Random.Range(0, 1000000);
            SingleDie singleDieA = tempA.GetComponent<SingleDie>();
            SingleDie singleDieB = tempB.GetComponent<SingleDie>();
            singleDieA.pairedDie = singleDieB;
            singleDieB.pairedDie = singleDieA;
            tempInstances.Add(singleDieA);
            tempInstances.Add(singleDieB);
            /* attach particle effects */
            ParticleEffectManager.instance.AttachTrail(singleDieA);
            ParticleEffectManager.instance.AttachTrail(singleDieB);
        }
    }

    /// <summary>
    /// Updates the prefabs in the DiceManager with new ones.
    /// </summary>
    /// <param name="diceSet">The set name to use.</param>
    public void PullDiceFromAssetManager(string diceSet) {
        /* guard clauses */
        if(assetManager.All.Contains(diceSet) == AssetType.None) {
            Debug.LogError("dice set with this name does not exist: " + diceSet);
            return;
        }
        else if(assetManager.Owned.GetDiceSetKeys().Contains(diceSet) == false) {
            Debug.LogError("User does not own " + diceSet);
            return;
        }

        /* Now to place the dice in their proper slots. */
        AKVPDice pair = assetManager.Owned.GetAssetPair(AssetType.DiceSet, diceSet) as AKVPDice;
        // Debug.Log(pair);
        foreach(GameObject prefab in pair.prefabs) {
            SingleDie singleDie = prefab.GetComponent<SingleDie>();
            // Debug.Log(singleDie);
            if(singleDie == null) {
                Debug.LogError(diceSet + " had a prefab without a SingleDie script instance.");
                return;
            }
            else {
                dicePrefabs[singleDie.dieSizeAndType] = prefab;
            }
        }
        ReplaceInSitu();
    }

    /// <summary>
    /// Replaces existing SingleDie instances with matching new ones
    /// </summary>
    private void ReplaceInSitu() {
        /* get ready */
        Debug.Log("Replace In Situ " + diceInstances.Length);
        List<SingleDie> newDice = new List<SingleDie>();
        Dictionary<SingleDie, SingleDie> pairs = new Dictionary<SingleDie, SingleDie>();

        /* clear instances in roll output*/
        rollOutput.ResetOutcomePool();

        /* generate new instances*/
        for(int i = 0; i < diceInstances.Length; i++) {
            SingleDie original = diceInstances[i];
            GameObject prefab = dicePrefabs[original.dieSizeAndType];
            GameObject temp = Instantiate(prefab, original.transform.position, original.transform.rotation, dicePoolParent);
            SingleDie newSingleDie = temp.GetComponent<SingleDie>();
            newDice.Add(newSingleDie);
            ParticleEffectManager.instance.AttachTrail(newSingleDie);

            /* copy motion */
            newSingleDie.HasCheckedRollOutcome = original.HasCheckedRollOutcome;
            Rigidbody source = original.GetComponent<Rigidbody>();
            Rigidbody destination = newSingleDie.GetComponent<Rigidbody>();
            destination.angularVelocity = source.angularVelocity;
            destination.linearVelocity = source.linearVelocity;

            /* re-link paired dice */
            if(original.pairedDie != null) {
                if(pairs.ContainsKey(original)) {
                    /* found a paired set, the other half of which was already  found */
                    newSingleDie.pairedDie = pairs[original];
                    pairs[original].pairedDie = newSingleDie;
                }
                else {
                    /* add the OLD single die from the pair to check against */
                    pairs[original.pairedDie] = newSingleDie;
                }
            }

            /* re-link dice to roll output */
            rollOutput.RegisterDie(newSingleDie);
        }

        /* reset instances list here in the dice manager */
        ClearInstances();
        diceInstances = newDice.ToArray();
        // Debug.Log(newDice.Count);
    }

    public void GetExistingDice() {
        diceInstances = FindObjectsByType<SingleDie>(FindObjectsSortMode.None);
    }
}
