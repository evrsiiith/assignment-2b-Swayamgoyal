using UnityEngine;

/// <summary>
/// Enhanced Game Manager for Version 5
/// Supports 4 ingredients and 6 potion bottles
/// </summary>
public class GameManagerV5 : MonoBehaviour
{
    public static GameManagerV5 Instance { get; private set; }

    [Header("Object References")]
    public GameObject workbench;
    public GameObject cauldron;
    
    [Header("4 Ingredients")]
    public GameObject redIngredient;
    public GameObject blueIngredient;
    public GameObject greenIngredient;
    public GameObject yellowIngredient;
    
    [Header("Stirring Rod")]
    public GameObject stirringRod;
    
    [Header("6 Potion Bottles")]
    public GameObject potionBottle1; // Purple (Red + Blue)
    public GameObject potionBottle2; // Brown (Red + Green)
    public GameObject potionBottle3; // Cyan (Blue + Green)
    public GameObject potionBottle4; // Orange (Red + Yellow)
    public GameObject potionBottle5; // Lime (Blue + Yellow)
    public GameObject potionBottle6; // YellowGreen (Green + Yellow)

    [Header("Initial Positions")]
    private Vector3 redIngredientInitialPos;
    private Vector3 blueIngredientInitialPos;
    private Vector3 greenIngredientInitialPos;
    private Vector3 yellowIngredientInitialPos;
    private Vector3 stirringRodInitialPos;
    private Vector3[] bottleInitialPositions = new Vector3[6];

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Store initial positions for reset
        if (redIngredient != null)
            redIngredientInitialPos = redIngredient.transform.position;
        if (blueIngredient != null)
            blueIngredientInitialPos = blueIngredient.transform.position;
        if (greenIngredient != null)
            greenIngredientInitialPos = greenIngredient.transform.position;
        if (yellowIngredient != null)
            yellowIngredientInitialPos = yellowIngredient.transform.position;
        if (stirringRod != null)
            stirringRodInitialPos = stirringRod.transform.position;

        // Store bottle positions
        if (potionBottle1 != null) bottleInitialPositions[0] = potionBottle1.transform.position;
        if (potionBottle2 != null) bottleInitialPositions[1] = potionBottle2.transform.position;
        if (potionBottle3 != null) bottleInitialPositions[2] = potionBottle3.transform.position;
        if (potionBottle4 != null) bottleInitialPositions[3] = potionBottle4.transform.position;
        if (potionBottle5 != null) bottleInitialPositions[4] = potionBottle5.transform.position;
        if (potionBottle6 != null) bottleInitialPositions[5] = potionBottle6.transform.position;

        Debug.Log("=== VR Potion Brewing Lab - VERSION 5 ===");
        Debug.Log("Instructions:");
        Debug.Log("1. Click any 2 INGREDIENTS to add to cauldron");
        Debug.Log("   - Red, Blue, Green, or Yellow");
        Debug.Log("2. Click STIRRING ROD to mix them");
        Debug.Log("3. Click ANY BOTTLE to store your potion");
        Debug.Log("   - ALL bottles can accept ANY potion color!");
        Debug.Log("   - Ingredients auto-respawn after bottling");
        Debug.Log("   Recipe Guide:");
        Debug.Log("     Red+Blue=Purple | Red+Green=Brown | Red+Yellow=Orange");
        Debug.Log("     Blue+Green=Cyan | Blue+Yellow=Lime | Green+Yellow=YellowGreen");
        Debug.Log("4. Press R to manually reset the scene");
    }

    private void Update()
    {
        // Reset scene with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
    }

    /// <summary>
    /// Resets all objects to their initial state
    /// </summary>
    public void ResetScene()
    {
        Debug.Log("============ RESETTING POTION LAB (V5) ============");

        // Reset all 4 ingredients
        ResetIngredient(redIngredient);
        ResetIngredient(blueIngredient);
        ResetIngredient(greenIngredient);
        ResetIngredient(yellowIngredient);

        // Reset Cauldron (try both versions)
        if (cauldron != null)
        {
            var cauldronScript = cauldron.GetComponent<CauldronBehavior>();
            if (cauldronScript != null)
            {
                cauldronScript.ResetCauldron();
            }

            var cauldronV5Script = cauldron.GetComponent<CauldronBehaviorV5>();
            if (cauldronV5Script != null)
            {
                cauldronV5Script.ResetCauldron();
            }
        }

        // Reset StirringRod
        if (stirringRod != null)
        {
            var rodScript = stirringRod.GetComponent<StirringRodBehavior>();
            if (rodScript != null)
            {
                rodScript.ResetRod();
            }
        }

        // Reset all 6 bottles (try both versions)
        ResetBottle(potionBottle1);
        ResetBottle(potionBottle2);
        ResetBottle(potionBottle3);
        ResetBottle(potionBottle4);
        ResetBottle(potionBottle5);
        ResetBottle(potionBottle6);

        // Reset visual effects
        VisualEffectsManager effectsManager = GetComponent<VisualEffectsManager>();
        if (effectsManager != null)
        {
            effectsManager.ResetEffects();
        }

        Debug.Log("Lab reset complete! Ready for new potion brewing.");
    }

    /// <summary>
    /// Respawns all 4 ingredients after bottling (called automatically by cauldron)
    /// </summary>
    public void RespawnAllIngredients()
    {
        Debug.Log("[GameManagerV5] Respawning all ingredients...");
        
        ResetIngredient(redIngredient);
        ResetIngredient(blueIngredient);
        ResetIngredient(greenIngredient);
        ResetIngredient(yellowIngredient);
        
        Debug.Log("[GameManagerV5] All ingredients respawned and ready!");
    }

    private void ResetIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;

        var ingredientScript = ingredient.GetComponent<IngredientBehavior>();
        if (ingredientScript != null)
        {
            ingredientScript.ResetIngredient();
        }
    }

    private void ResetBottle(GameObject bottle)
    {
        if (bottle == null) return;

        // Try V4 bottle script
        var bottleScript = bottle.GetComponent<PotionBottleBehavior>();
        if (bottleScript != null)
        {
            bottleScript.ResetBottle();
        }

        // Try V5 bottle script
        var bottleV5Script = bottle.GetComponent<PotionBottleBehaviorV5>();
        if (bottleV5Script != null)
        {
            bottleV5Script.ResetBottle();
        }
    }

    // Getter methods for initial positions
    public Vector3 GetRedIngredientInitialPos() => redIngredientInitialPos;
    public Vector3 GetBlueIngredientInitialPos() => blueIngredientInitialPos;
    public Vector3 GetGreenIngredientInitialPos() => greenIngredientInitialPos;
    public Vector3 GetYellowIngredientInitialPos() => yellowIngredientInitialPos;
    public Vector3 GetStirringRodInitialPos() => stirringRodInitialPos;
    public Vector3 GetBottleInitialPos(int index) => bottleInitialPositions[index];
}
