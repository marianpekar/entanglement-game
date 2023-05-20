using System.Collections;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    private Vector3 initialScale;
    private readonly float scaleStep = 100f;

    private readonly float lightIntensityStep = 25f;
    private float initialLightIntensity;

    private bool isTeleporting;

    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Light light;

    private void Awake()
    {
        initialScale = transform.localScale;

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        
        light = GetComponent<Light>();
        initialLightIntensity = light.intensity;
    }

    public void Teleport(Vector3 offset)
    {
        if (isTeleporting)
            return;

        isTeleporting = true;

        rigidbody.isKinematic = true;
        collider.enabled = false;

        StartCoroutine(ShrinkAndTransport(offset));
    }

    public IEnumerator ShrinkAndTransport(Vector3 offset)
    {
        while (transform.localScale.x > 0.001f)
        {
            float scale = transform.localScale.x - scaleStep * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            
            if (light != null)
            {
                light.intensity = light.intensity - lightIntensityStep * Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        transform.position = transform.position + offset;
        
        yield return new WaitForEndOfFrame();
        Restore();
        isTeleporting = false;
    }

    public void Restore()
    {
        transform.localScale = initialScale;
        rigidbody.isKinematic = false;
        collider.enabled = true;
        light.intensity = initialLightIntensity;
    }
}
