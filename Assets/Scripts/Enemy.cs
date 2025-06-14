using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    bool isShoot =true;
    public GameObject bullet;
    public Vector3 pos;
    Rigidbody2D rigid;
    Vector2 movePos;
    public bool isFInd = false;
    public bool isStop = false;
    public float speed;
    public float range;
    public float shootRange;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    public void SetHp(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pos = GameObject.Find("Player").transform.position - transform.position;
        Move();
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= shootRange)
        {
            isFInd = false;
            isStop = true;
        }
        else if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= range)
        {
            isFInd = true;
            isStop = false;
        }
        else
        {
            isFInd = false;
            isStop = false;
        }

        if(isFInd || isStop)
        {
            if(pos.x < 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
                transform.GetChild(0).transform.localPosition = new Vector2(-1.254f, -0.593f);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                transform.GetChild(0).transform.localPosition = new Vector2(1.254f, -0.593f);
            }
        }

    }

    private void Move()
    {
        if (isFInd)
        {
            pos = GameObject.Find("Player").transform.position - transform.position;
            rigid.velocity = new Vector3(pos.x, 0, 0);
            /*else//���� �ȉ����� �����̴� �ڵ���
            {
                rigid.AddForce(Vector2.right,ForceMode2D.Impulse);
            }*/
        }
        else if (isStop)
        {
            StartCoroutine(ShootBullet());

        }


        rigid.velocity = new Vector2(rigid.velocity.x * 0.99f, 0);
    }


    IEnumerator ShootBullet()
    {
       if(isShoot)
        {
            isShoot = false;
            pos = GameObject.Find("Player").transform.position - transform.position;
            GameObject ammo = bullet;
            GameObject.Instantiate(ammo, transform.GetChild(0).transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.75f);
            isShoot=true;
        }
    }
}
