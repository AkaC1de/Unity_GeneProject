using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TimeControlled
{
    [Header("角色初始")]
    public Rigidbody2D rb;
    public Animator anim;
    public Vector2 movement;
    public float speed = 10;
    [Header("枪械")]
    public GameObject[] guns;
    private int gunsNum = 0;
    [Header("拾取攻速物品后攻速增加几倍")]
    public float plusNTimes;
    [Header("拾取散弹物品后攻速减少几倍")]
    public float minusNTimes;
    [Header("冲刺")]
    public float dashSpeed;
    public float dashTime;
    float startDashTimer;
    public GameObject dashShadow;
    bool isDashing;
    Vector3 moveDir;
    [Header("冲刺位置")]
    public GameObject dashPosDown;
    public GameObject dashPosUp;
    
    public GameObject dashPosRight;
    
    public GameObject dashPosE;
    
    public GameObject dashPosC;
    // Start is called before the first frame update
    void Start()
    {
        guns[0].SetActive(true);
        
    }

    // Update is called once per frame
    
    private void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }
        if (moveX != 0)
        {
            transform.localScale = new Vector3(moveX, 1, 1);
        }

        //冲刺位置
        if (moveX != 0 && moveY == 0)
        {
            dashPosRight.SetActive(true); dashPosDown.SetActive(false);dashPosUp.SetActive(false);
            dashPosC.SetActive(false);dashPosE.SetActive(false);
        }
        
        else if(moveX == 0 && moveY > 0)
        {
            dashPosRight.SetActive(false); dashPosDown.SetActive(false); dashPosUp.SetActive(true);
            dashPosC.SetActive(false); dashPosE.SetActive(false); 
        }
        else if(moveX == 0 && moveY < 0)
        {
            dashPosRight.SetActive(false); dashPosDown.SetActive(true); dashPosUp.SetActive(false);
            dashPosC.SetActive(false); dashPosE.SetActive(false);
        }
        else if(moveX != 0 && moveY > 0)
        {
            dashPosRight.SetActive(false); dashPosDown.SetActive(false); dashPosUp.SetActive(false);
            dashPosC.SetActive(false); dashPosE.SetActive(true);
        }
        else if(moveX != 0 && moveY < 0)
        {
            dashPosRight.SetActive(false);  dashPosDown.SetActive(false); dashPosUp.SetActive(false);
            dashPosC.SetActive(true); dashPosE.SetActive(false);
        }
        else if(moveX == 0 && moveY == 0)
        {
            dashPosRight.SetActive(false);dashPosDown.SetActive(false); dashPosUp.SetActive(false);
            dashPosC.SetActive(false); dashPosE.SetActive(false); 
        }
        moveDir = new Vector3(moveX, moveY).normalized;
        SwitchAnim();
        Dash();
    }
    void FixedUpdate()
    {
        rb.velocity = moveDir * speed;
        //Movement();
    }
    //移动
    public void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        if(movement.x != 0)
        {
            transform.localScale = new Vector3(movement.x, 1, 1);
        }
        SwitchAnim();
    }
    //切换行走动画
    public void SwitchAnim()
    {
        anim.SetFloat("speed", Mathf.Abs(moveDir.x)+Mathf.Abs(moveDir.y));
    }
    //枪械切换

    public void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            guns[gunsNum].SetActive(false);
            if (--gunsNum < 0)
            {
                gunsNum = guns.Length - 1;
            }
            guns[gunsNum].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            guns[gunsNum].SetActive(false);
            if(++gunsNum > guns.Length - 1)
            {
                gunsNum = 0;
            }
            guns[gunsNum].SetActive(true);
        }
    }
    //物品收集
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShootUp"))
        {
            Pistol.interval = Pistol.interval / plusNTimes;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("BulletUp"))
        {
            guns[gunsNum].SetActive(false);
            guns[++gunsNum].SetActive(true);
            Pistol.interval = Pistol.interval / minusNTimes;
            Destroy(collision.gameObject);
        }
        //初始物品收集
        if (collision.CompareTag("InitShootUp"))
        {
            Pistol.interval = Pistol.interval / plusNTimes;
            InitCollection.off = true;
        }
        if (collision.CompareTag("InitBulletUp"))
        {
            guns[gunsNum].SetActive(false);
            guns[++gunsNum].SetActive(true);
            Pistol.interval = Pistol.interval / minusNTimes;
            InitCollection.off = true;
        }
    }
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("flash");
            isDashing = true;
        }
        if (isDashing == true)
        {
            
            rb.MovePosition(transform.position+moveDir*dashSpeed);
            isDashing = false;
        }
        //anim.ResetTrigger("flash");
    }
}
