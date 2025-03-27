using UnityEngine;
using TMPro;

public class MathCannon : MonoBehaviour
{
    [Header("Input Parameters")]
    [Tooltip("Launch angle in degrees (0 - 90)")]
    public float theta = 45f;
    [Tooltip("Launch velocity in meters/second")]
    public float velocity = 10f;

    [Header("Calculated Results")]
    [Tooltip("Maximum height (meters)")]
    public float maxHeight;
    [Tooltip("Horizontal range (meters)")]
    public float horizontalRange;

    [Header("References")]
    [Tooltip("Origin of the graph (position on the X,Y plane)")]
    public Transform graphOrigin;
    [Tooltip("Prefab of the cannonball (must have a Rigidbody)")]
    public GameObject cannonballPrefab;
    [Tooltip("Cannon object that rotates to match theta")]
    public Transform cannon;
    
    [Header("UI References")]
    public TextMeshPro angleText;
    public TextMeshPro velocityText;
    public TextMeshPro maxHeightText;
    public TextMeshPro rangeText;
    
    [Header("Silder References")]
    public MathCannonSlider angleSlider;
    public MathCannonSlider velocitySlider;
    
    [Header("Constants")]
    public float minVelocity = 10f;
    public float maxVelocity = 27.1247111f;
    public float minAngle = 0f;
    public float maxAngle = 90f;
    // Gravity magnitude (using Unity's Physics.gravity)
    private float g = 9.81f;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FireCannon();
        }
        
        theta = Mathf.Lerp(minAngle, maxAngle, angleSlider.GetNormalizedValue());
        velocity = Mathf.Lerp(minVelocity, maxVelocity, velocitySlider.GetNormalizedValue());
        
         // Clamp theta and ensure velocity is non-negative
        theta = Mathf.Clamp(theta, minAngle, maxAngle);
        velocity = Mathf.Clamp(velocity, minVelocity, maxVelocity);

        // Use the magnitude of the physics gravity (in case you change it in project settings)
        g = Mathf.Abs(Physics.gravity.y);
        float thetaRad = theta * Mathf.Deg2Rad;

        // Calculate maximum height: H = (v * sin(theta))^2 / (2 * g)
        maxHeight = (velocity * Mathf.Sin(thetaRad)) * (velocity * Mathf.Sin(thetaRad)) / (2f * g);

        // Calculate horizontal range: R = (v^2 * sin(2 * theta)) / g
        horizontalRange = (velocity * velocity * Mathf.Sin(2f * thetaRad)) / g;
        
        
        angleText.text = "θ = " + theta.ToString("F1") + "°";
        velocityText.text = "v = " + velocity.ToString("F1") + " m/s";
        
        maxHeightText.text = maxHeight.ToString("F1") + " m";
        rangeText.text = horizontalRange.ToString("F1") + " m";


        // Update the cannon object's rotation to match the theta input (rotation around Z-axis)
        if (cannon != null)
        {
            cannon.rotation = Quaternion.Euler(0f, 0f, theta);
        }
        
  
    }

    // Update calculations and cannon rotation in the Inspector when values are changed
    private void OnValidate()
    {
       
    }

    /// <summary>
    /// Fires a cannonball from the graph origin with the initial velocity computed from theta and velocity.
    /// </summary>
    public void FireCannon()
    {
        if (cannonballPrefab == null)
        {
            Debug.LogError("Cannonball prefab is not assigned!");
            return;
        }

        if (graphOrigin == null)
        {
            Debug.LogError("Graph Origin is not assigned!");
            return;
        }

        // Spawn the cannonball at the graph origin position
        GameObject cannonball = Instantiate(cannonballPrefab, cannon.position, Quaternion.identity);

        // Ensure the spawned cannonball has a Rigidbody
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("The cannonball prefab must have a Rigidbody component!");
            return;
        }

        // Compute initial velocity in the X-Y plane (Z component remains 0)
        float thetaRad = theta * Mathf.Deg2Rad;
        Vector3 initialVelocity = new Vector3(velocity * Mathf.Cos(thetaRad), velocity * Mathf.Sin(thetaRad), 0f);
        rb.velocity = initialVelocity;
    }
}