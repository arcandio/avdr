
using System;

/// <summary>
/// A struct that holds historical data of a roll.
/// </summary>
[Serializable]
public struct HistoryEntry
{
    public string dicePool;
    public string diceRolls;
    public string total;
    public string date;

    /// <summary>
    /// Create a new history entry using data.
    /// </summary>
    public HistoryEntry(DicePool dicePoolTemp, string outcomesTemp, int totalTemp, DateTime dateTimeTemp) {
        dicePool = dicePoolTemp.ToString();
        diceRolls = outcomesTemp;
        total = totalTemp.ToString();
        date = dateTimeTemp.ToString();
    }
    public override string ToString()
    {
        return date + ": " + dicePool + " -> " + diceRolls + " = " + total;
    }

}
