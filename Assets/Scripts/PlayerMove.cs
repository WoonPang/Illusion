using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float Speed;
    public float jump;
    private float lastHorizontalInput = 0;

    public GameManager manager;
    public UIControl UI;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Collider2D collider;
    Animator anim;

    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDead;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip overSound;
    public AudioClip finishSound;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
            return;

        // ����
        if (Input.GetKeyDown("c"))
        {
            if (isGrounded)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jump);
                // rigid.velocity = new Vector2(rigid.velocity.x, 0);
                // rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                canDoubleJump = true;
                JumpSound();
            }
            else if (!isGrounded && canDoubleJump)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jump);
                // rigid.velocity = new Vector2(rigid.velocity.x, 0);
                // rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                anim.SetBool("isJump", true);
                canDoubleJump = false;
                JumpSound();
            }
        }

        // ���� �ӵ�
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        // ���� ��ȯ
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != lastHorizontalInput)
            spriteRenderer.flipX = horizontalInput == -1;
        lastHorizontalInput = horizontalInput;

        // if (Input.GetButtonDown("Horizontal"))
            // spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // 0.3�� ������ ����Ű���� �� �� ���� > ���� ������ ����ų� �˱� ���� ���� �� �ִ�
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isMove", false);
        else
            anim.SetBool("isMove", true);
    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // ������ �̵� �ִ� �ӵ�
        if (rigid.velocity.x > Speed)
            rigid.velocity = new Vector2(Speed, rigid.velocity.y);
        // ���� �̵� �ִ� �ӵ�
        else if (rigid.velocity.x < Speed*(-1))
            rigid.velocity = new Vector2(Speed*(-1), rigid.velocity.y);

        Vector2 rayOrigin = new Vector2(collider.bounds.center.x, collider.bounds.min.y);
        RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Block", "Enemy"));

        if (rayHit.collider != null)
        {
            isGrounded = true;
            anim.SetBool("isJump", false);
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //  Attack
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                if (collision.gameObject.GetComponent<DogMove>() != null)
                    DogAttack(collision.transform);
                else if (collision.gameObject.GetComponent<DinoMove>() != null)
                    DinoAttack(collision.transform);
            }
            else
            {
                Die(collision.transform.position);
                Invoke("RestartLevel", 2f);
                OverSound();
            }
        }
        else if (collision.gameObject.tag == "Spike")
        {
            Die(collision.transform.position);
            Invoke("RestartLevel", 2f);
            OverSound();
        }
        else if (collision.gameObject.tag == "Cliff")
        {
            Die(collision.transform.position);
            Invoke("RestartLevel", 2f);
            OverSound();
        }
    }

    public void OverSound()
    {
        audioSource.PlayOneShot(overSound);
    }

    private void RestartLevel()
    {
        GameManager.Instance.ResetLevel();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            FinishSound();
            StartCoroutine(LoadNextLevel(2f));
            Debug.Log("���� ������ �Ѿ�ϴ�");
        }
    }

    public void FinishSound()
    {
        audioSource.PlayOneShot(finishSound);
    }

    private IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameManager.Instance.NextLevel();
    }

    // DogAttack, DinoAttack ���� �����̱⿡ EnmeyAttack���� �����Ͽ� ��ӽ�Ű�� ������� �� �� ����.
    void DogAttack(Transform enemy)
    {
        // Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Enemy Die
        DogMove dogMove = enemy.GetComponent<DogMove>();
        dogMove.DogDamaged();
    }

    void DinoAttack(Transform enemy)
    {
        // Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Enemy Die
        DinoMove dinoMove = enemy.GetComponent<DinoMove>();
        dinoMove.DinoDamaged();
    }

    public void Die(Vector2 targetPos)
    {
        isDead = true;
        collider.enabled = false;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        anim.SetTrigger("Die");
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}