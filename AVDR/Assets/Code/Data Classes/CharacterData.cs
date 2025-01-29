using System;
using System.Collections.Generic;

/// <summary>
/// A data class that holds the presets and options for a given "character".
/// We store presets and settings in characters so that users can quickly switch between them.
/// </summary>
[Serializable]
public class CharacterData {

    public string characterName;
    public List<DicePool> rollPresets = new List<DicePool>();
    public string diceSet;
    public D4Type d4Type;
    public string trayName;
    public string lightingSet;
    public string effectsSet;
}