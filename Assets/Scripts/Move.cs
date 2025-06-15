using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool isHit = false;
    public Vector2 moveDirection;                                      //이동벡터
    public float MoveX = 0;
    public float currentspeed;
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
    public bool isGround;                                       //땅에 닿았는가 판정
    public float rayDistance = 3;                               //레이 길이 
    public RaycastHit2D ray;                                           //레이캐스트
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
   
        if (!isDie)
        {
            currentspeed = rigid.velocity.magnitude;
            Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red);
            ray = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

            if (Input.GetButtonUp("Horizontal") && isGround)
            {
                rigid.velocity = new Vector2(rigid.velocity.x * 0.7f, rigid.velocity.y);
            }

            if (angle > 90 || angle < -90)
            {
                gunObj.GetComponent<SpriteRenderer>().flipY = true;
            }
            else if (angle <= 90 || angle >= -90)
            {
                gunObj.GetComponent<SpriteRenderer>().flipY = false;
            }

            if (isWalk)
            {
                animator.SetBool("isMove", true);
            }
            else
            {
                animator.SetBool("isMove", false);
            }
            PlayerMove();
            StartCoroutine(Shootgun());
            /*if (isGround)
            {
                speed = 5;
            }
            else if(!isGround)
            {
                speed = 3;
            }*/
           

            if (isGround && Mathf.Abs(rigid.velocity.x) > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x * 0.99f, rigid.velocity.y);
            }
            

            /*if (!isGround)
            {
                rigid.velocity = new Vector2(rigid.velocity.x * 0.98f, rigid.velocity.y);
            }*/
        }

        if(isGround)
        {
            currentBullet = maxBullet;
        }

        bulletText.text = "x" + currentBullet;

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
        if (!isHit)
        {
            currentHp -= damage;
            StartCoroutine(Invisible());
            if (currentHp <= 0 && !isDie)
            {
                Debug.Log("dfadf");
                currentHp = 0;
                isDie = true;

                UIManager.Instance.fullText = "Game\nOver";
                UIManager.Instance.ShowGameOver();
                GameManager.Instance.StageOver = true;
            }
            HpUpdate();
        }
        
    }

    IEnumerator Invisible()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);

        isHit = true;
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

        isHit = false;
    }
    public void PlayerMove()
    {
        MoveX = Input.GetAxis("Horizontal");
        rigid.AddForce(new Vector2(MoveX * speed,0),ForceMode2D.Force);
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



    IEnumerator Shootgun()
    {
        float moveX = Input.GetAxis("Horizontal");
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.transform.position.y, mouse.x - target.transform.position.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        jumpdir.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentBullet > 0)
        {
            maxSpeedx = 15;
            GameObject shootObj = Instantiate(bullet, target.transform.position, target.transform.rotation);
            moveDirection = -jumpdir.transform.up * GunPower;
            target.GetComponent<Animator>().SetTrigger("isAttack");
            StartCoroutine(Shake(0.4f, 0.3f));
            Debug.Log("dd");
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y) + moveDirection;
            if (!isGround)
            {
                currentBullet -= 1;

            }
            yield return new WaitForSeconds(0.05f);
            for (float i = maxSpeedx; i > 10; i--)
            {
                maxSpeedx -= 0.5f;
                i = maxSpeedx;
                yield return new WaitForSeconds(0.05f);
            }
            maxSpeedx = 10;
        }
       
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (ray.collider != null)
        {
            Debug.Log("왜 됨?");
            if (collision.gameObject.CompareTag("Ground") && ray.collider.name == "floor")
            {

                isGround = true;
            }
        }
        else
        {
            isGround = false;
            Debug.Log("adfasdf");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            SetHp(1);

        }

        if(collision.gameObject.name == "traps")
        {
            SetHp(1);
        }
        

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;   
        }
    }
    public IEnumerator Shake(float time, float power)
    {
        Transform camTransform = Camera.main.transform;
        Vector3 originalPos = camTransform.localPosition;

        float elapsed = 0f;

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * power;
            float y = Random.Range(-1f, 1f) * power;

            camTransform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        camTransform.localPosition = originalPos;
    }
}
