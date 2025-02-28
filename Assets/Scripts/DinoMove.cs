using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    public int nextMove;

    public AudioSource audioSource;
    public AudioClip deadSound;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        if (nextMove == 0)
            anim.SetBool("isMove", false);
        else
            anim.SetBool("isMove", true);

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        Invoke("Think", 5);
    }

    public void DinoDamaged()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        spriteRenderer.flipY = true;
        // Collider Disalbe
        boxCollider.enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // Destroy
        Invoke("DeActive", 5);
        DeadSound();
    }

    private void DeadSound()
    {
        audioSource.PlayOneShot(deadSound);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}