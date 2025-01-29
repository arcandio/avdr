using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float speed = 1f;
    public bool doFlicker = true;
    public float flickerPercentage = .5f;
    public bool doJitter = false;
    public float positionJitterSize = 1f;

    new private Light light;
    private float initialIntensity = 1;
    private Vector3 initialPosition;

    void Start() {
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
        initialPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        if(doFlicker) {
            /* get the actual value of the intensity change */
            float flickerMultiplier = flickerPercentage * initialIntensity;
            /* randomish value between 0 .. 1 */
            float randomIntensity = Mathf.PerlinNoise(transform.position.sqrMagnitude, Time.time * speed);
            /* reduce or increase the value */
            randomIntensity *= flickerMultiplier;
            /* make the flicker zero-weighted, so we get positive and negative values */
            randomIntensity -= flickerMultiplier / 2;
            /* add the random flicker to the original intensity */
            randomIntensity += initialIntensity;
            /* ensure positive light intensity */
            randomIntensity = Mathf.Max(randomIntensity, 0);
            /* set the actual value */
            light.intensity = randomIntensity;
        }
        if(doJitter) {
            /* create a random direction to move in */
            Vector3 randomVector = Random.insideUnitSphere;
            /* move the vector over time */
            randomVector *= Mathf.Sin(Time.fixedDeltaTime * speed);
            /* scale the jitter */
            randomVector *= positionJitterSize;
            /* move the object */
            transform.localPosition = initialPosition + randomVector;
        }
    }
}
