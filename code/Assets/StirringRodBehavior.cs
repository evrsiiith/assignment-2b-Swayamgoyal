using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the stirring rod behavior
/// States: Ready, Stirring
/// </summary>
public class StirringRodBehavior : MonoBehaviour
{
    public enum RodState
    {
        Ready,
        Stirring
    }

    private RodState currentState = RodState.Ready;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isAnimating = false;

    [Header("Animation Settings")]
    public float stirringDuration = 2f; // Time to stir in seconds

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        Debug.Log("[StirringRod] Initialized - State: Ready");
    }

    private void OnMouseDown()
    {
        // Only respond when ready and not animating
        if (currentState == RodState.Ready && !isAnimating)
        {
            AttemptStir();
        }
        else if (isAnimating)
        {
            Debug.Log("[StirringRod] Currently stirring, please wait...");
        }
    }

    /// <summary>
    /// Attempts to stir the cauldron (only works if both ingredients added)
    /// </summary>
    private void AttemptStir()
    {
        // Get cauldron from either V4 or V5 GameManager
        GameObject cauldronObj = null;
        
        if (GameManagerV5.Instance != null)
        {
            cauldronObj = GameManagerV5.Instance.cauldron;
        }
        else if (GameManager.Instance != null)
        {
            cauldronObj = GameManager.Instance.cauldron;
        }
        else
        {
            Debug.LogError("[StirringRod] No GameManager or GameManagerV5 found in scene!");
            return;
        }

        if (cauldronObj == null)
        {
            Debug.LogError("[StirringRod] Cauldron reference is null! Make sure GameManager has the Cauldron assigned.");
            return;
        }

        // Try V4 cauldron first
        CauldronBehavior cauldron = cauldronObj.GetComponent<CauldronBehavior>();
        if (cauldron != null && cauldron.GetState() == CauldronBehavior.CauldronState.HasBoth)
        {
            Debug.Log("[StirringRod] Clicked! Starting to stir the cauldron...");
            StartCoroutine(StirAnimation());
            return;
        }

        // Try V5 cauldron
        CauldronBehaviorV5 cauldronV5 = cauldronObj.GetComponent<CauldronBehaviorV5>();
        if (cauldronV5 != null && cauldronV5.CanStir())
        {
            Debug.Log("[StirringRod] Clicked! Starting to stir the cauldron...");
            StartCoroutine(StirAnimation());
            return;
        }

        Debug.Log("[StirringRod] Cannot stir yet - need 2 ingredients in cauldron!");
    }

    /// <summary>
    /// Animates the stirring process
    /// </summary>
    private IEnumerator StirAnimation()
    {
        isAnimating = true;
        currentState = RodState.Stirring;

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
            // Move rod into cauldron
            transform.position = cauldron.transform.position + new Vector3(0.1f, 0.6f, 0);
            transform.rotation = Quaternion.Euler(15, 0, 0);

            Debug.Log("[StirringRod] Stirring... Creating magical mixture!");

            // Notify cauldron to update its state (try both versions)
            CauldronBehavior cauldronScript = cauldron.GetComponent<CauldronBehavior>();
            if (cauldronScript != null)
            {
                cauldronScript.OnStirred();
            }

            CauldronBehaviorV5 cauldronV5Script = cauldron.GetComponent<CauldronBehaviorV5>();
            if (cauldronV5Script != null)
            {
                cauldronV5Script.OnStirred();
            }

            // Play visual effect
            if (VisualEffectsManager.Instance != null)
            {
                VisualEffectsManager.Instance.PlayStirringEffect(cauldron.transform.position);
            }

            // Wait for stirring duration
            yield return new WaitForSeconds(stirringDuration);

            // Move back to original position
            transform.position = initialPosition;
            transform.rotation = initialRotation;

            currentState = RodState.Ready;
            isAnimating = false;

            Debug.Log("[StirringRod] Stirring complete! Rod returned to original position.");
        }
    }

    /// <summary>
    /// Resets the stirring rod to initial state
    /// </summary>
    public void ResetRod()
    {
        StopAllCoroutines(); // Stop any ongoing animation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentState = RodState.Ready;
        isAnimating = false;
        Debug.Log("[StirringRod] Reset to ready state");
    }

    public RodState GetState() => currentState;
}
