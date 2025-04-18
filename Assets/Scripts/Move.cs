using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int maxBullet = 2;                                   //공중에서 발사 할 수 있는 최대 탄약
    public int currentBullet;                                   //현재 탄약
    public List<GameObject> bullets = new List<GameObject>();   //탄약 이미지
    bool isShoot = false;                                       //발사했는가 체크
    public float speed;                                         //이동 속도
    float angle;                                                //발사 각도(쓸일 없음)
    public GameObject target;                                   //날라갈 때 사용할 오브젝트(shotgun)의 방향
    public GameObject jumpdir;                                  //실제로 날아갈 방향
    Vector2 mouse;                                              //마우스 방향
    Rigidbody2D rigid;                                          //rigidbody2D
    public float GunPower = 5;                                  //총 파워
    bool isGround;                                              //땅에 닿았는가 판정
    public float rayDistance = 3;                               //레이 길이 
    RaycastHit2D ray;                                           //레이캐스트
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

        if (isGround)
        {
            currentBullet = maxBullet;
        }

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

    }

    void Shootgun()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.transform.position.y, mouse.x - target.transform.position.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        jumpdir.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentBullet > 0)
        {
            currentBullet -= 1;
            rigid.AddForce(-jumpdir.transform.up * GunPower, ForceMode2D.Impulse);
            target.GetComponent<Animator>().SetTrigger("isAttack");
            BulletUpdate();

        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") && ray.collider != null)
        {
            isGround = true;
            BulletUpdate();
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
