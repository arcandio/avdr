using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollOutput : MonoBehaviour
{
    public static RollOutput instance;
    public TextMeshProUGUI outputText;
    public HistoryManager historyManager;

    private Dictionary<SingleDie, int?> diceOutcomes = new Dictionary<SingleDie, int?>();
    [SerializeField] private int[] outputs;
    [SerializeField] private DicePool dicePool;

    /// <summary>
    /// Unity-style singleton pattern
    /// </summary>
    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("destroying duplicate RollOutput");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the expectation for a given `SingleDie` to be rolled.
    /// </summary>
    /// <param name="die">The `SingleDie` to register with the `RollOutput` system.</param>
    public void RegisterDie(SingleDie die) {
        // Debug.Log(die.name + " Registered");
        diceOutcomes.Add(die, null);
    }

    /// <summary>
    /// Receives the roll outcome of a single die.
    /// If all dice are complete in the roll, it will compile the total.
    /// </summary>
    /// <param name="die">The die to check.</param>
    /// <param name="outcome">The value rolled.</param>
    public void ReturnDie(SingleDie die, int outcome) {
        diceOutcomes[die] = outcome;
        List<int> ints = new List<int>();
        bool allComplete = true;
        historyManager.RecordStatEntry(die.dieSize, outcome);
        foreach(KeyValuePair<SingleDie, int?> pair in diceOutcomes) {
            ints.Add(pair.Value ?? int.MinValue);
            if(!pair.Value.HasValue) {
                allComplete = false;
            }
        }
        if(allComplete) {
            CompileRollTotal();
        }
        outputs = ints.ToArray();
    }

    /// <summary>
    /// Resets the outcome pool so that the last roll doesn't affect the next one.
    /// </summary>
    public void ResetOutcomePool() {
        // Debug.Log("reset outcome pool");
        diceOutcomes.Clear();
        outputText.text = "";
    }

    /// <summary>
    /// Remembers the dice pool for history later.
    /// </summary>
    /// <param name="dicePoolTemp"></param>
    public void SetDicePool(DicePool dicePoolTemp) {
        dicePool = dicePoolTemp;
    }

    /// <summary>
    /// Totals up the dice roll and publishes it.
    /// </summary>
    void CompileRollTotal() {
        int total = 0;
        List<string> outcomes = new List<string>();
        foreach(KeyValuePair<SingleDie, int?> pair in diceOutcomes) {
            if(pair.Value.HasValue) {
                total += pair.Value ?? 0;
                outcomes.Add(pair.Value.ToString());
            }
        }
        outputText.text = total.ToString();
        // Debug.Log(total);
        historyManager.RecordHistoryEntry(dicePool, string.Join(", ", outcomes), total, DateTime.Now);
    }
}
