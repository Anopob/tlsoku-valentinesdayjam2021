using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";

    private MusicPlayer _musicPlayer;
    private Slider _slider;

    private void Start()
    {
        _musicPlayer = FindObjectOfType<MusicPlayer>();
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });

        if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
        {
            float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
            _slider.value = volume;
        }
    }

    private void OnMusicVolumeChange()
    {
        _musicPlayer.SetMusicVolume(_slider.value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _slider.value);
    }
}
