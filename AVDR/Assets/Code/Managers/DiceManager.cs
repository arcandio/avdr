using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public Transform dicePoolParent;
    public GameObject d4CaltropPrefab;
    public GameObject d4CrystalPrefab;
    public GameObject d4PendantPrefab;
    public GameObject d6Prefab;
    public GameObject d8Prefab;
    public GameObject d10Prefab;
    public GameObject d12Prefab;
    public GameObject d20Prefab;
    public GameObject d100Prefab;
    public D4Type d4Type = D4Type.Crystal;

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
    /// <param name="inputDp">The roll to generate dice for.</param>
    public void CreateDice(DicePool inputDicePool) {
        ClearInstances();
        // Debug.Log("create dice");
        this.dicePoolData = inputDicePool;
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
        InstanceDie(d100Prefab, dicePoolData.d100s);
        InstanceDie(d10Prefab, dicePoolData.d100s);
        // GetDiceInstances();
        diceInstances = tempInstances.ToArray();
    }

    void InstanceDie(GameObject prefab, int count) {
        for(int i = 0; i < count; i++) {
            GameObject temp = Instantiate(prefab, dicePoolParent);
            // temp.name += UnityEngine.Random.Range(0, 1000000);
            tempInstances.Add(temp.GetComponent<SingleDie>());
        }
    }
}
