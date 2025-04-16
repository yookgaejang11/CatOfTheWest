using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float speed;         //이동 속도
    float angle;                //발사 각도(쓸일 없음)
    public GameObject target;   //날라갈 때 사용할 오브젝트(shotgun)의 방향
    public GameObject jumpdir;  //실제로 날아갈 방향
    Vector2 mouse;              //마우스 방향
    Rigidbody2D rigid;          //rigidbody2D
    public float GunPower = 5;  //총 파워
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.transform.position.y, mouse.x - target.transform.position.x) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

       
        jumpdir.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("adfd");
            rigid.AddForce(-jumpdir.transform.up * GunPower * Time.deltaTime, ForceMode2D.Impulse);
            target.GetComponent<Animator>().SetTrigger("isAttack");
        }

        PlayerMove();
        
    }

    public void PlayerMove()
    {
        float MoveX = Input.GetAxis("Horizontal");

        Vector2 dir = new Vector2(MoveX, 0);

        rigid.AddForce(dir * speed * Time.deltaTime, ForceMode2D.Impulse);

    }
}
