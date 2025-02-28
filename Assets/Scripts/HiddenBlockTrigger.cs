using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlockTrigger : MonoBehaviour
{
    public GameObject hiddenBlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y < transform.position.y)
            {
                hiddenBlock.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
