using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the physical dice on the table.
/// </summary>
public class DiceManager : MonoBehaviour
{
    /* Managers */
    public static DiceManager instance;
    public AssetManager assetManager;
    public CharacterManager characterManager;
    public Transform dicePoolParent;
    public RollOutput rollOutput;

    /* diceprefabs */
    public string defaultDiceName = "Humble Commoner";
    public GameObject d4CaltropPrefab;
    public GameObject d4CrystalPrefab;
    public GameObject d4PendantPrefab;
    public GameObject d6Prefab;
    public GameObject d8Prefab;
    public GameObject d10Prefab;
    public GameObject d12Prefab;
    public GameObject d20Prefab;
    public GameObject d100Prefab;

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

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Duplicate DiceManager instance destroyed.");
            Destroy(gameObject);
        }
    }

    void Start() {
        CreateDice(dicePoolData);
    }

    /// <summary>
    /// Cleans up the scene by removing the current dice.
    /// </summary>
    void ClearInstances() {
        // Debug.Log("clear instances");
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
        if(d4Type == D4Type.Caltrop) InstanceDie(d4CaltropPrefab, dicePoolData.d4s);
        if(d4Type == D4Type.Crystal) InstanceDie(d4CrystalPrefab, dicePoolData.d4s);
        if(d4Type == D4Type.Pendant) InstanceDie(d4PendantPrefab, dicePoolData.d4s);
        InstanceDie(d6Prefab, dicePoolData.d6s);
        InstanceDie(d8Prefab, dicePoolData.d8s);
        InstanceDie(d10Prefab, dicePoolData.d10s);
        InstanceDie(d12Prefab, dicePoolData.d12s);
        InstanceDie(d20Prefab, dicePoolData.d20s);
        // InstanceDie(d100Prefab, dicePoolData.d100s);
        // InstanceDie(d10Prefab, dicePoolData.d100s);
        InstancePairedDice(d10Prefab, d100Prefab, dicePoolData.d100s);
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
        AssetKeyValuePair pair = assetManager.Owned.GetAssetPair(AssetType.DiceSet, diceSet);
        // Debug.Log(pair);
        foreach(GameObject prefab in pair.prefabs) {
            SingleDie singleDie = prefab.GetComponent<SingleDie>();
            // Debug.Log(singleDie);
            if(singleDie == null) {
                Debug.LogError(diceSet + " had a prefab without a SingleDie script instance.");
                return;
            }
            else {
                switch(singleDie.dieSize) {
                    case DieSize.d4:
                        switch(singleDie.d4Type) {
                            case D4Type.Caltrop:
                                d4CaltropPrefab = prefab;
                                break;
                            case D4Type.Crystal:
                                d4CrystalPrefab = prefab;
                                break;
                            case D4Type.Pendant:
                                d4PendantPrefab = prefab;
                                break;
                            default:
                                Debug.LogError("Fell through D4Type switch.");
                                break;
                        }
                        break;
                    case DieSize.d6:
                        d6Prefab = prefab;
                        break;
                    case DieSize.d8:
                        d8Prefab = prefab;
                        break;
                    case DieSize.d10:
                        d10Prefab = prefab;
                        break;
                    case DieSize.d12:
                        d12Prefab = prefab;
                        break;
                    case DieSize.d20:
                        d20Prefab = prefab;
                        break;
                    case DieSize.d100:
                        d100Prefab = prefab;
                        break;
                    default:
                        Debug.LogError("Fell through DieType switch.");
                        break;
                }
            }
        }
    }
}
