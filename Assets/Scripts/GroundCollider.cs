using System.Collections;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public Sprite iceSprite;
    public Sprite[] grassSprites;
    public Sprite[] dirtSprites;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    public bool isFlammable = false;
    public bool isRevivable = false;
    public float burnDuration = 10f;
    public float wetDuration = 10f;

    public GameObject firePrefab; // Reference to the fire prefab
    private GameObject fireEffectInstance;
    public float frozenDuration = 5f;

    private bool isWet = false;
    private bool isFrozen = false;
    private bool isBurning = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
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
                else if (isRevivable)
                {
                    GrowGrass();
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
                else if (isRevivable)
                {
                    GrowGrass();
                }
                break;
        }
    }

    void GetWet()
    {
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
        GetWet();
    }

    void GetFrozen()
    {
        isFrozen = true;
        spriteRenderer.sprite = iceSprite;
        StartCoroutine(MeltIce(frozenDuration));
    }

    void GrowGrass()
    {
        int randomIndex = Random.Range(0, grassSprites.Length);
        spriteRenderer.sprite = grassSprites[randomIndex];
        isFlammable = true;
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
        gameObject.tag = "Ground"; // Reset the tag

        // Destroy the fire effect instance
        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }

        // Change sprite to dirt and update states
        int randomIndex = Random.Range(0, dirtSprites.Length);
        spriteRenderer.sprite = dirtSprites[randomIndex];
        isFlammable = false;
        isRevivable = true;
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
        yield return new WaitForSeconds(duration);
        isWet = false;
        spriteRenderer.color = Color.white; // Reset to original color
    }

    IEnumerator MeltIce(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        spriteRenderer.sprite = originalSprite; // Reset to original sprite
        GetWet(); // Enter wet state
    }
}