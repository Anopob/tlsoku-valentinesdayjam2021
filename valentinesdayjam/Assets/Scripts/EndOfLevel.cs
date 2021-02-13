using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private bool _hasEnded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hasEnded)
        {
            if (collision.CompareTag("Player"))
            {
                _canvas.gameObject.SetActive(true);
                _hasEnded = true;
            }
        }
    }
}
