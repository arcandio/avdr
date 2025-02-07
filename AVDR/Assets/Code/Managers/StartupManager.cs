using UnityEngine;

public class StartupManager : MonoBehaviour
{
    public CharacterManager characterManager;
    public DiceManager diceManager;
    public HistoryManager historyManager;
    public UiPageManager uiPageManager;
    public AppSettings appSettings;
    public AssetManager assetManager;
    public RollOutput rollOutput;

    public bool setupOnStart = true;
    public bool devGetsCheatCode = true;

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
        rollOutput.Setup();
        
        if(Debug.isDebugBuild && devGetsCheatCode) {
            assetManager.CheatCode();
        }
    }

    void SetupStart() {
        characterManager.Setup();
        historyManager.Setup();
    }
}
