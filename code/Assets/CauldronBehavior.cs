using UnityEngine;

/// <summary>
/// Manages the cauldron state and color changes
/// States: Empty, HasRed, HasBlue, HasBoth, Mixed, Used
/// </summary>
public class CauldronBehavior : MonoBehaviour
{
    public enum CauldronState
    {
        Empty,
        HasRed,
        HasBlue,
        HasBoth,
        Mixed,
        Used
    }

    private CauldronState currentState = CauldronState.Empty;
    private Renderer objectRenderer;

    [Header("Cauldron Colors")]
    public Color emptyColor = new Color(0.4f, 0.4f, 0.4f);      // Gray
    public Color hasRedColor = new Color(1f, 0.5f, 0.5f);        // Light red
    public Color hasBlueColor = new Color(0.5f, 0.5f, 1f);       // Light blue
    public Color hasBothColor = new Color(0.8f, 0.4f, 0.8f);     // Purple
    public Color mixedColor = new Color(0.5f, 0.1f, 0.7f);       // Dark purple (magical)

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        UpdateCauldronColor();
        Debug.Log("[Cauldron] Initialized - State: Empty");
    }

    /// <summary>
    /// Called when an ingredient is added
    /// </summary>
    public void OnIngredientAdded(IngredientBehavior.IngredientType ingredientType)
    {
        switch (currentState)
        {
            case CauldronState.Empty:
                if (ingredientType == IngredientBehavior.IngredientType.Red)
                {
                    currentState = CauldronState.HasRed;
                    Debug.Log("[Cauldron] ✓ Now has Red ingredient");
                    Debug.Log("[INFO] Add the BLUE ingredient next");
                }
                else if (ingredientType == IngredientBehavior.IngredientType.Blue)
                {
                    currentState = CauldronState.HasBlue;
                    Debug.Log("[Cauldron] ✓ Now has Blue ingredient");
                    Debug.Log("[INFO] Add the RED ingredient next");
                }
                break;

            case CauldronState.HasRed:
                if (ingredientType == IngredientBehavior.IngredientType.Blue)
                {
                    currentState = CauldronState.HasBoth;
                    Debug.Log("[Cauldron] ✓✓ Now has BOTH ingredients!");
                    Debug.Log("[INFO] ★ Now you can click the STIRRING ROD to mix them! ★");
                }
                break;

            case CauldronState.HasBlue:
                if (ingredientType == IngredientBehavior.IngredientType.Red)
                {
                    currentState = CauldronState.HasBoth;
                    Debug.Log("[Cauldron] ✓✓ Now has BOTH ingredients!");
                    Debug.Log("[INFO] ★ Now you can click the STIRRING ROD to mix them! ★");
                }
                break;

            default:
                Debug.Log("[Cauldron] Cannot add more ingredients in current state");
                break;
        }

        UpdateCauldronColor();
    }

    /// <summary>
    /// Called when the cauldron is stirred
    /// </summary>
    public void OnStirred()
    {
        if (currentState == CauldronState.HasBoth)
        {
            currentState = CauldronState.Mixed;
            Debug.Log("[Cauldron] ★ Potion mixed successfully! ★");
            Debug.Log("[INFO] Now click the POTION BOTTLE to fill it");
            UpdateCauldronColor();
        }
        else
        {
            Debug.Log("[Cauldron] Cannot stir - both ingredients not added yet!");
        }
    }

    /// <summary>
    /// Called when potion is bottled
    /// </summary>
    public void OnBottled()
    {
        if (currentState == CauldronState.Mixed)
        {
            currentState = CauldronState.Used;
            Debug.Log("[Cauldron] Potion bottled. Cauldron is now empty.");
            Debug.Log("[INFO] Press R to reset and brew another potion");
            UpdateCauldronColor();
        }
        else
        {
            Debug.Log("[Cauldron] Cannot bottle - potion not mixed yet!");
        }
    }

    /// <summary>
    /// Updates the cauldron's visual color based on state
    /// </summary>
    private void UpdateCauldronColor()
    {
        if (objectRenderer == null) return;

        switch (currentState)
        {
            case CauldronState.Empty:
            case CauldronState.Used:
                objectRenderer.material.color = emptyColor;
                break;
            case CauldronState.HasRed:
                objectRenderer.material.color = hasRedColor;
                break;
            case CauldronState.HasBlue:
                objectRenderer.material.color = hasBlueColor;
                break;
            case CauldronState.HasBoth:
                objectRenderer.material.color = hasBothColor;
                break;
            case CauldronState.Mixed:
                objectRenderer.material.color = mixedColor;
                break;
        }
    }

    /// <summary>
    /// Resets cauldron to empty state
    /// </summary>
    public void ResetCauldron()
    {
        currentState = CauldronState.Empty;
        UpdateCauldronColor();
        Debug.Log("[Cauldron] Reset to empty state");
    }

    public CauldronState GetState() => currentState;
}
