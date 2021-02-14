using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private AudioSource audioSource;

    private bool _hasEnded = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hasEnded)
        {
            if (collision.CompareTag("Player"))
            {
                collision.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                audioSource.Play();
                _canvas.gameObject.SetActive(true);
                _hasEnded = true;
            }
        }
    }
}
