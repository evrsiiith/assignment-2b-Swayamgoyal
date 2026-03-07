using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the potion bottle behavior
/// States: Empty, Filled
/// </summary>
public class PotionBottleBehavior : MonoBehaviour
{
    public enum BottleState
    {
        Empty,
        Filled
    }

    private BottleState currentState = BottleState.Empty;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Renderer objectRenderer;
    private bool isAnimating = false;

    [Header("Bottle Colors")]
    public Color emptyColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);  // Clear/white
    public Color filledColor = new Color(0.5f, 0.1f, 0.7f);        // Purple (matches mixed potion)

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

        Debug.Log("[PotionBottle] Initialized - State: Empty");
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
            Debug.Log("[PotionBottle] Already filled! Press R to reset.");
        }
        else if (isAnimating)
        {
            Debug.Log("[PotionBottle] Currently filling, please wait...");
        }
    }

    /// <summary>
    /// Attempts to fill the bottle (only works if cauldron is mixed)
    /// </summary>
    private void AttemptFill()
    {
        CauldronBehavior cauldron = GameManager.Instance.cauldron.GetComponent<CauldronBehavior>();

        if (cauldron != null && cauldron.GetState() == CauldronBehavior.CauldronState.Mixed)
        {
            Debug.Log("[PotionBottle] Clicked! Filling bottle with magical potion...");
            StartCoroutine(FillAnimation());
        }
        else
        {
            Debug.Log("[PotionBottle] Cannot fill yet - cauldron must be stirred first!");
        }
    }

    /// <summary>
    /// Animates the bottle filling process
    /// </summary>
    private IEnumerator FillAnimation()
    {
        isAnimating = true;

        GameObject cauldron = GameManager.Instance.cauldron;
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

            Debug.Log("[PotionBottle] Bottling the magical potion...");

            // Notify cauldron
            CauldronBehavior cauldronScript = cauldron.GetComponent<CauldronBehavior>();
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

            Debug.Log("[PotionBottle] ★ Potion successfully bottled! ★");
            Debug.Log("[INFO] Press R to reset and brew another potion.");
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

        if (objectRenderer != null)
        {
            objectRenderer.material.color = emptyColor;
        }

        Debug.Log("[PotionBottle] Reset to empty state");
    }

    public BottleState GetState() => currentState;
}
