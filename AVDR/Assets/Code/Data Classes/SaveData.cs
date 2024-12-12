/// <summary>
/// A wrapper class for our `CharacterData` list, containing most of the user's data.
/// This is necessary because the unity `JsonUtility` class doesn't serialize top-level
/// lists or arrays, so we have to wrap it in an object we can unwrap on loading.
/// </summary>
/// <remarks>
/// arrays and generic lists ARE ok in input for `JsonUtility`, just not as the top-level
/// element. Most of our character data exists in lists or arrays.
/// Honestly, there are a lot of solutions to this problem on the web that are seriously
/// overengineered for dealing with PlayerPrefs, where you'll always want to know exactly
/// what key you stored stuff in. A simple, two-line solution saved about 10 lines of
/// screwing around in the IoSystem from what I originally had, and dozens over a more
/// generic solution, not to mention the added complexity of loading in a library that
/// I'd be tied to forever and would eventually break down anyway.
/// /diatribe
/// </remarks>
public class SaveData {
    public CharacterData[] characterDatas;
}