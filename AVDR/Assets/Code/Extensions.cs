using System.Collections.Generic;

/// <summary>
/// A class for helper extension methods. Primarily for debugging.
/// </summary>
public static class ExtensionMethods {

    /// <summary>
    /// Collapses an array or list of ints into a string.
    /// </summary>
    /// <param name="ints"></param>
    /// <param name="title">optional prepend</param>
    /// <param name="sep">optional separator. Defaults to ", ".</param>
    /// <returns></returns>
    public static string IntsToString(this IEnumerable<int> ints, string title = "", string sep = ", ") {
        List<string> strings = new List<string>();
        foreach(int i in ints) {
            strings.Add(i.ToString());
        }
        if(title != "") {
            strings.Insert(0, title);
        }
        return string.Join(sep, strings);
    }

    /// <summary>
    /// prints all the ints in an array to the Unity debug console.
    /// </summary>
    /// <param name="ints"></param>
    /// <param name="title">optional string to begin debug line with</param>
    public static void Debug(this IEnumerable<int> ints, string title = "", string sep = ", ") {
        string str = ints.IntsToString(title, sep);
        UnityEngine.Debug.Log(str);
    }

    public static DieSizeAndType CombineDieSizeAndType (DieSize dieSize, D4Type d4Type) {
    if(dieSize == DieSize.d4) {
        switch(d4Type) {
            case D4Type.Crystal:
                return DieSizeAndType.d4Crystal;
            case D4Type.Pendant:
                return DieSizeAndType.d4Pendant;
            default:
            case D4Type.Caltrop:
                return DieSizeAndType.d4Caltrop;
        }
    }
    else {
        switch(dieSize) {
            case DieSize.d6:
                return DieSizeAndType.d6;
            case DieSize.d8:
                return DieSizeAndType.d8;
            case DieSize.d10:
                return DieSizeAndType.d10;
            case DieSize.d12:
                return DieSizeAndType.d12;
            case DieSize.d20:
                return DieSizeAndType.d20;
            case DieSize.d100:
                return DieSizeAndType.d100;
            default:
                UnityEngine.Debug.LogError("Fell through die size combiner");
                return DieSizeAndType.d6;
        }
    }
}
}