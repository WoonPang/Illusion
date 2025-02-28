using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public Sprite emptyBlock;
    public int maxHits = -1;

    private bool animating;

    public AudioSource audioSource;
    public AudioClip hitSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((!animating && maxHits != 0 && collision.gameObject.CompareTag("Player")))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
                Hit();
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;

        maxHits--;

        if (maxHits == 0)
            spriteRenderer.sprite = emptyBlock;

        StartCoroutine(Animate());

        HitSound();
    }

    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

    private void HitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }
}
