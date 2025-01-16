using System;

[Serializable]
public enum RollType {
    Sum,
    KeepHighest,
    KeepLowest,
    AboveThresholdSingleDie,
    BelowThresholdSingleDie,
    AboveThresholdAllDice,
    BelowThresholdAllDice
}