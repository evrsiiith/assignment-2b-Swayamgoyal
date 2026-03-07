using UnityEngine;
using System.Collections;

/// <summary>
/// Enhanced potion bottle behavior for Version 5
/// ANY bottle can accept ANY potion color (not pre-assigned)
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PotionBottleBehaviorV5 : MonoBehaviour
{
    public enum BottleState
    {
        Empty,
        Filled
    }

    [Header("Bottle Configuration")]
    private string storedPotionType = "none"; // Dynamically stores the potion type when filled
    private Color filledColor = new Color(0.5f, 0.1f, 0.7f); // Will be set based on cauldron potion

    private BottleState currentState = BottleState.Empty;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Renderer objectRenderer;
    private bool isAnimating = false;

    [Header("Bottle Colors")]
    public Color emptyColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);  // Clear/white

    [Header("Animation Settings")]
    public float fillingDuration = 1.5f; // Time to animate filling in seconds

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        objectRenderer = GetComponent<Renderer>();

        // Set initial color
        if (objectRenderer != null)
        {
            objectRenderer.material.color = emptyColor;
        }

        Debug.Log($"[{gameObject.name}] Initialized - Can accept ANY potion color - State: Empty");
    }

    private void OnMouseDown()
    {
        // Only respond when empty and not animating
        if (currentState == BottleState.Empty && !isAnimating)
        {
            AttemptFill();
        }
        else if (currentState == BottleState.Filled)
        {
            Debug.Log($"[{gameObject.name}] Already filled with {storedPotionType} potion! Press R to reset.");
        }
        else if (isAnimating)
        {
            Debug.Log($"[{gameObject.name}] Currently filling, please wait...");
        }
    }

    /// <summary>
    /// Gets the color for the given potion type
    /// </summary>
    private Color GetColorForPotionType(string potionType)
    {
        switch (potionType)
        {
            case "Purple":
                return new Color(0.5f, 0.1f, 0.7f);
            case "Brown":
                return new Color(0.6f, 0.3f, 0.1f);
            case "Orange":
                return new Color(1f, 0.5f, 0f);
            case "Cyan":
                return new Color(0f, 0.8f, 0.8f);
            case "Lime":
                return new Color(0.7f, 1f, 0.3f);
            case "YellowGreen":
                return new Color(0.8f, 1f, 0f);
            default:
                return new Color(0.5f, 0.5f, 0.5f);
        }
    }

    /// <summary>
    /// Attempts to fill the bottle (any empty bottle can accept any mixed potion)
    /// </summary>
    private void AttemptFill()
    {
        CauldronBehaviorV5 cauldron = GameManagerV5.Instance.cauldron.GetComponent<CauldronBehaviorV5>();

        if (cauldron != null)
        {
            // Check if potion is mixed
            if (!cauldron.IsMixed())
            {
                Debug.Log($"[{gameObject.name}] Cannot fill yet - cauldron must be stirred first!");
                return;
            }

            // Get the current potion type from cauldron
            storedPotionType = cauldron.GetCurrentMixture();
            filledColor = GetColorForPotionType(storedPotionType);

            // Any empty bottle can accept any potion!
            Debug.Log($"[{gameObject.name}] ✓ Filling with {storedPotionType} potion...");
            StartCoroutine(FillAnimation());
        }
    }

    /// <summary>
    /// Animates the bottle filling process
    /// </summary>
    private IEnumerator FillAnimation()
    {
        isAnimating = true;

        // Get cauldron from either V4 or V5 GameManager
        GameObject cauldron = null;
        
        if (GameManagerV5.Instance != null)
        {
            cauldron = GameManagerV5.Instance.cauldron;
        }
        else if (GameManager.Instance != null)
        {
            cauldron = GameManager.Instance.cauldron;
        }

        if (cauldron != null)
        {
            // Move bottle near cauldron to show filling action
            transform.position = cauldron.transform.position + new Vector3(0.4f, 0.5f, 0);
            transform.rotation = Quaternion.Euler(30, 0, 0); // Tilt for filling

            // Change color to show filled state
            if (objectRenderer != null)
            {
                objectRenderer.material.color = filledColor;
            }

            // Update state
            currentState = BottleState.Filled;

            Debug.Log($"[{gameObject.name}] Bottling the {storedPotionType} potion...");

            // Notify cauldron (triggers auto-reset and ingredient respawn)
            CauldronBehaviorV5 cauldronScript = cauldron.GetComponent<CauldronBehaviorV5>();
            if (cauldronScript != null)
            {
                cauldronScript.OnBottled();
            }

            // Play visual effect
            if (VisualEffectsManager.Instance != null)
            {
                VisualEffectsManager.Instance.PlayBottlingEffect(transform.position);
            }

            // Wait for filling animation duration
            yield return new WaitForSeconds(fillingDuration);

            // Move back to original position
            transform.position = initialPosition;
            transform.rotation = initialRotation;

            isAnimating = false;

            Debug.Log($"[{gameObject.name}] ★ {storedPotionType} potion successfully bottled! ★");
            Debug.Log("[INFO] Ingredients respawned automatically - brew another potion!");
        }
    }

    /// <summary>
    /// Resets the bottle to empty state
    /// </summary>
    public void ResetBottle()
    {
        StopAllCoroutines(); // Stop any ongoing animation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentState = BottleState.Empty;
        isAnimating = false;
        storedPotionType = "none";

        if (objectRenderer != null)
        {
            objectRenderer.material.color = emptyColor;
        }

        Debug.Log($"[{gameObject.name}] Reset to empty state");
    }

    public BottleState GetState() => currentState;
    public string GetStoredPotionType() => storedPotionType;
}
