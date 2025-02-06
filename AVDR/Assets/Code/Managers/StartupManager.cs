using UnityEngine;

public class StartupManager : MonoBehaviour
{
    public bool setupOnStart = true;
    CharacterManager characterManager;
    DiceManager diceManager;
    HistoryManager historyManager;
    UiPageManager uiPageManager;
    AppSettings appSettings;

    void Awake() {
        if(setupOnStart) {
            SetupAwake();
        }
        else {
            Debug.LogWarning("SetupOnStart was not true, not starting up.");

        }
    }

    void Start()
    {
        if(setupOnStart) {
            SetupStart();
        }
        else {
            Debug.LogWarning("SetupOnStart was not true, not starting up.");
        }
    }

    public void LateStartup() {
        SetupAwake();
        SetupStart();
    }

    void SetupAwake() {
        diceManager.Setup();
        uiPageManager.Setup();
        appSettings.Setup();
    }

    void SetupStart() {
        characterManager.Setup();
        historyManager.Setup();
    }
}
