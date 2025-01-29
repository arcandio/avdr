using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// UI-attached class used on buttons that sends dice pools to the `DiceManager`.
/// </summary>
public class RollLoader : MonoBehaviour
{
    public DicePool dicePool;
    public TextMeshProUGUI buttonText;
    public bool isPresetButton;

    void Start() {
        buttonText.text = dicePool.GetName();
    }

    /// <summary>
    /// Sends a single die to the `DiceManager` and then rolls it.
    /// </summary>
    /// <param name="dieSize"></param>
    public void QuickRoll() {
        DiceManager.instance.CreateDice(dicePool);
        Thrower.instance.AttemptThrow();
    }

    public void LoadDice() {
        DiceManager.instance.CreateDice(dicePool);
    }

    /// <summary>
    /// Sets up a new non-preset button from code.
    /// </summary>
    /// <param name="dicePoolTemp"></param>
    public void SetupRoll(DicePool dicePoolTemp) {
        dicePool = dicePoolTemp;
        isPresetButton = false;
        buttonText.text = dicePool.GetName();
    }
}
