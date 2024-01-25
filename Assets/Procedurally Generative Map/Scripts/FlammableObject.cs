using UnityEngine;
using System.Collections;

public class FlammableObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    public float burnDuration = 20f;
    public float wetDuration = 10f;

    public bool isFlammable = false;

    public GameObject firePrefab;
    private GameObject fireEffectInstance;
    public float frozenDuration = 5f;

    private bool isWet = false;
    private bool isFrozen = false;
    private bool isBurning = false;
    private string objectTag;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Water":
                if (!isFrozen && !isBurning)
                {
                    GetWet();
                }
                else if (isBurning)
                {
                    CancelBurning();
                }
                break;
            case "Ice":
                GetFrozen();
                if (isBurning)
                {
                    CancelBurning();
                }
                break;
            case "Cold":
                if (isWet)
                {
                    GetFrozen();
                }
                else if (isBurning)
                {
                    CancelBurning();
                }
                break;
            case "Grass":
                if (isWet)
                {
                    isWet = false;
                    spriteRenderer.color = Color.white; // Reset to original color
                }
                break;
            case "Fire":
                if (isFlammable && !isWet && !isFrozen)
                {
                    Burn();
                }
                else if (isFrozen)
                {
                    InstantlyMelt();
                }
                break;
            case "Flame":
                if (isFlammable && !isWet && !isFrozen)
                {
                    Burn();
                }
                else if (isFrozen)
                {
                    InstantlyMelt();
                }
                else if (isWet)
                {
                    isWet = false;
                    spriteRenderer.color = Color.white;
                }
                break;
        }
    }

    void GetWet()
    {
        if (isBurning)
                {
                    CancelBurning();
                }
        if (!isFrozen)
        {
            isWet = true;
            spriteRenderer.color = Color.Lerp(Color.blue, Color.white, 0.5f);
            StartCoroutine(ResetAfterDuration(wetDuration));
        }
    }

    void CancelBurning()
    {
        StopCoroutine("BurningCoroutine");
        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }
        isBurning = false;
        gameObject.tag = objectTag;
    }

    void GetFrozen()
    {
        if (isBurning)
                {
                    CancelBurning();
                }
        isFrozen = true;
        spriteRenderer.color = Color.blue;
        StartCoroutine(MeltIce(frozenDuration));
    }

    void Burn()
    {
        if (isBurning)
        {
            StopCoroutine("BurningCoroutine");
            if (fireEffectInstance != null)
            {
                Destroy(fireEffectInstance);
            }
            isBurning = false;
        }

        isBurning = true;
        objectTag = gameObject.tag;
        gameObject.tag = "Fire";

        // Instantiate the fire prefab at this GameObject's position
        if (firePrefab != null)
        {
            fireEffectInstance = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);
        }

        // Start the coroutine for burning
        StartCoroutine(BurningCoroutine(burnDuration));
    }

    IEnumerator BurningCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        isBurning = false;
        gameObject.tag = objectTag;

        // Destroy the fire effect instance
        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }

        Destroy(gameObject);
    }

    void InstantlyMelt()
    {
        StopCoroutine("MeltIce");
        isFrozen = false;
        spriteRenderer.sprite = originalSprite;
        GetWet();
    }

    IEnumerator ResetAfterDuration(float duration)
    {
        StartCoroutine(FadeToWhite(spriteRenderer, duration));
        yield return new WaitForSeconds(duration);
        isWet = false;
    }

    IEnumerator FadeToWhite(SpriteRenderer spriteRenderer, float duration)
    {
        Color startColor = spriteRenderer.color;
        Color endColor = Color.white;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        spriteRenderer.color = endColor;
    }

    IEnumerator MeltIce(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        spriteRenderer.sprite = originalSprite; // Reset to original sprite
        GetWet(); // Enter wet state
    }
}