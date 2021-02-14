using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip _mainMenuTheme, _gameplayTheme, _buttonClickClip, _jumpSound, _deathSound, _victorySound;
    private static MusicPlayer _instance;
    private static AudioSource _musicSource;
    private static AudioSource _soundSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            var aSources = this.GetComponents<AudioSource>();
            _musicSource = aSources[0];
            _soundSource = aSources[1];
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            PlayMainMenuMusic();
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        _soundSource.volume = volume;
    }

    public void PlayMainMenuMusic()
    {
        PlayMusicIfNotAlready(_mainMenuTheme);
    }

    public void PlayGameplayMusic()
    {
        PlayMusicIfNotAlready(_gameplayTheme);
    }

    public void PlayJumpSound()
    {
        _soundSource.PlayOneShot(_jumpSound);
    }

    public void PlayDeathSound()
    {
        _soundSource.PlayOneShot(_deathSound);
    }

    public void PlayVictorySound()
    {
        _soundSource.PlayOneShot(_victorySound);
    }

    public void PlayButtonClickClip()
    {
        _soundSource.clip = _buttonClickClip;
        _soundSource.Play();
    }

    private void PlayMusicIfNotAlready(AudioClip clip)
    {
        if (_musicSource.clip != clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    }
}
