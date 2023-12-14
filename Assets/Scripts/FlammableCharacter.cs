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

    public GameObject frozenEffect;
    public GameObject wetEffect;

    public Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private int characterDamage;

    private CharacterController_2D characterController;

    void Start()
    {
        spriteGroup = this.transform.GetComponentsInChildren<SpriteRenderer>(true);
        playerHealth = GetComponent<PlayerHealth>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        characterDamage = characterController.attackDamage;
        characterController = GetComponent<CharacterController_2D>();
    }

    void Update()
    {
        if (isFrozen)
        {
            frozenEffect.SetActive(true);
/*             SpriteRenderer spriteRenderer = wetEffect.GetComponent<SpriteRenderer>();
            Color newColor = spriteRenderer.color;
            newColor.a = 1f;
            spriteRenderer.color = newColor; */
            characterController.attackDamage = 0;
        }
        else
        {
            frozenEffect.SetActive(false);
/*             SpriteRenderer spriteRenderer = wetEffect.GetComponent<SpriteRenderer>();
            Color newColor = spriteRenderer.color;
            newColor.a = 0;
            spriteRenderer.color = newColor; */
            characterController.attackDamage = characterDamage;
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
                if (!isWet && (gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame") && (gameObject.tag != "Cold") && (gameObject.tag != "Water"))
                {
                    Burn();
                }
                else if (isFrozen)
                {
                    InstantlyMelt();
                }
                break;
            case "Flame":
                if ((gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame"))
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
                break;
            case "Ice":
                if (gameObject.tag != "Ice")
                    GetFrozen();
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
                if (!isWet && !isFrozen && (gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame") && (gameObject.tag != "Cold") && (gameObject.tag != "Water"))
                {
                    Burn();
                }
                else if (isFrozen)
                {
                    InstantlyMelt();
                }
                break;
            case "Flame":
                if ((gameObject.tag != "Ice") && (gameObject.tag != "Fire") && (gameObject.tag != "Flame"))
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
            if (gameObject.tag == "Grass")
                playerHealth.TakeFireDamage(burnDamage * 2);
            else
                playerHealth.TakeFireDamage(burnDamage);
            yield return new WaitForSeconds(burnDamageInterval);
        }

        CancelBurning();
    }

    void GetWet()
    {
        if (!isFrozen)
        {
            if (isBurning)
            {
                CancelBurning();
            }
            if (!isFrozen)
            {
                isWet = true;
                foreach (SpriteRenderer spriteRenderer in spriteGroup)
                    spriteRenderer.color = Color.Lerp(Color.blue, Color.white, 0.5f);
                StartCoroutine(ResetAfterDuration(wetDuration));
            }
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
        gameObject.tag = playerTag;
    }

    void GetFrozen()
    {
        if (isBurning)
                {
                    CancelBurning();
                }
        isFrozen = true;
        StopPlayerMovement();
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
            foreach (SpriteRenderer spriteRenderer1 in spriteGroup)
            StartCoroutine(FadeToWhite(spriteRenderer1, duration));
    float elapsed = 0;
            SpriteRenderer spriteRenderer = wetEffect.GetComponent<SpriteRenderer>();
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;
            spriteRenderer.color = newColor;

        yield return null; // Wait for the next frame
    }

    isWet = false;

        Color finalColor = spriteRenderer.color;
        finalColor.a = 0; // Ensure final alpha is set to 0
        spriteRenderer.color = finalColor;
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
        ResumePlayerMovement();
        GetWet(); // Enter wet state
    }

    public void StopPlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.angularVelocity = 0f;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze position and rotation
        }

        if (playerAnimator != null)
        {
            playerAnimator.speed = 0; // Freeze animations
        }
    }

    public void ResumePlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None; // Or any other constraints you originally had
        }

        if (playerAnimator != null)
        {
            playerAnimator.speed = 1; // Resume animations
        }
    }
}
