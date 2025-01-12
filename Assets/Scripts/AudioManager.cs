using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager
{
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _dragSound;
    [SerializeField] private AudioSource _oneShotAudioSource;
    [SerializeField] private AudioSource _loopingAudioSource;

    private void Awake()
    {
        if (_oneShotAudioSource == null)
        {
            _oneShotAudioSource = gameObject.AddComponent<AudioSource>();
            _oneShotAudioSource.playOnAwake = false;
        }
        if (_loopingAudioSource == null)
        {
            _loopingAudioSource = gameObject.AddComponent<AudioSource>();
            _loopingAudioSource.playOnAwake = false;
            _loopingAudioSource.loop = true;
        }
    }

    public void PlayClickSound()
    {
        _oneShotAudioSource.PlayOneShot(_clickSound);
    }

    public void PlayDragSound()
    {
        if (_loopingAudioSource.isPlaying && _loopingAudioSource.clip == _dragSound) return;
        _loopingAudioSource.clip = _dragSound;
        _loopingAudioSource.Play();
    }

    public void StopDragSound()
    {
        _loopingAudioSource.Stop();
    }
}