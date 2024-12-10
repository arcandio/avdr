using System;
using Unity;

/// <summary>
/// A struct-like data class for storing the variables of a single dice pool.
/// Includes number and type of dice.
/// Includes bonuses, multipliers, and changes to the rolling methodology.
/// </summary>
/// <remarks>
/// One of these will be used at a time in the app, but we'll want a way to pass
/// them around and serialize them for saving to the device and creating presets.
/// We use a class here instead of a struct so we can update it as the user changes
/// the values of a preset.
/// </remarks>
[Serializable]
public class DicePool {
    public int d4s = 0;
    public int d6s = 0;
    public int d8s = 0;
    public int d10s = 0;
    public int d12s = 0;
    public int d20s = 0;
    public int d100s = 0;

    public int bonus = 0;
    public int penalty = 0;
    public int multiplier = 1;
    public int divisor = 1;

    private int keepHighest = 0;
    /// <summary>
    /// KeepHighest and KeepLowest are mutually exclusive.
    /// Setting either resets the other.
    /// </summary>
    public int KeepHighest {
        get => keepHighest;
        set {
            if(value > 0) {
                keepHighest = value;
                keepLowest = 0;
            }
        }
    }
    private int keepLowest = 0;
    /// <summary>
    /// KeepHighest and KeepLowest are mutually exclusive.
    /// Setting either resets the other.
    /// </summary>
    public int KeepLowest {
        get => keepLowest;
        set {
            if(value > 0) {
                keepLowest = value;
                keepHighest = 0;
            }
        }
    }
}