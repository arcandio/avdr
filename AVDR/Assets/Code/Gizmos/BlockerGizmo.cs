using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A debug tool for showing invisible collision walls in the editor.
/// </summary>
public class BlockerGizmo : MonoBehaviour
{
    public Color color = new Color(1, 1, 0, .5f);
    void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
