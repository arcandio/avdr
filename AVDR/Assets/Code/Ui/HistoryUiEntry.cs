using TMPro;
using UnityEngine;

/// <summary>
/// Displays a history entry.
/// </summary>
public class HistoryUiEntry : MonoBehaviour
{
    public TextMeshProUGUI dicePool;
    public TextMeshProUGUI total;
    public TextMeshProUGUI diceRolls;
    public TextMeshProUGUI date;

    [SerializeField] private HistoryEntry historyEntry;

    public void SetupHistoryEntry(HistoryEntry historyEntryTemp) {
        historyEntry = historyEntryTemp;
        dicePool.text = historyEntry.dicePool;
        total.text = historyEntry.total;
        diceRolls.text = historyEntry.diceRolls;
        date.text = historyEntry.date;
    }
}
