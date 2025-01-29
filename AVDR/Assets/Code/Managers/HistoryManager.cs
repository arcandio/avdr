using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages historical statistical data
/// </summary>
public class HistoryManager : MonoBehaviour
{
    public IoSystem ioSystem;
    public Transform historyUiEntryParent;
    public GameObject historyUiEntryPrefab;
    public int maxHistoryUiEntries = 99;

    [SerializeField] private HistorySaveData data = new HistorySaveData();


    void Start() {
        data = ioSystem.LoadHistoricalData();
        PopulateLoadedHistory();
    }

    /// <summary>
    /// Records the outcome of a single die roll.
    /// </summary>
    /// <param name="dieSize"></param>
    /// <param name="outcome"></param>
    public void RecordStatEntry(DieSize dieSize, int outcome) {
        switch(dieSize) {
            case DieSize.d4:
                data.d4Rolls.Add(outcome);
                break;
            case DieSize.d6:
                data.d6Rolls.Add(outcome);
                break;
            case DieSize.d8:
                data.d8Rolls.Add(outcome);
                break;
            case DieSize.d10:
                data.d10Rolls.Add(outcome);
                break;
            case DieSize.d12:
                data.d12Rolls.Add(outcome);
                break;
            case DieSize.d20:
                data.d20Rolls.Add(outcome);
                break;
            case DieSize.d100:
                data.d100Rolls.Add(outcome);
                break;
            default:
                Debug.LogError("fell through record stat entry switch.");
                break;
        }
        // Debug.Log(dieSize + " average: " + GetAverage(dieSize));
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
                rolls = data.d4Rolls;
                break;
            case DieSize.d6:
                rolls = data.d6Rolls;
                break;
            case DieSize.d8:
                rolls = data.d8Rolls;
                break;
            case DieSize.d10:
                rolls = data.d10Rolls;
                break;
            case DieSize.d12:
                rolls = data.d12Rolls;
                break;
            case DieSize.d20:
                rolls = data.d20Rolls;
                break;
            case DieSize.d100:
                rolls = data.d100Rolls;
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

    public void RecordHistoryEntry(DicePool dicePool, string outcomes, string total, DateTime time) {
        HistoryEntry historyEntry = new HistoryEntry(dicePool, outcomes, total, time);
        data.historyEntries.Add(historyEntry);
        UpdateUi(historyEntry);
        ioSystem.SaveHistoricalData(data);
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

    private void PopulateLoadedHistory() {
        foreach(Transform child in historyUiEntryParent) {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        foreach(HistoryEntry historyEntry in data.historyEntries) {
            UpdateUi(historyEntry);
        }
    }
}
