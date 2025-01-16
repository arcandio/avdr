using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A debug tool for showing the size and placement of the respawn point.
/// </summary>
public class RespawnerGizmo : MonoBehaviour
{
    public Color color = Color.green;

    void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawSphere(Vector3.zero, 1);
        Gizmos.DrawWireSphere(Vector3.zero, 1);
    }
}
