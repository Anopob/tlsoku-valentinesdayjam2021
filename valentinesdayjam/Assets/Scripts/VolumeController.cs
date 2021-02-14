using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
    private const string SOUND_VOLUME_KEY = "SOUND_VOLUME";

    private MusicPlayer _musicPlayer;
    public Slider MusicSlider, SoundSlider;

    private void Start()
    {
        _musicPlayer = FindObjectOfType<MusicPlayer>();
        MusicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });

        if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
        {
            float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
            MusicSlider.value = volume;
        }

        SoundSlider.onValueChanged.AddListener(delegate { OnSoundVolumeChange(); });

        if (PlayerPrefs.HasKey(SOUND_VOLUME_KEY))
        {
            float volume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
            SoundSlider.value = volume;
        }
    }

    private void OnMusicVolumeChange()
    {
        _musicPlayer.SetMusicVolume(MusicSlider.value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicSlider.value);
    }

    private void OnSoundVolumeChange()
    {
        _musicPlayer.SetSoundVolume(SoundSlider.value);
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, SoundSlider.value);
    }
}
