using UnityEngine;
using System.Collections;

public class FlammableCharacter : MonoBehaviour
{
    private SpriteRenderer[] spriteGroup;
    public float burnDuration = 20f;
    public float wetDuration = 10f;
    public float frozenDuration = 5f;
    public int burnDamage = 1; // Damage per tick while burning
    public float burnDamageInterval = 0.5f; // Interval in seconds for applying burn damage

    private PlayerHealth playerHealth;
    private GameObject fireEffectInstance;
    public GameObject firePrefab;

    private bool isWet = false;
    private bool isFrozen = false;
    private bool isBurning = false;

    private string playerTag;

    void Start()
    {
        spriteGroup = this.transform.GetComponentsInChildren<SpriteRenderer>(true);
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (gameObject.tag == "Water")
        {
            GetWet();
        }
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
                if (gameObject.tag != "Ice")
                    GetFrozen();
                if (isBurning)
                {
                    CancelBurning();
                }
                break;
            case "Cold":
                if ((gameObject.tag != "Ice") && isWet)
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
                    foreach (SpriteRenderer spriteRenderer in spriteGroup)
                        spriteRenderer.color = Color.white;
                }
                break;
            case "Fire":
                if (!isWet && (gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame") && (gameObject.tag != "Cold"))
                {
                    Burn();
                }
                else if (isFrozen)
                {
                    InstantlyMelt();
                }
                break;
            case "Flame":
                if (!isWet && (gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame") && (gameObject.tag != "Cold"))
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
                    foreach (SpriteRenderer spriteRenderer in spriteGroup)
                        spriteRenderer.color = Color.white;
                }
                break;
        }
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
                if (gameObject.tag != "Ice")
                    GetFrozen();
                if (isBurning)
                {
                    CancelBurning();
                }
                break;
            case "Cold":
                if ((gameObject.tag != "Ice") && isWet)
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
                    foreach (SpriteRenderer spriteRenderer in spriteGroup)
                        spriteRenderer.color = Color.white;
                }
                break;
            case "Fire":
                if (!isWet && !isFrozen && (gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame") && (gameObject.tag != "Cold"))
                {
                    Burn();
                }
                else if (isFrozen)
                {
                    InstantlyMelt();
                }
                break;
            case "Flame":
                if (!isWet && (gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame") && (gameObject.tag != "Cold"))
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
                    foreach (SpriteRenderer spriteRenderer in spriteGroup)
                        spriteRenderer.color = Color.white;
                }
                break;
        }
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

        playerTag = gameObject.tag;
        gameObject.tag = "Fire";

        isBurning = true;
        if (firePrefab != null)
        {
            fireEffectInstance = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);
            fireEffectInstance.transform.localPosition = new Vector3(0, -1f, 0);
        }

        StartCoroutine(BurningCoroutine(burnDuration));
    }

    IEnumerator BurningCoroutine(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += burnDamageInterval;
            playerHealth.TakeDamage(burnDamage);
            yield return new WaitForSeconds(burnDamageInterval);
        }

        StopBurning();
    }

    void StopBurning()
    {
        isBurning = false;
        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }

        gameObject.tag = playerTag;
    }

    void GetWet()
    {
        isWet = true;
        foreach (SpriteRenderer spriteRenderer in spriteGroup)
            spriteRenderer.color = Color.Lerp(Color.blue, Color.white, 0.5f);
        StartCoroutine(ResetAfterDuration(wetDuration));
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
        foreach (SpriteRenderer spriteRenderer in spriteGroup)
            spriteRenderer.color = Color.blue;
        StartCoroutine(MeltIce(frozenDuration));
    }

    void InstantlyMelt()
    {
        StopCoroutine("MeltIce");
        isFrozen = false;
        GetWet();
    }

    IEnumerator ResetAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isWet = false;
        foreach (SpriteRenderer spriteRenderer in spriteGroup)
            spriteRenderer.color = Color.white; // Reset to original color
    }

    IEnumerator MeltIce(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        GetWet(); // Enter wet state
    }
}
