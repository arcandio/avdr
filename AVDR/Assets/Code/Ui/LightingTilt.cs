using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Dynamically changes the lighting based on device attitude
/// making the dice look sparklier and more alive.
/// </summary>
public class LightingTilt : MonoBehaviour
{
    public float speed = 50;
    public Vector3 multiplier = new Vector3(.01f, .1f, -1f);
    public Vector3 offset = new Vector3(0, 90, 0);
    public Color color = new Color(1, 1, 0, .3f);
    public Vector3 boxSize;
    void Start() {
        Input.gyro.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 target = Input.gyro.attitude.eulerAngles;
        target = new Vector3(target.y * multiplier.x + offset.x, target.z * multiplier.y + offset.y, target.x * multiplier.z + offset.z);
        Quaternion targetQuat = Quaternion.Euler(target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuat, speed * Time.deltaTime);
    }

    void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, boxSize);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.DrawSphere(new Vector3(0, 0 , .5f), .1f);
    }
}
