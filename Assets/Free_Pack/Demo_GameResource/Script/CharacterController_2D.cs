using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_2D : MonoBehaviour {

  
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
    private bool isSpacePressed = false;
    private float spacePressTime = 0f;
    public float longPressThreshold = 0.2f;

    private bool isRAltPressed = false;
    private float rAltPressTime = 0f;
    
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

    // Use this for initialization
    void Start () {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_tran = this.transform;
        m_SpriteGroup = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponentsInChildren<SpriteRenderer>(true);

    fireMeter.SetMaxHealth(8f);
    waterMeter.SetMaxHealth(8f);
    grassMeter.SetMaxHealth(8f);
    coldMeter.SetMaxHealth(8f);
    }

    void Update()
    {
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

        if ((gameObject.tag == "Fire" || gameObject.tag == "Flame") && firePower > 0)
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
        }

        if (firePower <= 0)
        {
            isFire = false;
            DeactiveFirePower();
        }
        else if (waterPower <= 0)
        {
            isWater = false;
            DeactiveWaterPower();
        }
        else if (grassPower <= 0)
        {
            isGrass = false;
            DeactiveGrassPower();
        }
        else if (coldPower <= 0)
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
    }

    IEnumerator DecreasePower(float firePower)
    {
        while (firePower > 0)
        {
            firePower -= 0.5f * Time.deltaTime;
            Debug.Log("Fire Power: " + firePower);
            yield return null;
        }

        firePower = 0; // Ensure firePower doesn't go below zero
    }

    public void ActiveFirePower()
    {
        isFire = true;
        firePowerEffect.SetActive(true);
        firePowerParticle.Play();
    }

    public void DeactiveFirePower()
    {
        isFire = false;
        firePowerEffect.SetActive(false);
        firePowerParticle.Stop();
    }

    public void ActiveWaterPower()
    {
        isWater = true;
        waterPowerEffect.SetActive(true);
        waterPowerParticle.Play();
    }

    public void DeactiveWaterPower()
    {
        isWater = false;
        waterPowerEffect.SetActive(false);
        waterPowerParticle.Stop();
    }

    public void ActiveGrassPower()
    {
        isGrass = true;
        grassPowerEffect.SetActive(true);
        grassPowerParticle.Play();
    }

    public void DeactiveGrassPower()
    {
        isGrass = false;
        grassPowerEffect.SetActive(false);
        grassPowerParticle.Stop();
    }

    public void ActiveColdPower()
    {
        isCold = true;
        coldPowerEffect.SetActive(true);
        coldPowerParticle.Play();
    }

    public void DeactiveColdPower()
    {
        isCold = false;
        coldPowerEffect.SetActive(false);
        coldPowerParticle.Stop();
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
        }
        else
        {
            PlayerIIMove();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
          
            Debug.Log("1");
            m_Animator.Play("Hit");




        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("2");
            m_Animator.Play("Die");


        }


        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Die")||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")|| m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            return;

        if (!isPlayer2)
        {
        Move_Fuc();
        }
        else
        {
            Move_FucII();
        }
 
    }

    void PlayerIMove()
    {
        // Check if the space key is initially pressed
    if (Input.GetKeyDown(KeyCode.Space))
    {
        isSpacePressed = true;
        spacePressTime = 0f;
    }

    // If space key is being held down, increment the timer
    if (isSpacePressed)
    {
        spacePressTime += Time.deltaTime;
    }

    // Check if the space key is released
    if (Input.GetKeyUp(KeyCode.Space))
    {
        if (spacePressTime < longPressThreshold)
        {
            PerformAttack1();
        }
        else
        {
            PerformAttack2();
        }
        isSpacePressed = false;
    }
    }

    void PlayerIIMove()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
    {
        isRAltPressed = true;
        rAltPressTime = 0f;
    }

    // If space key is being held down, increment the timer
    if (isRAltPressed)
    {
        rAltPressTime += Time.deltaTime;
    }

    // Check if the space key is released
    if (Input.GetKeyUp(KeyCode.Slash))
    {
        if (rAltPressTime < longPressThreshold)
        {
            PerformAttack1();
        }
        else
        {
            PerformAttack2();
        }
        isRAltPressed = false;
    }
    }

    void PerformAttack1()
    {
        Once_Attack = false;
            m_Animator.SetTrigger("Attack");

            m_rigidbody.velocity = new Vector3(0, 0, 0);
            
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
            }
        }
    }

    void PerformAttack2()
    {
        Once_Attack = false;
            m_Animator.SetTrigger("Attack2");

            m_rigidbody.velocity = new Vector3(0, 0, 0);
        
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
            }
        }
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
            inputForce += Vector2.left * MoveSpeed * dashMultiplier;
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputForce += Vector2.right * MoveSpeed * dashMultiplier;
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.W))
        {
            inputForce += Vector2.up * MoveSpeed * dashMultiplier;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputForce += Vector2.down * MoveSpeed * dashMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.A))
    {
        if (Time.time < lastTapTimeA + doubleTapTime)
            Dash(Vector2.left);
        lastTapTimeA = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.D))
    {
        if (Time.time < lastTapTimeD + doubleTapTime)
            Dash(Vector2.right);
        lastTapTimeD = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.W))
    {
        if (Time.time < lastTapTimeW + doubleTapTime)
            Dash(Vector2.up);
        lastTapTimeW = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.S))
    { 
        if (Time.time < lastTapTimeS + doubleTapTime)
            Dash(Vector2.down);
        lastTapTimeS = Time.time;
    }

        ApplyMovement(inputForce * Time.deltaTime);

        // Update Animator MoveSpeed
        float h1 = Input.GetAxis("Horizontal");
        float v1 = Input.GetAxis("Vertical");
        m_Animator.SetFloat("MoveSpeed", Mathf.Abs(h1) + Mathf.Abs(v1));
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
            inputForce += Vector2.left * MoveSpeed * dashMultiplier;
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.L))
        {
            inputForce += Vector2.right * MoveSpeed * dashMultiplier;
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.I))
        {
            inputForce += Vector2.up * MoveSpeed * dashMultiplier;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            inputForce += Vector2.down * MoveSpeed * dashMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.J))
    {
        if (Time.time < lastTapTimeA + doubleTapTime)
            Dash(Vector2.left);
        lastTapTimeA = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.L))
    {
        if (Time.time < lastTapTimeD + doubleTapTime)
            Dash(Vector2.right);
        lastTapTimeD = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.I))
    {
        if (Time.time < lastTapTimeW + doubleTapTime)
            Dash(Vector2.up);
        lastTapTimeW = Time.time;
    }

    if (Input.GetKeyDown(KeyCode.K))
    { 
        if (Time.time < lastTapTimeS + doubleTapTime)
            Dash(Vector2.down);
        lastTapTimeS = Time.time;
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
