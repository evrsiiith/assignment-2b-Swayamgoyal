using UnityEngine;

/// <summary>
/// Manages visual effects for the potion brewing lab
/// Adds particle effects, glow effects, and visual feedback
/// </summary>
public class VisualEffectsManager : MonoBehaviour
{
    public static VisualEffectsManager Instance { get; private set; }

    [Header("Particle System Prefabs")]
    public GameObject ingredientAddEffect;
    public GameObject stirringEffect;
    public GameObject bottlingEffect;
    public GameObject magicalAuraEffect;

    [Header("Light Effects")]
    public Light cauldronLight;
    public Color emptyLightColor = Color.white;
    public Color mixedLightColor = new Color(0.8f, 0.2f, 1f); // Purple glow

    private void Awake()
    {
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
        // Initialize cauldron light if exists
        if (cauldronLight != null)
        {
            cauldronLight.color = emptyLightColor;
            cauldronLight.intensity = 0.5f;
        }
    }

    /// <summary>
    /// Plays effect when ingredient is added
    /// </summary>
    public void PlayIngredientAddEffect(Vector3 position, Color particleColor)
    {
        if (ingredientAddEffect != null)
        {
            GameObject effect = Instantiate(ingredientAddEffect, position, Quaternion.identity);
            var particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var main = particleSystem.main;
                main.startColor = particleColor;
            }
            Destroy(effect, 2f);
        }
    }

    /// <summary>
    /// Plays stirring effect around cauldron
    /// </summary>
    public void PlayStirringEffect(Vector3 position)
    {
        if (stirringEffect != null)
        {
            GameObject effect = Instantiate(stirringEffect, position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        // Change cauldron light color
        if (cauldronLight != null)
        {
            StartCoroutine(AnimateLightColor(mixedLightColor, 2f));
        }
    }

    /// <summary>
    /// Plays bottling effect
    /// </summary>
    public void PlayBottlingEffect(Vector3 position)
    {
        if (bottlingEffect != null)
        {
            GameObject effect = Instantiate(bottlingEffect, position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Fade out cauldron light
        if (cauldronLight != null)
        {
            StartCoroutine(AnimateLightIntensity(0f, 1f));
        }
    }

    /// <summary>
    /// Resets all effects
    /// </summary>
    public void ResetEffects()
    {
        if (cauldronLight != null)
        {
            cauldronLight.color = emptyLightColor;
            cauldronLight.intensity = 0.5f;
        }
    }

    /// <summary>
    /// Smoothly animates light color
    /// </summary>
    private System.Collections.IEnumerator AnimateLightColor(Color targetColor, float duration)
    {
        if (cauldronLight == null) yield break;

        Color startColor = cauldronLight.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cauldronLight.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            cauldronLight.intensity = Mathf.Lerp(0.5f, 2f, elapsed / duration); // Increase intensity
            yield return null;
        }

        cauldronLight.color = targetColor;
    }

    /// <summary>
    /// Smoothly animates light intensity
    /// </summary>
    private System.Collections.IEnumerator AnimateLightIntensity(float targetIntensity, float duration)
    {
        if (cauldronLight == null) yield break;

        float startIntensity = cauldronLight.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cauldronLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            yield return null;
        }

        cauldronLight.intensity = targetIntensity;
    }
}
