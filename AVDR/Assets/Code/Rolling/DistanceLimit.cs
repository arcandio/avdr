using UnityEngine;

public class DistanceLimit : MonoBehaviour
{
    void OnTriggerExit(Collider collider) {
        // Debug.Log("Distance Limit Reached!");
        SingleDie singleDie = collider.GetComponent<SingleDie>();
        if(singleDie == null) {
            Debug.LogError("exiting collider was not a die");
            return;
        }
        singleDie.RespawnDie();
    }
}
