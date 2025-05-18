using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject bullet;                                    //총알 obj
    bool isDie = false;                                         //게임오버 판정
    public int MaxHp = 3;                                       //최대 hp        
    public int currentHp = 3;                                   //현재 hp
    Animator animator;
    bool isWalk;                                                //걸음 판정
    public TextMeshProUGUI bulletText;                          //총알 txt
    public GameObject gunObj;                                   //총(돌리기용)
    public int maxBullet = 2;                                   //공중에서 발사 할 수 있는 최대 탄약
    public int currentBullet;                                   //현재 탄약
    public List<GameObject> hp = new List<GameObject>();   //탄약 이미지
    bool isShoot = false;                                       //발사했는가 체크
    public float speed;                                         //이동 속도
    public float angle;                                         //발사 각도(쓸일 생김)
    public GameObject target;                                   //날라갈 때 사용할 오브젝트(shotgun)의 방향
    public GameObject jumpdir;                                  //실제로 날아갈 방향
    Vector2 mouse;                                              //마우스 방향
    Rigidbody2D rigid;                                          //rigidbody2D
    public float GunPower = 5;                                  //총 파워
    bool isGround;                                              //땅에 닿았는가 판정
    public float rayDistance = 3;                               //레이 길이 
    RaycastHit2D ray;                                           //레이캐스트
    public float maxSpeedx;
    public float maxSpeedy;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red);
        ray = Physics2D.Raycast(transform.position,Vector2.down,rayDistance,LayerMask.GetMask("Ground") );
        
        if(Input.GetButtonUp("Horizontal") && isGround)
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.7f, rigid.velocity.normalized.y);
        }

        if (angle > 90 || angle< -90)
        {
            gunObj.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (angle <= 90 ||angle >= -90)
        {
            gunObj.GetComponent<SpriteRenderer>().flipY = false;
        }
        if (isGround && !isShoot)
        {
            currentBullet = maxBullet;
            bulletText.text = "x" + currentBullet;
        }

        if (isWalk)
        {
            animator.SetBool("isMove",true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
        PlayerMove();
        Shootgun();

        if(isGround && Mathf.Abs(rigid.velocity.x) >0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x * 0.99f, rigid.velocity.y);
        }

    }


    private void HpUpdate()
    {
        
        for (int i = 0; i < MaxHp; i++)
        {
            hp[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < currentHp; i++)
        {
            hp[i].gameObject.SetActive(true);
        }
    }

    public void SetHp(int damage)
    {
        currentHp -= damage;
        StartCoroutine(Invisible());
        if (currentHp < 0)
        {
            currentHp = 0;
            isDie = true;
        }
        HpUpdate();
    }

    IEnumerator Invisible()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        
        
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
    public void PlayerMove()
    {
        float MoveX = Input.GetAxis("Horizontal");

        transform.position += new Vector3(MoveX * speed * Time.deltaTime, 0, 0);

        if (Input.GetAxis("Horizontal") != 0)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        if(Input.GetAxis("Horizontal")>0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX  = false;
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            this.gameObject.GetComponent <SpriteRenderer>().flipX = true;
        }
        if (rigid.velocity.x >= maxSpeedx) 
        {
            rigid.velocity = new Vector2(maxSpeedx, rigid.velocity.y); 
        }
        else if  (rigid.velocity.x <= -maxSpeedx)
        {
            rigid.velocity = new Vector2(-maxSpeedx, rigid.velocity.y); 
        }
        if (rigid.velocity.y >= maxSpeedy)
        {
            rigid.velocity = new Vector2(rigid.velocity.x,maxSpeedy);
        }
    }



    void Shootgun()
    {
        float moveX = Input.GetAxis("Horizontal");
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.transform.position.y, mouse.x - target.transform.position.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                                                                                                                            

        jumpdir.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentBullet > 0)
        {
            GameObject shootObj = Instantiate(bullet,target.transform.position,target.transform.rotation); 
            rigid.AddForce(-jumpdir.transform.up * GunPower, ForceMode2D.Impulse);
            target.GetComponent<Animator>().SetTrigger("isAttack");

            if(!isGround)
            {
                currentBullet -= 1;
                bulletText.text = "x" + currentBullet;
            }
            isShoot = true; // 발사했으니 true
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && ray.collider != null)
        {
            if (!isGround)
            {
                isGround = true;
                isShoot = false; // 땅에 닿았을 때 초기화
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;   
        }
    }
}
