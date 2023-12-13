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

    // Use this for initialization
    void Start () {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_tran = this.transform;
        m_SpriteGroup = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponentsInChildren<SpriteRenderer>(true);

  
    }
	
	// Update is called once per frame
	void Update () {


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
        if (Input.GetKeyDown(KeyCode.RightAlt))
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
    if (Input.GetKeyUp(KeyCode.RightAlt))
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
            Debug.Log("Lclick");
            m_Animator.SetTrigger("Attack");

            m_rigidbody.velocity = new Vector3(0, 0, 0);
    }

    void PerformAttack2()
    {
        Once_Attack = false;
            Debug.Log("Rclick");
            m_Animator.SetTrigger("Attack2");

            m_rigidbody.velocity = new Vector3(0, 0, 0);
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
        if (Input.GetKey(KeyCode.A))
        {
            inputForce += Vector2.left * MoveSpeed;
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputForce += Vector2.right * MoveSpeed;
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.W))
        {
            inputForce += Vector2.up * MoveSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputForce += Vector2.down * MoveSpeed;
        }

        ApplyMovement(inputForce * Time.deltaTime);

        float h1 = Input.GetAxis("Horizontal");
            float v1 = Input.GetAxis("Vertical");
            m_Animator.SetFloat("MoveSpeed", Mathf.Abs(h1) + Mathf.Abs(v1));
    }

    void Move_FucII()
    {
        Vector2 inputForce = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.J))
        {
            inputForce += Vector2.left * MoveSpeed;
            if (B_FacingRight)
                Filp();
        }
        else if (Input.GetKey(KeyCode.L))
        {
            inputForce += Vector2.right * MoveSpeed;
            if (!B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.I))
        {
            inputForce += Vector2.up * MoveSpeed;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            inputForce += Vector2.down * MoveSpeed;
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
