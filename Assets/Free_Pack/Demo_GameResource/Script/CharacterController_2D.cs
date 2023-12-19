using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController_2D : MonoBehaviour {
    public TrailRenderer trailRenderer;
    private float trailDuration = 0.3f;

    private Vector2 iceForce;
    public bool isOnIce = false;
    
    Rigidbody2D m_rigidbody;
    public Animator m_Animator;
    Transform m_tran;

    private float h = 0;
    private float v = 0;

    public float MoveSpeed = 40;

    public SpriteRenderer[] m_SpriteGroup;

    public bool Once_Attack = false;

    public bool isPlayer2 = false;
    
    public float dashSpeed = 2.0f;

    public float attackRange = 1f; // Adjust as needed
    public LayerMask attackLayerMask;

    public int attackDamage;

    private bool isDashing = false;
    public float doubleTapTime = 0.2f; // Time window for double-tapping
    private float lastTapTimeA = -1f;
    private float lastTapTimeD = -1f;
    private float lastTapTimeW = -1f;
    private float lastTapTimeS = -1f;

    public MeterScript fireMeter;
    private float firePower = 0;
    public GameObject firePowerEffect;
    public ParticleSystem firePowerParticle;
    private bool isFire = false;

    public MeterScript waterMeter;
    private float waterPower = 0;
    public GameObject waterPowerEffect;
    public ParticleSystem waterPowerParticle;
    private bool isWater = false;

    public MeterScript grassMeter;
    private float grassPower = 0;
    public GameObject grassPowerEffect;
    public ParticleSystem grassPowerParticle;
    private bool isGrass = false;

    public MeterScript coldMeter;
    private float coldPower = 0;
    public GameObject coldPowerEffect;
    public ParticleSystem coldPowerParticle;
    private bool isCold = false;

    public float attackInterval = 0.5f; // Time in seconds between attacks
    private float lastAttackTime = 0;

    private FootstepSounds footstepSounds;
    private FlammableCharacter flammableCharacter;

    public ParticleSystem hitEffectPrefab;

    public AudioClip[] swordSwingClips;
    public AudioClip[] hitClips;
    public AudioClip[] swordHitClips;

    public AudioClip[] powerClips;

    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_tran = this.transform;
        m_SpriteGroup = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponentsInChildren<SpriteRenderer>(true);

        fireMeter.SetMaxHealth(8f);
        waterMeter.SetMaxHealth(8f);
        grassMeter.SetMaxHealth(8f);
        coldMeter.SetMaxHealth(8f);

        footstepSounds = GetComponent<FootstepSounds>();
        flammableCharacter = GetComponent<FlammableCharacter>();

        trailRenderer.emitting = false;

        audioSource = GetComponent<AudioSource>();
    }

    void OnGUI()
    {
        foreach(string joystick in Input.GetJoystickNames())
        {
            GUILayout.Label("Joystick: " + joystick);    
        }
    }

    void Update()
    {
        string[] joystickNames = Input.GetJoystickNames();
    for (int i = 0; i < joystickNames.Length; i++)
    {
        if (!string.IsNullOrEmpty(joystickNames[i]))
        {
            Debug.Log("Joystick " + (i + 1) + ": " + joystickNames[i]);
        }
    }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (!isFire)
                {
                    if (firePower > 0 && !isWater && !isCold)
                    {
                        ActiveFirePower();
                    }
                }
                else
                {
                    DeactiveFirePower();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!isWater)
                {
                    if (waterPower > 0 && !isFire && !isGrass)
                    {
                        ActiveWaterPower();
                    }
                }
                else
                {
                    DeactiveWaterPower();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!isGrass)
                {
                    if (grassPower > 0 && !isWater && !isCold)
                    {
                        ActiveGrassPower();
                    }
                }
                else
                {
                    DeactiveGrassPower();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (!isCold)
                {
                    if (coldPower > 0 && !isFire && !isGrass)
                    {
                        ActiveColdPower();
                    }
                }
                else
                {
                    DeactiveColdPower();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (!isFire)
                {
                    if (firePower > 0 && !isWater && !isCold)
                    {
                        ActiveFirePower();
                    }
                }
                else
                {
                    DeactiveFirePower();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (!isWater)
                {
                    if (waterPower > 0 && !isFire && !isGrass)
                    {
                        ActiveWaterPower();
                    }
                }
                else
                {
                    DeactiveWaterPower();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (!isGrass)
                {
                    if (grassPower > 0 && !isWater && !isCold)
                    {
                        ActiveGrassPower();
                    }
                }
                else
                {
                    DeactiveGrassPower();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (!isCold)
                {
                    if (coldPower > 0 && !isFire && !isGrass)
                    {
                        ActiveColdPower();
                    }
                }
                else
                {
                    DeactiveColdPower();
                }
            }
        }

        /*if ((gameObject.tag == "Fire" || gameObject.tag == "Flame") && firePower > 0)
        {
            StartCoroutine(DecreasePower(firePower));
        }
        else if ((gameObject.tag == "Water" || gameObject.tag == "Ice") && waterPower > 0)
        {
            StartCoroutine(DecreasePower(waterPower));
        }
        else if ((gameObject.tag == "Grass" || gameObject.tag == "Flame") && grassPower > 0)
        {
            StartCoroutine(DecreasePower(grassPower));
        }
        else if ((gameObject.tag == "Cold" || gameObject.tag == "Ice") && coldPower > 0)
        {
            StartCoroutine(DecreasePower(coldPower));
        }*/

        if (firePower <= 0)
        {
            isFire = false;
            DeactiveFirePower();
        }
        
        if (waterPower <= 0)
        {
            isWater = false;
            DeactiveWaterPower();
        }
        
        if (grassPower <= 0)
        {
            isGrass = false;
            DeactiveGrassPower();
        }
        
        if (coldPower <= 0)
        {
            isCold = false;
            DeactiveColdPower();
        }

        if (isFire && !isGrass)
        {
            gameObject.tag = "Fire";
        }
        else if (isFire && isGrass)
        {
            gameObject.tag = "Flame";
        }
        else if (!isFire && isGrass)
        {
            gameObject.tag = "Grass";
        }
        else if (isWater && !isCold)
        {
            gameObject.tag = "Water";
        }
        else if (!isWater && isCold)
        {
            gameObject.tag = "Cold";
        }
        else if (isWater && isCold)
        {
            gameObject.tag = "Ice";
        }
        else
        {
            gameObject.tag = "Player";
        }

        if (isDashing)
        {
            footstepSounds.minInterval = 0.15f;
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                m_Animator.speed = 2.0f;
            }
            else
                m_Animator.speed = 1f;
        }
        else
        {
            footstepSounds.minInterval = 0.3f;
            m_Animator.speed = 1f;
        }
    }

   private bool isDpadLeftPressed = false;
private bool isDpadRightPressed = false;
private bool isDpadUpPressed = false;
private bool isDpadDownPressed = false;

void UsePowerI_Joystick()
{
    float dPadHorizontal = Input.GetAxis("P1_DPadHorizontal");
    float dPadVertical = Input.GetAxis("P1_DPadVertical");

    // D-pad Left
    if (dPadHorizontal < 0)
    {
        if (!isDpadLeftPressed)
        {
            isDpadLeftPressed = true;
            ToggleFirePower();
        }
    }
    else if (dPadHorizontal >= 0)
    {
        isDpadLeftPressed = false;
    }

    // D-pad Right
    if (dPadHorizontal > 0)
    {
        if (!isDpadRightPressed)
        {
            isDpadRightPressed = true;
            ToggleWaterPower();
        }
    }
    else if (dPadHorizontal <= 0)
    {
        isDpadRightPressed = false;
    }

    // D-pad Up
    if (dPadVertical > 0)
    {
        if (!isDpadUpPressed)
        {
            isDpadUpPressed = true;
            ToggleGrassPower();
        }
    }
    else if (dPadVertical <= 0)
    {
        isDpadUpPressed = false;
    }

    // D-pad Down
    if (dPadVertical < 0)
    {
        if (!isDpadDownPressed)
        {
            isDpadDownPressed = true;
            ToggleColdPower();
        }
    }
    else if (dPadVertical >= 0)
    {
        isDpadDownPressed = false;
    }
}

private bool isDpadLeftPressed2 = false;
private bool isDpadRightPressed2 = false;
private bool isDpadUpPressed2 = false;
private bool isDpadDownPressed2 = false;

void UsePowerII_Joystick()
{
    float dPadHorizontal = Input.GetAxis("P2_DPadHorizontal");
    float dPadVertical = Input.GetAxis("P2_DPadVertical");

    // D-pad Left
    if (dPadHorizontal < 0)
    {
        if (!isDpadLeftPressed2)
        {
            isDpadLeftPressed2 = true;
            ToggleFirePower();
        }
    }
    else if (dPadHorizontal >= 0)
    {
        isDpadLeftPressed2 = false;
    }

    // D-pad Right
    if (dPadHorizontal > 0)
    {
        if (!isDpadRightPressed2)
        {
            isDpadRightPressed2 = true;
            ToggleWaterPower();
        }
    }
    else if (dPadHorizontal <= 0)
    {
        isDpadRightPressed2 = false;
    }

    // D-pad Up
    if (dPadVertical > 0)
    {
        if (!isDpadUpPressed2)
        {
            isDpadUpPressed2 = true;
            ToggleGrassPower();
        }
    }
    else if (dPadVertical <= 0)
    {
        isDpadUpPressed2 = false;
    }

    // D-pad Down
    if (dPadVertical < 0)
    {
        if (!isDpadDownPressed2)
        {
            isDpadDownPressed2 = true;
            ToggleColdPower();
        }
    }
    else if (dPadVertical >= 0)
    {
        isDpadDownPressed2 = false;
    }
}

void ToggleFirePower()
{
    if (!isFire)
    {
        if (firePower > 0 && !isWater && !isCold)
        {
            ActiveFirePower();
        }
    }
    else
    {
        DeactiveFirePower();
    }
}

void ToggleWaterPower()
{
    if (!isWater)
    {
        if (waterPower > 0 && !isFire && !isGrass)
        {
            ActiveWaterPower();
        }
    }
    else
    {
        DeactiveWaterPower();
    }
}

void ToggleGrassPower()
{
    if (!isGrass)
    {
        if (grassPower > 0 && !isWater && !isCold)
        {
            ActiveGrassPower();
        }
    }
    else
    {
        DeactiveGrassPower();
    }
}

void ToggleColdPower()
{
    if (!isCold)
    {
        if (coldPower > 0 && !isFire && !isGrass)
        {
            ActiveColdPower();
        }
    }
    else
    {
        DeactiveColdPower();
    }
}

    IEnumerator DecreaseFirePower()
    {
        while (firePower > 0)
        {
            firePower -= 0.5f * Time.deltaTime;
            yield return null;
        }
        firePower = 0;
    }

    IEnumerator DecreaseWaterPower()
    {
        while (waterPower > 0)
        {
            waterPower -= 0.5f * Time.deltaTime;
            yield return null;
        }
        waterPower = 0;
    }

    IEnumerator DecreaseGrassPower()
    {
        while (grassPower > 0)
        {
            grassPower -= 0.5f * Time.deltaTime;
            yield return null;
        }
        grassPower = 0;
    }

    IEnumerator DecreaseColdPower()
    {
        while (coldPower > 0)
        {
            coldPower -= 0.5f * Time.deltaTime;
            yield return null;
        }
        coldPower = 0;
    }

    private Coroutine firePowerCoroutine;
    public void ActiveFirePower()
    {
        isFire = true;
        firePowerEffect.SetActive(true);
        firePowerParticle.Play();
        PlaySoundEffect(powerClips[0]);

        flammableCharacter.CancelBurning();

        if (firePowerCoroutine != null)
            StopCoroutine(firePowerCoroutine);
        firePowerCoroutine = StartCoroutine(DecreaseFirePower());
    }

    public void DeactiveFirePower()
    {
        isFire = false;
        firePowerEffect.SetActive(false);
        firePowerParticle.Stop();

        if (firePowerCoroutine != null)
            StopCoroutine(firePowerCoroutine);
    }

    private Coroutine waterPowerCoroutine;
    public void ActiveWaterPower()
    {
        isWater = true;
        waterPowerEffect.SetActive(true);
        waterPowerParticle.Play();
        PlaySoundEffect(powerClips[1]);

        flammableCharacter.CancelBurning();

        if (waterPowerCoroutine != null)
            StopCoroutine(waterPowerCoroutine);
        waterPowerCoroutine = StartCoroutine(DecreaseWaterPower());
    }

    public void DeactiveWaterPower()
    {
        isWater = false;
        waterPowerEffect.SetActive(false);
        waterPowerParticle.Stop();
        
        if (waterPowerCoroutine != null)
            StopCoroutine(waterPowerCoroutine);
    }

    private Coroutine grassPowerCoroutine;
    public void ActiveGrassPower()
    {
        isGrass = true;
        grassPowerEffect.SetActive(true);
        grassPowerParticle.Play();
        PlaySoundEffect(powerClips[2]);

        if (grassPowerCoroutine != null)
            StopCoroutine(grassPowerCoroutine);
        grassPowerCoroutine = StartCoroutine(DecreaseGrassPower());
    }

    public void DeactiveGrassPower()
    {
        isGrass = false;
        grassPowerEffect.SetActive(false);
        grassPowerParticle.Stop();

        if (grassPowerCoroutine != null)
            StopCoroutine(grassPowerCoroutine);
    }

    private Coroutine coldPowerCoroutine;
    public void ActiveColdPower()
    {
        isCold = true;
        coldPowerEffect.SetActive(true);
        coldPowerParticle.Play();
        PlaySoundEffect(powerClips[3]);

        flammableCharacter.CancelBurning();

        if (coldPowerCoroutine != null)
            StopCoroutine(coldPowerCoroutine);
        coldPowerCoroutine = StartCoroutine(DecreaseColdPower());
    }

    public void DeactiveColdPower()
    {
        isCold = false;
        coldPowerEffect.SetActive(false);
        coldPowerParticle.Stop();

        if (coldPowerCoroutine != null)
            StopCoroutine(coldPowerCoroutine);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        fireMeter.SetHealth(firePower);
        waterMeter.SetHealth(waterPower);
        grassMeter.SetHealth(grassPower);
        coldMeter.SetHealth(coldPower);

        //spriteOrder_Controller();

        if (!isPlayer2)
        {
            PlayerIMove();
            PlayerIMove_Joystick();
            UsePowerI_Joystick();
        }
        else
        {
            PlayerIIMove();
            PlayerIIMove_Joystick();
            UsePowerII_Joystick();
        }

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Die")||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")|| m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            return;

        if (!isPlayer2)
        {
        Move_Fuc();
        Move_Fuc_Joystick();
        }
        else
        {
            Move_FucII();
            Move_FucII_Joystick();
        }
    }

    void PlayerIMove()
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackInterval)
        {
            // Check for attack key input
            if (Input.GetKeyUp(KeyCode.C))
            {
                PerformAttack1();
                lastAttackTime = Time.time; // Update the last attack time
            }
        }

        if (Time.time >= lastAttackTime + attackInterval * 1.5f)
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                PerformAttack2();
                lastAttackTime = Time.time; // Update the last attack time
            }
        }
    }

    void PlayerIMove_Joystick()
    {
        // Check if enough time has passed since the last attack for Attack 1
        if (Time.time >= lastAttackTime + attackInterval)
        {
            // Check for attack key input for Attack 1
            if (Input.GetAxis("Joystick1Button2") > 0)
            {
                PerformAttack1();
                lastAttackTime = Time.time; // Update the last attack time
            }
        }

        // Check if enough time has passed since the last attack for Attack 2
        if (Time.time >= lastAttackTime + attackInterval * 1.5f)
        {
            // Check for attack key input for Attack 2
            if (Input.GetAxis("Joystick1Button3") > 0)
            {
                PerformAttack2();
                lastAttackTime = Time.time; // Update the last attack time
            }
        }
    }

    void PlayerIIMove()
    {
        if (Time.time >= lastAttackTime + attackInterval)
        {
            if (Input.GetKeyUp(KeyCode.Semicolon))
            {
                PerformAttack1();
                lastAttackTime = Time.time;
            }
        }

        if (Time.time >= lastAttackTime + attackInterval * 1.5f)
        {
            if (Input.GetKeyUp(KeyCode.Quote))
                {
                    PerformAttack2();
                lastAttackTime = Time.time;
                }
        }
    }

    void PlayerIIMove_Joystick()
    {
        // Check if enough time has passed since the last attack for Attack 1
        if (Time.time >= lastAttackTime + attackInterval)
        {
            // Check for attack key input for Attack 1
            if (Input.GetAxis("Joystick2Button2") > 0)
            {
                PerformAttack1();
                lastAttackTime = Time.time; // Update the last attack time
            }
        }

        // Check if enough time has passed since the last attack for Attack 2
        if (Time.time >= lastAttackTime + attackInterval * 1.5f)
        {
            // Check for attack key input for Attack 2
            if (Input.GetAxis("Joystick2Button3") > 0)
            {
                PerformAttack2();
                lastAttackTime = Time.time; // Update the last attack time
            }
        }
    }

    void PerformAttack1()
    {
        Once_Attack = false;
        m_Animator.SetTrigger("Attack");

        m_rigidbody.velocity = new Vector3(0, 0, 0);

        trailRenderer.emitting = true;
        if (!isDashing)
        {
            trailRenderer.startWidth = 0.5f;
        }
        else
        {
            trailRenderer.startWidth = 1f;
        }
        Invoke("StopTrail", trailDuration);

        PlaySoundEffect(swordSwingClips[Random.Range(0, swordSwingClips.Length)]);

        StartCoroutine(DelayedRaycastVisualization1());
    }

    IEnumerator DelayedRaycastVisualization1()
    {
        yield return new WaitForSeconds(0.15f);

        Vector2 direction = B_FacingRight ? Vector2.right : Vector2.left;
        Vector2 offset = new Vector2(attackRange * 0.5f, 0);
        Vector2 start = transform.position + (Vector3)(direction * offset);
        Vector2 size = new Vector2(1f, 2f); // Adjust the size of the box as needed

        // Perform the boxcast
        RaycastHit2D hit = Physics2D.BoxCast(start, size, 0f, direction, attackRange, attackLayerMask);

        // Visualize the boxcast
        Color rayColor = hit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(start, direction * attackRange, rayColor, 1.0f);
        Debug.DrawRay(start + new Vector2(size.x / 2, size.y / 2), direction * attackRange, rayColor, 1.0f);
        Debug.DrawRay(start - new Vector2(size.x / 2, size.y / 2), direction * attackRange, rayColor, 1.0f);
        Debug.DrawRay(start + new Vector2(size.x / 2, -size.y / 2), direction * attackRange, rayColor, 1.0f);
        Debug.DrawRay(start - new Vector2(size.x / 2, -size.y / 2), direction * attackRange, rayColor, 1.0f);
        Debug.Log("Hit collider: " + hit.collider);

        if (hit.collider != null)
        {
            PlayerHealth targetHealth = hit.collider.GetComponent<PlayerHealth>();
            WoodDoll_Mgr woodDoll = hit.collider.GetComponent<WoodDoll_Mgr>();
            EnemyController enemyController = hit.collider.GetComponent<EnemyController>();
            Projectile projectile = hit.collider.GetComponent<Projectile>();

            PlaySoundEffect(swordHitClips[Random.Range(0, swordHitClips.Length)]);

            if (targetHealth != null)
            {
                // Apply damage
                if (isDashing)
                {
                    targetHealth.TakeDamage(attackDamage * 2);
                }
                else
                {
                    targetHealth.TakeDamage(attackDamage);
                }

                ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                if (isDashing)
                    hitEffectInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Destroy(hitEffectInstance.gameObject, hitEffectInstance.main.duration);
            }

            if (woodDoll != null)
            {
                woodDoll.Sword_Hitted();
                
                switch (woodDoll.element)
                {
                    case "Fire":
                        firePower++;
                        firePower = Mathf.Clamp(firePower, 0, 8f);
                        break;
                    case "Water":
                        waterPower++;
                        waterPower = Mathf.Clamp(waterPower, 0, 8f);
                        break;
                    case "Grass":
                        grassPower++;
                        grassPower = Mathf.Clamp(grassPower, 0, 8f);
                        break;
                    case "Cold":
                        coldPower++;
                        coldPower = Mathf.Clamp(coldPower, 0, 8f);
                        break;
                }

                ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                if (isDashing)
                    hitEffectInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Destroy(hitEffectInstance.gameObject, hitEffectInstance.main.duration);
            }

            if (enemyController != null)
            {
                if (enemyController.isIce)
                {
                    if (isDashing)
                    {
                        waterPower += 2;
                        waterPower = Mathf.Clamp(waterPower, 0, 8f);
                        coldPower += 2;
                        coldPower = Mathf.Clamp(coldPower, 0, 8f);
                    }
                    else
                    {
                        waterPower++;
                        waterPower = Mathf.Clamp(waterPower, 0, 8f);
                        coldPower++;
                        coldPower = Mathf.Clamp(coldPower, 0, 8f);
                    }
                }
                else
                {
                    if (isDashing)
                    {
                        firePower += 2;
                        firePower = Mathf.Clamp(firePower, 0, 8f);
                        grassPower += 2;
                        grassPower = Mathf.Clamp(grassPower, 0, 8f);
                    }
                    else
                    {
                        firePower++;
                        firePower = Mathf.Clamp(firePower, 0, 8f);
                        grassPower++;
                        grassPower = Mathf.Clamp(grassPower, 0, 8f);
                    }
                }

                ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                if (isDashing)
                    hitEffectInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Destroy(hitEffectInstance.gameObject, hitEffectInstance.main.duration);
            }

            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }
    }

        void PerformAttack2()
    {
        Once_Attack = false;
            m_Animator.SetTrigger("Attack2");

            m_rigidbody.velocity = new Vector3(0, 0, 0);

        trailRenderer.emitting = true;
        if (!isDashing)
        {
            trailRenderer.startWidth = 0.5f;
        }
        else
        {
            trailRenderer.startWidth = 1f;
        }
        Invoke("StopTrail", trailDuration * 2f);

        PlaySoundEffect(swordSwingClips[Random.Range(0, swordSwingClips.Length)]);

        StartCoroutine(DelayedRaycastVisualization2());
    }

    IEnumerator DelayedRaycastVisualization2()
    {
        if (!isDashing)
            yield return new WaitForSeconds(0.4f);
        else
            yield return new WaitForSeconds(0.4f);

        Vector2 direction = B_FacingRight ? Vector2.right : Vector2.left;
        Vector2 offset = new Vector2(attackRange * 0.5f, 0);
        Vector2 start = transform.position + (Vector3)(direction * offset);
        Vector2 size = new Vector2(1f, 2f); // Adjust the size of the box as needed

        // Perform the boxcast
        RaycastHit2D hit = Physics2D.BoxCast(start, size, 0f, direction, attackRange * 2f, attackLayerMask);

        // Visualize the boxcast
        Color rayColor = hit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(start, direction * attackRange, rayColor, 1.0f);
        Debug.DrawRay(start + new Vector2(size.x / 2, size.y / 2), direction * attackRange * 2f, rayColor, 1.0f);
        Debug.DrawRay(start - new Vector2(size.x / 2, size.y / 2), direction * attackRange * 2f, rayColor, 1.0f);
        Debug.DrawRay(start + new Vector2(size.x / 2, -size.y / 2), direction * attackRange * 2f, rayColor, 1.0f);
        Debug.DrawRay(start - new Vector2(size.x / 2, -size.y / 2), direction * attackRange * 2f, rayColor, 1.0f);
        Debug.Log("Hit collider: " + hit.collider);

        if (hit.collider != null)
        {
            PlayerHealth targetHealth = hit.collider.GetComponent<PlayerHealth>();
            WoodDoll_Mgr woodDoll = hit.collider.GetComponent<WoodDoll_Mgr>();
            EnemyController enemyController = hit.collider.GetComponent<EnemyController>();
            Projectile projectile = hit.collider.GetComponent<Projectile>();

            PlaySoundEffect(swordHitClips[Random.Range(0, swordHitClips.Length)]);

            if (targetHealth != null)
            {
                // Apply damage
                if (isDashing)
                {
                    targetHealth.TakeDamage(attackDamage * 3);
                }
                else
                {
                    targetHealth.TakeDamage(attackDamage);
                }

                ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                if (isDashing)
                    hitEffectInstance.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(hitEffectInstance.gameObject, hitEffectInstance.main.duration);
            }

            if (woodDoll != null)
            {
                woodDoll.Sword_Hitted();

                switch (woodDoll.element)
                {
                    case "Fire":
                        firePower++;
                        firePower = Mathf.Clamp(firePower, 0, 8f);
                        break;
                    case "Water":
                        waterPower++;
                        waterPower = Mathf.Clamp(waterPower, 0, 8f);
                        break;
                    case "Grass":
                        grassPower++;
                        grassPower = Mathf.Clamp(grassPower, 0, 8f);
                        break;
                    case "Cold":
                        coldPower++;
                        coldPower = Mathf.Clamp(coldPower, 0, 8f);
                        break;
                }

                ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                if (isDashing)
                    hitEffectInstance.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(hitEffectInstance.gameObject, hitEffectInstance.main.duration);
            }

            if (enemyController != null)
            {
                if (enemyController.isIce)
                {
                    if (isDashing)
                    {
                        waterPower += 3;
                        waterPower = Mathf.Clamp(waterPower, 0, 8f);
                        coldPower += 3;
                        coldPower = Mathf.Clamp(coldPower, 0, 8f);
                    }
                    else
                    {
                        waterPower++;
                        waterPower = Mathf.Clamp(waterPower, 0, 8f);
                        coldPower++;
                        coldPower = Mathf.Clamp(coldPower, 0, 8f);
                    }
                }
                else
                {
                    if (isDashing)
                    {
                        firePower += 3;
                        firePower = Mathf.Clamp(firePower, 0, 8f);
                        grassPower +=3;
                        grassPower = Mathf.Clamp(grassPower, 0, 8f);
                    }
                    else
                    {
                        firePower++;
                        firePower = Mathf.Clamp(firePower, 0, 8f);
                        grassPower++;
                        grassPower = Mathf.Clamp(grassPower, 0, 8f);
                    }
                }

                ParticleSystem hitEffectInstance = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                if (isDashing)
                    hitEffectInstance.transform.localScale = new Vector3(2f, 2f, 2f);
                Destroy(hitEffectInstance.gameObject, hitEffectInstance.main.duration);
            }

            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }
    }

    private void StopTrail()
    {
        trailRenderer.emitting = false;
    }

    public void PlaySoundEffect(AudioClip clip, float volumn = 1.0f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volumn);
    }

    public int sortingOrder = 0;
    public int sortingOrderOrigine = 0;

    private float Update_Tic = 0;
    private float Update_Time = 0.1f;

    void spriteOrder_Controller()
    {

        Update_Tic += Time.deltaTime;

        if (Update_Tic > 0.1f)
        {
            sortingOrder = Mathf.RoundToInt(this.transform.position.y * 100);
            //Debug.Log("y::" + this.transform.position.y);
            //  Debug.Log("sortingOrder::" + sortingOrder);
            for (int i = 0; i < m_SpriteGroup.Length; i++)
            {

                m_SpriteGroup[i].sortingOrder = sortingOrderOrigine - sortingOrder;

            }

            Update_Tic = 0;
        }
    }

    // character Move Function
    void Move_Fuc()
    {
        Vector2 inputForce = new Vector2(0, 0);
        float dashMultiplier = 1f;
        isDashing = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            dashMultiplier = dashSpeed;
            isDashing = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyUp(KeyCode.F))
                Dash(Vector2.left);
            inputForce += Vector2.left * MoveSpeed * dashMultiplier;
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyUp(KeyCode.F))
                Dash(Vector2.right);
            inputForce += Vector2.right * MoveSpeed * dashMultiplier;
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyUp(KeyCode.F))
                Dash(Vector2.up);
            inputForce += Vector2.up * MoveSpeed * dashMultiplier;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyUp(KeyCode.F))
                Dash(Vector2.down);
            inputForce += Vector2.down * MoveSpeed * dashMultiplier;
        }

        ApplyMovement(inputForce * Time.deltaTime);

        // Update Animator MoveSpeed
        float h1 = Input.GetAxis("Horizontal");
        float v1 = Input.GetAxis("Vertical");
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(h1) + Mathf.Abs(v1));
    }

    void Move_Fuc_Joystick()
    {
        Vector2 inputForce = new Vector2(0, 0);
        float dashMultiplier = 1f;
        isDashing = false;

        // Controller movement input
        float horizontal = Input.GetAxis("P1_Horizontal");
        float vertical = Input.GetAxis("P1_Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        float leftTrigger = Input.GetAxis("P1_TriggerLeft");
        float rightTrigger = Input.GetAxis("P1_TriggerRight");

        if (Input.GetAxis("Joystick1Button0") > 0 || leftTrigger > 0)
        {
            dashMultiplier = dashSpeed;
            isDashing = true;
        }

        if (Time.time >= lastAttackTime + 1f)
        {
            if (Input.GetAxis("Joystick1Button1") > 0)
            {
                Dash(moveDirection);
                lastAttackTime = Time.time; 
            }
        }

        inputForce = moveDirection * MoveSpeed * dashMultiplier;

        // Flip character if necessary based on horizontal input
        if (horizontal > 0 && !B_FacingRight)
            Filp();
        else if (horizontal < 0 && B_FacingRight)
            Filp();

        ApplyMovement(inputForce * Time.deltaTime);

        // Update Animator MoveSpeed
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }

    void Move_FucII()
    {
        Vector2 inputForce = new Vector2(0, 0);
        float dashMultiplier = 1f;
        isDashing = false;

        if (Input.GetKey(KeyCode.RightShift))
        {
            dashMultiplier = dashSpeed;
            isDashing = true;
        }

        if (Input.GetKey(KeyCode.J))
        {
            if (Input.GetKeyUp(KeyCode.Slash))
                Dash(Vector2.left);
            inputForce += Vector2.left * MoveSpeed * dashMultiplier;
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.L))
        {
            if (Input.GetKeyUp(KeyCode.Slash))
                Dash(Vector2.right);
            inputForce += Vector2.right * MoveSpeed * dashMultiplier;
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.I))
        {
            if (Input.GetKeyUp(KeyCode.Slash))
                Dash(Vector2.up);
            inputForce += Vector2.up * MoveSpeed * dashMultiplier;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            if (Input.GetKeyUp(KeyCode.Slash))
                Dash(Vector2.down);
            inputForce += Vector2.down * MoveSpeed * dashMultiplier;
        }

        ApplyMovement(inputForce * Time.deltaTime);

                    // For Player 2
            float h2 = 0f;
            float v2 = 0f;
            if (Input.GetKey(KeyCode.J)) h2 = -1f;
            if (Input.GetKey(KeyCode.L)) h2 = 1f;
            if (Input.GetKey(KeyCode.I)) v2 = 1f;
            if (Input.GetKey(KeyCode.K)) v2 = -1f;
            m_Animator.SetFloat("MoveSpeed", Mathf.Abs(h2) + Mathf.Abs(v2));
    }

     void Move_FucII_Joystick()
    {
        Vector2 inputForce = new Vector2(0, 0);
        float dashMultiplier = 1f;
        isDashing = false;

        // Controller movement input
        float horizontal = Input.GetAxis("P2_Horizontal");
        float vertical = Input.GetAxis("P2_Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        float leftTrigger = Input.GetAxis("P2_TriggerLeft");
        float rightTrigger = Input.GetAxis("P2_TriggerRight");

        if (Input.GetAxis("Joystick2Button0") > 0 || leftTrigger > 0)
        {
            dashMultiplier = dashSpeed;
            isDashing = true;
        }

        if (Time.time >= lastAttackTime + 1f)
        {
            if (Input.GetAxis("Joystick2Button1") > 0)
            {
                Dash(moveDirection);
                lastAttackTime = Time.time; 
            }
        }

        inputForce = moveDirection * MoveSpeed * dashMultiplier;

        // Flip character if necessary based on horizontal input
        if (horizontal > 0 && !B_FacingRight)
            Filp();
        else if (horizontal < 0 && B_FacingRight)
            Filp();

        ApplyMovement(inputForce * Time.deltaTime);

        // Update Animator MoveSpeed
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }

    void ApplyMovement(Vector2 force)
    {
        if (isOnIce)
        {
            // Apply force gradually to simulate sliding
            iceForce = Vector2.Lerp(iceForce, force * 1.5f, Time.deltaTime * 2); // Adjust the lerp speed as needed
            m_rigidbody.AddForce(iceForce);
        }
        else
        {
            m_rigidbody.AddForce(force);
            iceForce = Vector2.zero; // Reset ice force when not on ice
        }
    }

    // Methods to enter and exit ice
    public void EnterIce()
    {
        isOnIce = true;
    }

    public void ExitIce()
    {
        isOnIce = false;
    }

    public float dashDistance = 3f; // Distance of the dash
public float dashDuration = 0.2f; // Duration of the dash

void Dash(Vector2 direction)
{
    StartCoroutine(DashCoroutine(direction));
}

IEnumerator DashCoroutine(Vector2 direction)
{
    float startTime = Time.time;
    Vector3 startPosition = transform.position;
    Vector3 endPosition = startPosition + new Vector3(direction.x, direction.y, 0) * dashDistance;

    while (Time.time < startTime + dashDuration)
    {
        float t = (Time.time - startTime) / dashDuration;
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        yield return null;
    }

    transform.position = endPosition; // Ensure exact end position
}

    // character Filp 
    bool B_Attack = false;
    bool B_FacingRight = true;

    void Filp()
    {
        B_FacingRight = !B_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        m_tran.localScale = theScale;
    }


 
    //   Sword,Dagger,Spear,Punch,Bow,Gun,Grenade


  

  
}
