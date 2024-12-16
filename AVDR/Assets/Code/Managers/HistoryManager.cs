using System;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    public Transform historyUiEntryParent;
    public GameObject historyUiEntryPrefab;
    public int maxHistoryUiEntries = 99;

    [SerializeField] private List<HistoryEntry> historyEntries = new List<HistoryEntry>();
    [SerializeField] private List<int> d4Rolls = new List<int>();
    [SerializeField] private List<int> d6Rolls = new List<int>();
    [SerializeField] private List<int> d8Rolls = new List<int>();
    [SerializeField] private List<int> d10Rolls = new List<int>();
    [SerializeField] private List<int> d12Rolls = new List<int>();
    [SerializeField] private List<int> d20Rolls = new List<int>();
    [SerializeField] private List<int> d100Rolls = new List<int>();

    /// <summary>
    /// Records the outcome of a single die roll.
    /// </summary>
    /// <param name="dieSize"></param>
    /// <param name="outcome"></param>
    public void RecordStatEntry(DieSize dieSize, int outcome) {
        switch(dieSize) {
            case DieSize.d4:
                d4Rolls.Add(outcome);
                break;
            case DieSize.d6:
                d6Rolls.Add(outcome);
                break;
            case DieSize.d8:
                d8Rolls.Add(outcome);
                break;
            case DieSize.d10:
                d10Rolls.Add(outcome);
                break;
            case DieSize.d12:
                d12Rolls.Add(outcome);
                break;
            case DieSize.d20:
                d20Rolls.Add(outcome);
                break;
            case DieSize.d100:
                d100Rolls.Add(outcome);
                break;
            default:
                Debug.LogError("fell through record stat entry switch.");
                break;
        }
        Debug.Log(dieSize + " average: " + GetAverage(dieSize));
    }

    /// <summary>
    /// Gets the average roll of a specific die size.
    /// </summary>
    /// <param name="dieSize"></param>
    /// <returns></returns>
    public double GetAverage(DieSize dieSize) {
        double sum = 0;
        List<int> rolls = new List<int>();
        switch(dieSize) {
            case DieSize.d4:
                rolls = d4Rolls;
                break;
            case DieSize.d6:
                rolls = d6Rolls;
                break;
            case DieSize.d8:
                rolls = d8Rolls;
                break;
            case DieSize.d10:
                rolls = d10Rolls;
                break;
            case DieSize.d12:
                rolls = d12Rolls;
                break;
            case DieSize.d20:
                rolls = d20Rolls;
                break;
            case DieSize.d100:
                rolls = d100Rolls;
                break;
            default:
                Debug.LogError("fell through get average switch.");
                break;
        }
        foreach(int roll in rolls) {
            sum += roll;
        }
        double output = sum / rolls.Count;
        return output;
    }

    public void RecordHistoryEntry(DicePool dicePool, string outcomes, int total, DateTime time) {
        HistoryEntry historyEntry = new HistoryEntry(dicePool, outcomes, total, time);
        historyEntries.Add(historyEntry);
        UpdateUi(historyEntry);
    }

    private void UpdateUi(HistoryEntry historyEntry) {
        if(historyUiEntryParent.childCount > maxHistoryUiEntries) {
            Transform topOfList = historyUiEntryParent.GetChild(0);
            topOfList.gameObject.SetActive(false);
            Destroy(topOfList.gameObject);
        }
        GameObject instance = Instantiate(historyUiEntryPrefab, historyUiEntryParent);
        instance.GetComponent<HistoryUiEntry>().SetupHistoryEntry(historyEntry);
    }
}
