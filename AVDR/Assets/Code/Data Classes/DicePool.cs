using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

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
    public string nameOverride = "";

    public RollType rollType = RollType.Sum;

    /* Dice numbers */
    public int d4s = 0;
    public int d6s = 0;
    public int d8s = 0;
    public int d10s = 0;
    public int d12s = 0;
    public int d20s = 0;
    public int d100s = 0;

    /* values */
    public int bonus = 0;
    public int penalty = 0;
    public int multiplier = 1;
    public int divisor = 1;
    public int keepHighest = 0;
    public int keepLowest = 0;
    /* Note: all thresholds are target-inclusive, because that's how
        users will expect it to work.*/
    public int aboveThreshold = 0;
    public int belowThreshold = 0;

    /// <summary>
    /// Construct a blank dice roll.
    /// </summary>
    public DicePool() {
        
    }

    /// <summary>
    /// Construct a simple single die roll dice pool with no modifiers or bonuses.
    /// </summary>
    /// <param name="dieSize"></param>
    public DicePool(DieSize dieSize) {
        switch(dieSize) {
            case DieSize.d4:
                d4s  = 1;
                break;
            case DieSize.d6:
                d6s  = 1;
                break;
            case DieSize.d8:
                d8s  = 1;
                break;
            case DieSize.d10:
                d10s  = 1;
                break;
            case DieSize.d12:
                d12s  = 1;
                break;
            case DieSize.d20:
                d20s  = 1;
                break;
            case DieSize.d100:
                d100s  = 1;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Construct a simple DicePool with no modifiers or bonuses.
    /// </summary>
    /// <param name="fours">number of d4s</param>
    /// <param name="sixes">number of d6s</param>
    /// <param name="eights">number of d8s</param>
    /// <param name="tens">number of d10s</param>
    /// <param name="twelves">number of d12s</param>
    /// <param name="twenties">number of d20s</param>
    /// <param name="hundreds">number of d100s</param>
    public DicePool(int fours, int sixes, int eights, int tens, int twelves, int twenties, int hundreds) {
        d4s = fours;
        d6s = sixes;
        d8s = eights;
        d10s = tens;
        d12s = twelves;
        d20s = twenties;
        d100s = hundreds;
    }

    /// <summary>
    /// Compiles the number of dice to a string.
    /// </summary>
    /// <returns></returns>
    private string GetRollText() {
        List<string> strings = new List<string>();
        if(d4s > 0) strings.Add(d4s + "d4");
        if(d6s > 0) strings.Add(d6s + "d6");
        if(d8s > 0) strings.Add(d8s + "d8");
        if(d10s > 0) strings.Add(d10s + "d10");
        if(d12s > 0) strings.Add(d12s + "d12");
        if(d20s > 0) strings.Add(d20s + "d20");
        if(d100s > 0) strings.Add(d100s + "d100");

        return string.Join(" + ", strings);
    }

    /// <summary>
    /// Compiles the roll modifiers to a string to append to the dice.
    /// </summary>
    /// <returns></returns>
    public string GetRollMods() {
        List<string> output = new List<string>();
        if(bonus > 0) output.Add("+" + bonus);
        if(penalty > 0) output.Add("-" + penalty);
        if(multiplier > 1) output.Add("×" + multiplier);
        if(divisor > 1) output.Add("÷" + divisor);
        if(keepHighest > 0) output.Add("kh" + keepHighest);
        if(keepLowest > 0) output.Add("kl" + keepLowest);
        if(aboveThreshold > 0) output.Add("<=" + aboveThreshold);
        if(belowThreshold > 0) output.Add("<=" + belowThreshold);
        return string.Join(' ', output);
    }


    public string GetName() {
        string rollText = GetRollText() + " " + GetRollMods();
        if(nameOverride != "") {
            // UnityEngine.Debug.Log("name Override");
            return nameOverride;
        }
        else if (rollText != "") {
            // UnityEngine.Debug.Log("roll text");
            return rollText;
        }
        else {
            // UnityEngine.Debug.Log("empty roll preset");
            return "Empty Roll Preset";
        }
    }

    private string WrapNoBreak(string strTemp) {
        return "<nobr>" + strTemp + "</nobr>";
    }

    public override string ToString()
    {
        return GetRollText();
    }
}