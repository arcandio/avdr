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
        SingleDie pairedDie = null;
        foreach(KeyValuePair<SingleDie, int?> kvp in diceOutcomes) {
            /* check if the die is paired with another, so we can do special math on it. */
            if(kvp.Key.IsPaired()) {
                pairedDie = kvp.Key.pairedDie;
                /* Now we wait for the end of the loop, because we can't modify the dictionary
                    while iterating over it. */
            }
            /* the die is NOT a paired die, so we can just add the value. */
            else {
                /* We need a null coalescing operator here because diceOutcomes' value is nullable. */
                ints.Add(kvp.Value ?? 0);
            }
            /* this die isn't done rolling, so the roll can't be complete yet. */
            if(!kvp.Value.HasValue) {
                allComplete = false;
            }
        }
        /* Now do our modification of paired dice, outside of the loop. */
        if(pairedDie != null) {
            /* we have a paired die and that paired die has a value! */
            if(diceOutcomes[pairedDie] != null) {
                // KeyValuePair<SingleDie, int?> otherKvp = 
                (int singles, int tens) = PercentileRollModulo(SwizzleD100(pairedDie, pairedDie.pairedDie));
                ints.Add(singles);
                ints.Add(tens);
            }
            /* we have a paired die, but that die isn't done yet.
                do not add the die to the list until the other one has finished. */
            else {
                allComplete = false;
            }
        }

        if(allComplete) {
            CompileRollTotal();
        }
        outputs = ints.ToArray();
    }

    /// <summary>
    /// A helper method to arrange two SingleDie objects into an order of [d10, d100]
    /// </summary>
    /// <remarks>
    /// We need this because we can never guarantee what order a d10 and d% land in.
    /// </remarks>
    private (SingleDie, SingleDie) SwizzleD100(SingleDie a, SingleDie b) {
        if(a.dieSize == DieSize.d100) {
            return (b, a);
        }
        else {
            return (a, b);
        }
    }

    /// <summary>
    /// This method converts 1d10 (1-10) + 1d% (10-100) to a pair of values between 1 and 100.
    /// </summary>
    /// <remarks>
    /// When you roll a percentile, (ie 1d10 + 1d% = 1d100), if you get a 00 on the percentile,
    /// there are 4 possible cases for altering the reading of the dice.
    /// <code>
    ///         +-------+--------+
    ///         | 10-90 |  100   |
    ///         | ex=70 | ex=100 |
    /// +-------+-------+--------+
    /// |  1-9  |   -   |  d%=0  |
    /// | ex=7  |   77  |   7    |
    /// +-------+------+---------+
    /// |  10   | d10=0 | d10=0  |
    /// | ex=10 |   70  |  100   |
    /// +-------+------+---------+
    /// 
    /// </code>
    /// Note: this requires our dice to have their "0" and "00" faces numbered as "10" and "100" respectively,
    /// so that they work when rolled separately. When you roll 1d10 and get a 0, you count it as a 10.
    /// </remarks>
    private (int tens, int singles) PercentileRollModulo((SingleDie ones, SingleDie tens) dice) {
        SingleDie onesDie = dice.ones;
        SingleDie tensDie = dice.tens;
        
        /* case 1-9 & 100 */
        if(diceOutcomes[onesDie] != 10 && diceOutcomes[tensDie] == 100) diceOutcomes[tensDie] = 0;

        /* case 10 & 10-90 */
        if(diceOutcomes[onesDie] == 10 && diceOutcomes[tensDie] != 100) diceOutcomes[onesDie] = 0;

        /* case 10 & 100 */
        if(diceOutcomes[onesDie] == 10 && diceOutcomes[tensDie] == 100) diceOutcomes[onesDie] = 0;

        /* case 1-9 & 10-90 */
        return(diceOutcomes[onesDie] ?? 0, diceOutcomes[tensDie] ?? 0);
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
        List<string> rawRollStrings = new List<string>();
        List<int> rawRollInts = new List<int>();
        /* first, collect the values. */
        foreach(KeyValuePair<SingleDie, int?> pair in diceOutcomes) {
            if(pair.Value.HasValue) {
                rawRollStrings.Add(pair.Value.ToString());
                rawRollInts.Add(pair.Value ?? 0);
            }
            else {
                Debug.LogError("We should only ever receive pairs with real int values.");
            }
        }
        int[] rollInts = rawRollInts.ToArray();
        /* determine roll type */
        string output = "";
        int total = 0;
        switch(dicePool.rollType) {
            case RollType.Sum:
                foreach(int i in rawRollInts) {
                    total += i;
                }
                total = ApplyBonuses(total);
                output = total.ToString();
                break;
            case RollType.KeepHighest:
                Array.Sort(rollInts);
                Array.Reverse(rollInts);
                int[] keptHighest = KeepFromGroup(rollInts, dicePool.KeepHighest);
                total = SumCollection(keptHighest);
                total = ApplyBonuses(total);
                output = total.ToString();
                break;
            case RollType.KeepLowest:
                Array.Sort(rollInts);
                int[] keptLowest = KeepFromGroup(rollInts, dicePool.KeepHighest);
                total = SumCollection(keptLowest);
                total = ApplyBonuses(total);
                output = total.ToString();
                break;
            case RollType.AboveThresholdSingleDie:
                foreach(int i in rollInts) {
                    if(i >= dicePool.AboveThreshold) {
                        total++;
                    }
                }
                total = ApplyBonuses(total);
                output = total.ToString();
                break;
            case RollType.BelowThresholdSingleDie:
                foreach(int i in rollInts) {
                    if(i <= dicePool.BelowThreshold) {
                        total++;
                    }
                }
                total = ApplyBonuses(total);
                output = total.ToString();
                break;
            case RollType.AboveThresholdAllDice:
                total = SumCollection(rollInts);
                total = ApplyBonuses(total);
                total = total >= dicePool.AboveThreshold ? 1 : 0;
                output = total.ToString();
                break;
            case RollType.BelowThresholdAllDice:
                total = SumCollection(rollInts);
                total = ApplyBonuses(total);
                total = total <= dicePool.AboveThreshold ? 1 : 0;
                output = total.ToString();
                break;
            default:
                Debug.LogError("Fell through roll type switch. Don't do that." + dicePool.rollType);
                break;
        }
        /* publish the output to the UI and History. */
        outputText.text = output;
        historyManager.RecordHistoryEntry(
            dicePool,
            string.Join(", ", rawRollStrings),
            output,
            DateTime.Now);
        // Debug.Log(output);
    }

    /// <summary>
    /// Sums up the ints in an array.
    /// </summary>
    private int SumCollection(int[] intList) {
        if(intList == null) return 0;
        int output = 0;
        foreach(int i in intList) {
            output += i;
        }
        return output;
    }

    /// <summary>
    /// Sums up the ints in a list.
    /// </summary>
    private int SumCollection(List<int> intList) {
        return SumCollection(intList.ToArray());
    }

    /// <summary>
    /// Returns a new array with only the first N elements.
    /// </summary>
    private int[] KeepFromGroup(int[] ints, int keep) {
        keep = Mathf.Min(ints.Length, keep);
        int[] output = new int[keep];
        Array.Copy(ints, output, keep);
        return output;
    }

    /// <summary>
    /// Returns a new list with only the first N elements.
    /// </summary>
    private int[] KeepFromGroup(List<int> ints, int keep) {
        return KeepFromGroup(ints, keep);
    }

    private int ApplyBonuses(int value) {
        value += dicePool.bonus;
        value -= dicePool.penalty;
        value *= dicePool.multiplier > 0 ? dicePool.multiplier : 1;
        value /= dicePool.divisor > 0 ? dicePool.divisor : 1;
        return value;
    }
}
