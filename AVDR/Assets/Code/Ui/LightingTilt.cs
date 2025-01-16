using Unity.Mathematics;
using UnityEngine;

public class LightingTilt : MonoBehaviour
{
    public float speed = 50;
    void Start() {
        Input.gyro.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 target = Input.gyro.attitude.eulerAngles;
        target = new Vector3(target.y * 1, (target.z -90f) * 1, target.x * -1);
        Quaternion targetQuat = Quaternion.Euler(target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuat, speed * Time.deltaTime);
    }
}
