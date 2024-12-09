using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject d4CaltropPrefab;
    public GameObject d4CrystalPrefab;
    public GameObject d4PendantPrefab;
    public GameObject d6Prefab;
    public GameObject d8Prefab;
    public GameObject d10Prefab;
    public GameObject d12Prefab;
    public GameObject d20Prefab;
    public GameObject d100Prefab;

    [SerializeField] private DicePool dicePoolData;

    [SerializeField] private SingleDie[] diceInstances;
    private List<SingleDie> tempInstances = new List<SingleDie>();

    void Start() {
        Debug.LogWarning("Start");
        GetDiceInstances();
        ClearInstances();
        // dicePoolData = new DicePool();
        CreateDice(dicePoolData);
    }
    
    void GetDiceInstances() {
        Debug.LogWarning("get dice instances");
        diceInstances = FindObjectsByType<SingleDie>(FindObjectsSortMode.None);
        Thrower.instance.GetDice();
    }

    void ClearInstances() {
        Debug.LogWarning("clear instances");
        foreach(SingleDie die in diceInstances) {
            Destroy(die.gameObject);
        }
    }

    void CreateDice(DicePool dicePool) {
        Debug.LogWarning("create dice");
        this.dicePoolData = dicePool;
        /* create one die for each one in the pool */
        tempInstances = new List<SingleDie>();
        InstanceDie(d4CrystalPrefab, dicePoolData.d4s);
        InstanceDie(d6Prefab, dicePoolData.d6s);
        InstanceDie(d8Prefab, dicePoolData.d8s);
        InstanceDie(d10Prefab, dicePoolData.d10s);
        InstanceDie(d8Prefab, dicePoolData.d12s);
        InstanceDie(d12Prefab, dicePoolData.d20s);
        InstanceDie(d20Prefab, dicePoolData.d100s);
        InstanceDie(d10Prefab, dicePoolData.d100s);
        // GetDiceInstances();
        diceInstances = tempInstances.ToArray();
    }

    void InstanceDie(GameObject prefab, int count) {
        for(int i = 0; i < count; i++) {
            Instantiate(prefab);
            tempInstances.Add(prefab.GetComponent<SingleDie>());
        }
    }
}
