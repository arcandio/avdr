using TMPro;
using UnityEngine;

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
