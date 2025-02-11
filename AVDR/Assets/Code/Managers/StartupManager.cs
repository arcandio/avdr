using UnityEngine;

public class StartupManager : MonoBehaviour
{
    public bool setupOnStart = true;
    public bool devGetsCheatCode = true;
    public bool deleteObjectsOnStart = true;
    public GameObject[] deleteObjects;
    
    private ManagerBehaviour[] managerBehaviours;

    void Awake() {
        Debug.LogWarning("STARTUP MANAGER LIVES");
        managerBehaviours = GetComponentsInChildren<ManagerBehaviour>();
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
        foreach(ManagerBehaviour managerBehaviour in managerBehaviours) {
            Debug.Log(managerBehaviour.gameObject.name);
            managerBehaviour.SetupInAwake();
            /* Begin edge cases */
            if(managerBehaviour is AssetManager assetManager) {
                if(Debug.isDebugBuild && devGetsCheatCode) {
            assetManager.CheatCode();
                }
            }
        }
        if(deleteObjectsOnStart == true) {
            foreach(GameObject gameObject in deleteObjects) {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    void SetupStart() {
        foreach(ManagerBehaviour managerBehaviour in managerBehaviours) {
            managerBehaviour.SetupInStart();
            /* begin edge cases */
        }
    }
}
