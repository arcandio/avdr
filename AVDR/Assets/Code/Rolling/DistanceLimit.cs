using UnityEngine;

/// <summary>
/// Prevents dice from leaving a certain volume.
/// Used as a last-resort backup to keep dice on the table.
/// </summary>
public class DistanceLimit : MonoBehaviour
{
    void OnTriggerExit(Collider collider) {
        // Debug.Log("Distance Limit Reached!");
        SingleDie singleDie = collider.GetComponent<SingleDie>();
        if(singleDie == null) {
            Debug.LogError("exiting collider was not a die");
            return;
        }
        singleDie.RecenterDie();
    }
}
