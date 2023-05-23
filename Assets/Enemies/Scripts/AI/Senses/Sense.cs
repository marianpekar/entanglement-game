using UnityEngine;
using UnityEngine.Events;

public class Sense : MonoBehaviour
{
    public Detectable detectable;
    public float distance;

    protected bool isSensing;

    public bool isDetectionContinuous = true;

    public UnityAction<Detectable> OnDetect;
    public UnityAction<Detectable> OnLost;

#if UNITY_EDITOR
    public Color DebugDrawColor = Color.green;
#endif

    private void Detect(Detectable detectable) 
    {
        isSensing = true;
        OnDetect?.Invoke(detectable);
    }
    private void Lost(Detectable detectable)
    {
        isSensing = false;
        OnLost?.Invoke(detectable); 
    }

    void Update()
    {
        if (isSensing)
        {
            if (!HasDetected(detectable))
            {
                Lost(detectable);
                return;
            }

            if(isDetectionContinuous) 
            {
                Detect(detectable);
            }
        }
        else
        {
            if (!HasDetected(detectable))
                return;

            Detect(detectable);
        }      
    }

    protected virtual bool HasDetected(Detectable detectable) => false;
}