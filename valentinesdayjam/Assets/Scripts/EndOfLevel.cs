using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private MusicPlayer _musicPlayer;
    private bool _hasEnded = false;

    void Start()
    {
        _musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hasEnded)
        {
            if (collision.CompareTag("Player"))
            {
                collision.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                _musicPlayer.PlayVictorySound();
                _canvas.gameObject.SetActive(true);
                _hasEnded = true;
            }
        }
    }
}
