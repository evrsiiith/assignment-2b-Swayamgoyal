using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enhanced Cauldron Behavior for Version 5
/// Supports 4 ingredients and 6 different potion combinations
/// </summary>
public class CauldronBehaviorV5 : MonoBehaviour
{
    // Ingredient tracking
    private HashSet<IngredientBehavior.IngredientType> addedIngredients = new HashSet<IngredientBehavior.IngredientType>();
    private Renderer objectRenderer;

    [Header("Cauldron Colors")]
    public Color emptyColor = new Color(0.4f, 0.4f, 0.4f);      // Gray
    public Color hasRedColor = new Color(1f, 0.5f, 0.5f);        // Light red
    public Color hasBlueColor = new Color(0.5f, 0.5f, 1f);       // Light blue
    public Color hasGreenColor = new Color(0.5f, 1f, 0.5f);      // Light green
    public Color hasYellowColor = new Color(1f, 1f, 0.5f);       // Light yellow
    
    // Mixed colors (2 ingredients)
    public Color mixedPurpleColor = new Color(0.5f, 0.1f, 0.7f);     // Red + Blue = Purple
    public Color mixedBrownColor = new Color(0.6f, 0.3f, 0.1f);      // Red + Green = Brown
    public Color mixedOrangeColor = new Color(1f, 0.5f, 0f);         // Red + Yellow = Orange
    public Color mixedCyanColor = new Color(0f, 0.8f, 0.8f);         // Blue + Green = Cyan
    public Color mixedLimeColor = new Color(0.7f, 1f, 0.3f);         // Blue + Yellow = Lime
    public Color mixedYellowGreenColor = new Color(0.8f, 1f, 0f);    // Green + Yellow = YellowGreen

