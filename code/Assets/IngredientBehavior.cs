using UnityEngine;

/// <summary>
/// Handles the behavior of ingredient objects (Red and Blue)
/// Manages states: idle, added
/// </summary>
public class IngredientBehavior : MonoBehaviour
{
    public enum IngredientState
    {
        Idle,
        Added
    }

    public enum IngredientType
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [Header("Ingredient Configuration")]
    public IngredientType ingredientType;
    public Color ingredientColor;

    private IngredientState currentState = IngredientState.Idle;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Renderer objectRenderer;

    private void Start()
    {
        // Store initial values
        initialPosition = transform.position;
        initialScale = transform.localScale;
        objectRenderer = GetComponent<Renderer>();

        // Set ingredient color
        if (objectRenderer != null)
        {
            objectRenderer.material.color = ingredientColor;
        }

        Debug.Log($"[{ingredientType}Ingredient] Initialized at position {initialPosition}");
    }

    private void OnMouseDown()
    {
        // Only respond to clicks when in Idle state
        if (currentState == IngredientState.Idle)
        {
            AddIngredientToCauldron();
        }
        else
        {
            Debug.Log($"[{ingredientType}Ingredient] Already added to cauldron. Cannot click again.");
        }
    }

    /// <summary>
    /// Moves the ingredient into the cauldron
    /// </summary>
    private void AddIngredientToCauldron()
    {
        // Try to get cauldron from either V4 or V5 GameManager
        GameObject cauldron = null;
        
        if (GameManagerV5.Instance != null)
        {
            cauldron = GameManagerV5.Instance.cauldron;
        }
        else if (GameManager.Instance != null)
        {
            cauldron = GameManager.Instance.cauldron;
        }
        else
        {
            Debug.LogError("[IngredientBehavior] No GameManager or GameManagerV5 found in scene! Please add one.");
            return;
        }

        if (cauldron != null)
        {
            Debug.Log($"[{ingredientType}Ingredient] Clicked! Moving to cauldron.");

            // Move ingredient into cauldron with slight offset for visual variety
            float yOffset = (ingredientType == IngredientType.Red) ? 0.2f : 0.25f;
            transform.position = cauldron.transform.position + new Vector3(0, yOffset, 0);

            // Make it smaller to simulate dropping into cauldron
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

            // Update state
            currentState = IngredientState.Added;

            // Keep color bright
            if (objectRenderer != null)
            {
                objectRenderer.material.color = ingredientColor;
            }

            // Notify cauldron (try both V4 and V5 scripts)
            CauldronBehavior cauldronScript = cauldron.GetComponent<CauldronBehavior>();
            if (cauldronScript != null)
            {
                cauldronScript.OnIngredientAdded(ingredientType);
            }
            
            CauldronBehaviorV5 cauldronV5Script = cauldron.GetComponent<CauldronBehaviorV5>();
            if (cauldronV5Script != null)
            {
                cauldronV5Script.OnIngredientAdded(ingredientType);
            }

            // Play visual effect
            if (VisualEffectsManager.Instance != null)
            {
                VisualEffectsManager.Instance.PlayIngredientAddEffect(transform.position, ingredientColor);
            }

            // Stop floating animation if present
            FloatingAnimation floatAnim = GetComponent<FloatingAnimation>();
            if (floatAnim != null)
            {
                floatAnim.StopFloating();
            }

            // Disable glow effect after use
            ObjectGlowEffect glowEffect = GetComponent<ObjectGlowEffect>();
            if (glowEffect != null)
            {
                glowEffect.DisableGlow();
            }
        }
        else
        {
            Debug.LogError("[IngredientBehavior] Cauldron reference is null! Make sure GameManager has the Cauldron assigned.");
        }
    }

    /// <summary>
    /// Resets the ingredient to initial state
    /// </summary>
    public void ResetIngredient()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        transform.localScale = initialScale;
        currentState = IngredientState.Idle;

        if (objectRenderer != null)
        {
            objectRenderer.material.color = ingredientColor;
        }

        // Re-enable effects
        FloatingAnimation floatAnim = GetComponent<FloatingAnimation>();
        if (floatAnim != null)
        {
            floatAnim.ResumeFloating();
        }

        ObjectGlowEffect glowEffect = GetComponent<ObjectGlowEffect>();
        if (glowEffect != null)
        {
            glowEffect.EnableGlow();
        }

        Debug.Log($"[{ingredientType}Ingredient] Reset to idle state");
    }

    public IngredientState GetState() => currentState;
}
