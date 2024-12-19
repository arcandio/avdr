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
}