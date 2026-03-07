using UnityEngine;

/// <summary>
/// Central game manager that coordinates the potion brewing workflow
/// Handles scene reset and provides references to all interactive objects
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Object References")]
    public GameObject workbench;
    public GameObject cauldron;
    public GameObject redIngredient;
    public GameObject blueIngredient;
    public GameObject stirringRod;
    public GameObject potionBottle;

    [Header("Initial Positions")]
    private Vector3 redIngredientInitialPos;
    private Vector3 blueIngredientInitialPos;
    private Vector3 stirringRodInitialPos;
    private Vector3 bottleInitialPos;

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
        if (stirringRod != null)
            stirringRodInitialPos = stirringRod.transform.position;
        if (potionBottle != null)
            bottleInitialPos = potionBottle.transform.position;

        Debug.Log("=== VR Potion Brewing Lab ===");
        Debug.Log("Instructions:");
        Debug.Log("1. Click RED or BLUE ingredient to add to cauldron");
        Debug.Log("2. After both ingredients are added, click STIRRING ROD");
        Debug.Log("3. Click POTION BOTTLE to fill it");
        Debug.Log("4. Press R to reset the scene");
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
        Debug.Log("============ RESETTING POTION LAB ============");

        // Reset RedIngredient
        if (redIngredient != null)
        {
            var redScript = redIngredient.GetComponent<IngredientBehavior>();
            if (redScript != null)
            {
                redScript.ResetIngredient();
            }
        }

        // Reset BlueIngredient
        if (blueIngredient != null)
        {
            var blueScript = blueIngredient.GetComponent<IngredientBehavior>();
            if (blueScript != null)
            {
                blueScript.ResetIngredient();
            }
        }

        // Reset Cauldron
        if (cauldron != null)
        {
            var cauldronScript = cauldron.GetComponent<CauldronBehavior>();
            if (cauldronScript != null)
            {
                cauldronScript.ResetCauldron();
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

        // Reset PotionBottle
        if (potionBottle != null)
        {
            var bottleScript = potionBottle.GetComponent<PotionBottleBehavior>();
            if (bottleScript != null)
            {
                bottleScript.ResetBottle();
            }
        }

        // Reset visual effects
        VisualEffectsManager effectsManager = GetComponent<VisualEffectsManager>();
        if (effectsManager != null)
        {
            effectsManager.ResetEffects();
        }

        Debug.Log("Lab reset complete! Ready for new potion brewing.");
    }

    public Vector3 GetRedIngredientInitialPos() => redIngredientInitialPos;
    public Vector3 GetBlueIngredientInitialPos() => blueIngredientInitialPos;
    public Vector3 GetStirringRodInitialPos() => stirringRodInitialPos;
    public Vector3 GetBottleInitialPos() => bottleInitialPos;
}
