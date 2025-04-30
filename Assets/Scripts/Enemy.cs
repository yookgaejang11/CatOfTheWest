using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isStop = false;
    public float speed;
    public float stopDistance = 0.8f;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= stopDistance)
        {
            isStop = true;
        }
        else
        {
            isStop = false;
        }

    }

    private void Move()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= range && !isStop)
        {
            Vector2 pos = GameObject.Find("Player").transform.position - transform.position;
            transform.Translate(new Vector2(pos.x,0) * speed * Time.deltaTime);
        }
    }
}
