using UnityEngine;

/// <summary>
/// Adds a glow/highlight effect to objects when hovering
/// Makes clickable objects more obvious
/// </summary>
[RequireComponent(typeof(Renderer))]
public class ObjectGlowEffect : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = new Color(1f, 1f, 0.5f, 1f); // Yellow glow
    public float glowIntensity = 2f;
    public bool enablePulse = true;
    public float pulseSpeed = 2f;

    private Renderer objectRenderer;
    private Material originalMaterial;
    private Material glowMaterial;
    private Color originalColor;
    private Color originalEmission;
    private bool isHovering = false;
    private bool canInteract = true;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        
        if (objectRenderer != null)
        {
            // Store original material
            originalMaterial = objectRenderer.material;
            originalColor = originalMaterial.color;
            
            // Create a copy for glow effect
            glowMaterial = new Material(originalMaterial);
            
            // Enable emission for glow
            glowMaterial.EnableKeyword("_EMISSION");
        }
    }

    private void Update()
    {
        if (!canInteract) return;

        // Pulse effect when hovering
        if (isHovering && enablePulse)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            Color emissionColor = glowColor * (glowIntensity * (0.5f + pulse * 0.5f));
            glowMaterial.SetColor("_EmissionColor", emissionColor);
        }
    }

    private void OnMouseEnter()
    {
        if (!canInteract) return;

        isHovering = true;
        
        if (objectRenderer != null)
        {
            // Apply glow material
            objectRenderer.material = glowMaterial;
            glowMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);
        }

        // Change cursor (optional)
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        isHovering = false;
        
        if (objectRenderer != null)
        {
            // Restore original material
            objectRenderer.material = originalMaterial;
        }
    }

    /// <summary>
    /// Disable interaction glow (when object is used)
    /// </summary>
    public void DisableGlow()
    {
        canInteract = false;
        isHovering = false;
        if (objectRenderer != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }

    /// <summary>
    /// Enable interaction glow
    /// </summary>
    public void EnableGlow()
    {
        canInteract = true;
    }

    private void OnDestroy()
    {
        // Clean up materials
        if (glowMaterial != null)
        {
            Destroy(glowMaterial);
        }
    }
}
