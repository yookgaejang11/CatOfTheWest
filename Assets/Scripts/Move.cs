using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject bullet;                                    //�Ѿ� obj
    bool isDie = false;                                         //���ӿ��� ����
    public int MaxHp = 3;                                       //�ִ� hp        
    public int currentHp = 3;                                   //���� hp
    Animator animator;
    bool isWalk;                                                //���� ����
    public TextMeshProUGUI bulletText;                          //�Ѿ� txt
    public GameObject gunObj;                                   //��(�������)
    public int maxBullet = 2;                                   //���߿��� �߻� �� �� �ִ� �ִ� ź��
    public int currentBullet;                                   //���� ź��
    public List<GameObject> hp = new List<GameObject>();   //ź�� �̹���
    bool isShoot = false;                                       //�߻��ߴ°� üũ
    public float speed;                                         //�̵� �ӵ�
    public float angle;                                         //�߻� ����(���� ����)
    public GameObject target;                                   //���� �� ����� ������Ʈ(shotgun)�� ����
    public GameObject jumpdir;                                  //������ ���ư� ����
    Vector2 mouse;                                              //���콺 ����
    Rigidbody2D rigid;                                          //rigidbody2D
    public float GunPower = 5;                                  //�� �Ŀ�
    bool isGround;                                              //���� ��Ҵ°� ����
    public float rayDistance = 3;                               //���� ���� 
    RaycastHit2D ray;                                           //����ĳ��Ʈ
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
            isShoot = true; // �߻������� true
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && ray.collider != null)
        {
            if (!isGround)
            {
                isGround = true;
                isShoot = false; // ���� ����� �� �ʱ�ȭ
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
