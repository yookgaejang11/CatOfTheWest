using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int maxBullet = 2;                                   //���߿��� �߻� �� �� �ִ� �ִ� ź��
    public int currentBullet;                                   //���� ź��
    public List<GameObject> bullets = new List<GameObject>();   //ź�� �̹���
    bool isShoot = false;                                       //�߻��ߴ°� üũ
    public float speed;                                         //�̵� �ӵ�
    float angle;                                                //�߻� ����(���� ����)
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
    }
    private void Start()
    {

    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red);
        ray = Physics2D.Raycast(transform.position,Vector2.down,rayDistance,LayerMask.GetMask("Ground"));

       

        PlayerMove();
        Shootgun();
    }

    private void BulletUpdate()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < currentBullet; i++)
        {
            bullets[i].gameObject.SetActive(true);
        }
    }
    public void PlayerMove()
    {
        float MoveX = Input.GetAxis("Horizontal");

        Vector2 dir = new Vector2(MoveX, 0);

        rigid.AddForce(dir * speed * Time.deltaTime, ForceMode2D.Impulse);

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
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.transform.position.y, mouse.x - target.transform.position.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        jumpdir.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentBullet > 0)
        {
           
            rigid.AddForce(-jumpdir.transform.up * GunPower, ForceMode2D.Impulse);
            target.GetComponent<Animator>().SetTrigger("isAttack");
            if (!isGround)
            {
                currentBullet -= 1;
                BulletUpdate();
            }

        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") && ray.collider != null)
        {
            if (!isGround)
            {
                isGround = true;
                currentBullet = maxBullet;
                BulletUpdate();
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
