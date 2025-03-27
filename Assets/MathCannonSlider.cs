using UnityEngine;

public class MathCannonSlider : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The slider object that moves between bounds")]
    public Transform slider;
    
    [Header("Bounds")]
    [Tooltip("Lower bound transform (represents value 0)")]
    public Transform lowerBound;
    
    [Tooltip("Upper bound transform (represents value 1)")]
    public Transform upperBound;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Current slider value (read-only)")]
    private float currentValue;

    public float GetNormalizedValue()
    {
        if (lowerBound == null || upperBound == null || slider == null)
        {
            Debug.LogError("Slider or bounds not set!");
            return 0f;
        }

        // Get the total distance between bounds
        float totalDistance = Vector3.Distance(lowerBound.position, upperBound.position);
        
        // Get the current distance from lower bound to slider
        float currentDistance = Vector3.Distance(lowerBound.position, slider.position);
        
        // Calculate normalized value (0 to 1)
        currentValue = Mathf.Clamp01(currentDistance / totalDistance);
        
        if(currentValue > 1)
        {
            currentValue = 1;
        }
        else if(currentValue < 0)
        {
            currentValue = 0;
        }
        
        return currentValue;
    }

    private void Update()
    {
        // Update the current value each frame
        GetNormalizedValue();
    }

    private void OnValidate()
    {
        if (lowerBound == null || upperBound == null || slider == null)
        {
            Debug.LogWarning("Please assign the slider and both bounds for proper functionality.");
        }
    }
}