using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Transform playerTransform;
    private SpriteRenderer spriteRenderer;

    public GameObject stingHitBox;
    public GameObject slashHitBox;
    public GameObject kungHitBox;
    public GameObject spinHitBox;
    public GameObject rushHitBox;

    public float detectionRange;
    public float attackRange;
    public float rushMinDistance;
    public float moveSpeed;

    public int maxHp = 100;
    private int currentHp;
    private bool isAttacking = false;
    private bool isRushing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHp = maxHp;
        DisableAllHitBoxes();
        StartCoroutine(PatternLoop());
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 방향 전환
        if (playerTransform.position.x < transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        // 따라가기
        if (distance <= detectionRange && distance > attackRange && !isAttacking && !isRushing)
        {
            FollowPlayer();
            animator.SetBool("walk", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("walk", false);
        }
    }

    private void FollowPlayer()
    {
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    IEnumerator PatternLoop()
    {
        while (currentHp > 0)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            if (distance <= attackRange && !isAttacking)
            {
                isAttacking = true;
                animator.SetBool("walk", false);
                rb.velocity = Vector2.zero;

                int pattern = Random.Range(0, 5);
                switch (pattern)
                {
                    case 0: yield return StartCoroutine(Sting()); break;
                    case 1: yield return StartCoroutine(Slash()); break;
                    case 2: yield return StartCoroutine(Kung()); break;
                    case 3: yield return StartCoroutine(Spin()); break;
                    case 4: yield return StartCoroutine(LongSpin()); break;
                    //case 4:
                    //    if (distance > rushMinDistance)
                    //        yield return StartCoroutine(Rush());
                    //    break;
                }
                DisableAllHitBoxes();
                isAttacking = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Sting()
    {
        animator.SetTrigger("sting");
        yield return new WaitForSeconds(0.3f);
        stingHitBox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        stingHitBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Slash()
    {
        animator.SetTrigger("slash");
        yield return new WaitForSeconds(0.4f);
        slashHitBox.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        slashHitBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Kung()
    {
        animator.SetTrigger("kung");
        yield return new WaitForSeconds(0.5f);
        kungHitBox.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        kungHitBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Spin()
    {
        animator.SetTrigger("spin");
        spinHitBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        spinHitBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator LongSpin()
    {
        animator.SetBool("long",true);
        spinHitBox.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        spinHitBox.SetActive(false);
        animator.SetBool("long", false);
        yield return new WaitForSeconds(0.5f);

    }

    //IEnumerator Rush()
    //{
    //    isRushing = true;
    //    animator.SetTrigger("rush");
    //    rushHitBox.SetActive(true);

    //    Vector2 dir = (playerTransform.position - transform.position).normalized;
    //    float speed = 8f;
    //    float duration = 0.5f;
    //    float timer = 0f;

    //    while (timer < duration && isRushing)
    //    {
    //        rb.velocity = dir * speed;
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    rb.velocity = Vector2.zero;
    //    rushHitBox.SetActive(false);
    //    isRushing = false;
    //    yield return new WaitForSeconds(0.5f);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRushing && collision.CompareTag("Player"))
        {
            Move player = collision.GetComponent<Move>();
            if (player != null)
            {
                player.SetHp(1);
            }
            //StopRush();
        }
    }

    //public void StopRush()
    //{
    //    isRushing = false;
    //    rb.velocity = Vector2.zero;
    //    rushHitBox.SetActive(false);
    //}

    private void DisableAllHitBoxes()
    {
        stingHitBox.SetActive(false);
        slashHitBox.SetActive(false);
        kungHitBox.SetActive(false);
        spinHitBox.SetActive(false);
        rushHitBox.SetActive(false);
    }
}