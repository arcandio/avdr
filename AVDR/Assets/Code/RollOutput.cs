using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;

public class RollOutput : MonoBehaviour
{
    public static RollOutput instance;
    public TextMeshProUGUI outputText;

    private Dictionary<SingleDie, int?> diceOutcomes = new Dictionary<SingleDie, int?>();
    [SerializeField] private int[] outputs;

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

    public void RegisterDie(SingleDie die) {
        diceOutcomes.Add(die, null);
    }

    public void ReturnDie(SingleDie die, int output) {
        diceOutcomes[die] = output;
        List<int> ints = new List<int>();
        bool allComplete = true;
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

    public void ClearDicePool() {
        diceOutcomes.Clear();
        outputText.text = "";
    }

    void CompileRollTotal() {
        int total = 0;
        foreach(KeyValuePair<SingleDie, int?> pair in diceOutcomes) {
            if(pair.Value.HasValue) {
                total += pair.Value ?? 0;
            }
        }
        outputText.text = total.ToString();
        Debug.Log(total);
    }
}
