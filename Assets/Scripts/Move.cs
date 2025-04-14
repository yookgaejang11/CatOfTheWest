using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    float speed;
    float angle;
    public GameObject target;
    public GameObject jumpdir;
    Vector2 mouse;
    Rigidbody2D rigid;
    public float GunPower = 5;
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

        
    }

    
}