    private bool isMixed = false;
    private string currentMixture = "none";

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        UpdateCauldronColor();
        Debug.Log("[CauldronV5] Initialized - Ready for 4-ingredient brewing!");
        Debug.Log("[INFO] Recipe Guide:");
        Debug.Log("  Red + Blue = Purple");
        Debug.Log("  Red + Green = Brown");
        Debug.Log("  Red + Yellow = Orange");
        Debug.Log("  Blue + Green = Cyan");
        Debug.Log("  Blue + Yellow = Lime");
        Debug.Log("  Green + Yellow = Yellow-Green");
    }

    /// <summary>
    /// Called when an ingredient is added
    /// </summary>
    public void OnIngredientAdded(IngredientBehavior.IngredientType ingredientType)
    {
        // Check if cauldron already has 2 ingredients
        if (addedIngredients.Count >= 2)
        {
            Debug.Log($"[CauldronV5] Cannot add {ingredientType} - Cauldron already has 2 ingredients! Stir or reset first.");
            return;
        }

        // Check if this ingredient was already added
        if (addedIngredients.Contains(ingredientType))
        {
            Debug.Log($"[CauldronV5] {ingredientType} already in cauldron!");
            return;
        }

        addedIngredients.Add(ingredientType);
        Debug.Log($"[CauldronV5] ✓ {ingredientType} ingredient added");

        if (addedIngredients.Count == 1)
        {
            Debug.Log($"[INFO] Add 1 more ingredient to create a potion!");
        }
        else if (addedIngredients.Count == 2)
        {
            Debug.Log($"[INFO] ★ 2 ingredients added! Click STIRRING ROD to mix them! ★");
        }

        UpdateCauldronColor();
    }

    /// <summary>
    /// Called when the cauldron is stirred
    /// </summary>
    public void OnStirred()
    {
        if (addedIngredients.Count != 2)
        {
            Debug.Log("[CauldronV5] Cannot stir - need exactly 2 ingredients!");
            return;
        }

        if (isMixed)
        {
            Debug.Log("[CauldronV5] Already mixed! Bottle it or reset.");
            return;
        }

        // Determine the mixture type
        currentMixture = GetMixtureType();
        isMixed = true;

        Debug.Log($"[CauldronV5] ★ {currentMixture} POTION created! ★");
        Debug.Log($"[INFO] Click the correct bottle to store your {currentMixture} potion");

        UpdateCauldronColor();
    }

    /// <summary>
    /// Called when potion is bottled - automatically resets cauldron and respawns ingredients
    /// </summary>
    public void OnBottled()
    {
        if (!isMixed)
        {
            Debug.Log("[CauldronV5] Cannot bottle - potion not mixed yet!");
            return;
        }

        string bottledPotionType = currentMixture;
        Debug.Log($"[CauldronV5] ✓ {bottledPotionType} potion bottled successfully!");
        
        // Auto-reset cauldron
        ResetCauldron();
        
        // Auto-respawn all ingredients
        if (GameManagerV5.Instance != null)
        {
            GameManagerV5.Instance.RespawnAllIngredients();
        }
        
        Debug.Log("[CauldronV5] ★ Auto-reset complete! Ready for next brew! ★");
    }

    /// <summary>
    /// Determines what mixture type based on ingredients
    /// </summary>
    private string GetMixtureType()
    {
        bool hasRed = addedIngredients.Contains(IngredientBehavior.IngredientType.Red);
        bool hasBlue = addedIngredients.Contains(IngredientBehavior.IngredientType.Blue);
        bool hasGreen = addedIngredients.Contains(IngredientBehavior.IngredientType.Green);
        bool hasYellow = addedIngredients.Contains(IngredientBehavior.IngredientType.Yellow);

        if (hasRed && hasBlue) return "Purple";
        if (hasRed && hasGreen) return "Brown";
        if (hasRed && hasYellow) return "Orange";
        if (hasBlue && hasGreen) return "Cyan";
        if (hasBlue && hasYellow) return "Lime";
        if (hasGreen && hasYellow) return "YellowGreen";

        return "Unknown";
    }

    /// <summary>
    /// Updates the cauldron's visual color based on contents
    /// </summary>
    private void UpdateCauldronColor()
    {
        if (objectRenderer == null) return;

        // If mixed, show the final potion color
        if (isMixed)
        {
            switch (currentMixture)
            {
                case "Purple": objectRenderer.material.color = mixedPurpleColor; break;
                case "Brown": objectRenderer.material.color = mixedBrownColor; break;
                case "Orange": objectRenderer.material.color = mixedOrangeColor; break;
                case "Cyan": objectRenderer.material.color = mixedCyanColor; break;
                case "Lime": objectRenderer.material.color = mixedLimeColor; break;
                case "YellowGreen": objectRenderer.material.color = mixedYellowGreenColor; break;
                default: objectRenderer.material.color = emptyColor; break;
            }
            return;
        }

        // If 1 ingredient, show that color
        if (addedIngredients.Count == 1)
        {
            if (addedIngredients.Contains(IngredientBehavior.IngredientType.Red))
                objectRenderer.material.color = hasRedColor;
            else if (addedIngredients.Contains(IngredientBehavior.IngredientType.Blue))
                objectRenderer.material.color = hasBlueColor;
            else if (addedIngredients.Contains(IngredientBehavior.IngredientType.Green))
                objectRenderer.material.color = hasGreenColor;
            else if (addedIngredients.Contains(IngredientBehavior.IngredientType.Yellow))
                objectRenderer.material.color = hasYellowColor;
        }
        // If 2 ingredients (not mixed), show blend
        else if (addedIngredients.Count == 2)
        {
            bool hasRed = addedIngredients.Contains(IngredientBehavior.IngredientType.Red);
            bool hasBlue = addedIngredients.Contains(IngredientBehavior.IngredientType.Blue);
            bool hasGreen = addedIngredients.Contains(IngredientBehavior.IngredientType.Green);
            bool hasYellow = addedIngredients.Contains(IngredientBehavior.IngredientType.Yellow);

            if (hasRed && hasBlue) objectRenderer.material.color = new Color(0.75f, 0.25f, 0.75f);
            else if (hasRed && hasGreen) objectRenderer.material.color = new Color(0.75f, 0.5f, 0.25f);
            else if (hasRed && hasYellow) objectRenderer.material.color = new Color(1f, 0.75f, 0.25f);
            else if (hasBlue && hasGreen) objectRenderer.material.color = new Color(0.25f, 0.75f, 0.75f);
            else if (hasBlue && hasYellow) objectRenderer.material.color = new Color(0.6f, 0.75f, 0.5f);
            else if (hasGreen && hasYellow) objectRenderer.material.color = new Color(0.75f, 0.9f, 0.25f);
        }
        // Empty
        else
        {
            objectRenderer.material.color = emptyColor;
        }
    }

    /// <summary>
    /// Resets cauldron to empty state
    /// </summary>
    public void ResetCauldron()
    {
        addedIngredients.Clear();
        isMixed = false;
        currentMixture = "none";
        UpdateCauldronColor();
        Debug.Log("[CauldronV5] Reset to empty state - Ready for new brew!");
    }

    public bool IsMixed() => isMixed;
    public string GetCurrentMixture() => currentMixture;
    public int GetIngredientCount() => addedIngredients.Count;
    public bool CanStir() => addedIngredients.Count == 2 && !isMixed;
}
