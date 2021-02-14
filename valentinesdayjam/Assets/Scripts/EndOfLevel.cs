using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private AudioSource audio;

    private bool _hasEnded = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hasEnded)
        {
            if (collision.CompareTag("Player"))
            {
                collision.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                audio.Play();
                _canvas.gameObject.SetActive(true);
                _hasEnded = true;
            }
        }
    }
}
