using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource _musicSource;
    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
        _musicSource.Play();
    }

    public void SwitchMusic(bool stopMusic)
    {
        if (stopMusic)
            _musicSource.Pause();
        else
            _musicSource.Play();
    }
}
