/// <summary>
/// A wrapper class for our `CharacterData` list, containing most of the user's data.
/// This is necessary because the unity `JsonUtility` class doesn't serialize top-level
/// lists or arrays, so we have to wrap it in an object we can unwrap on loading.
/// </summary>
/// <remarks>
/// arrays and generic lists ARE ok in input for `JsonUtility`, just not as the top-level
/// element. Most of our character data exists in lists or arrays.
/// </remarks>
public class SaveData {
    public CharacterData[] characterDatas;
}