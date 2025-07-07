using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip gunClickClip;
    [SerializeField] AudioClip gunShotClip;
    [SerializeField] AudioClip gunReloadClip;
    [SerializeField] AudioClip[] moveClips;
    [SerializeField] AudioClip[] hurtClips;
    [SerializeField] AudioClip[] jumpClips;
    [SerializeField] AudioClip buttonClickClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AudioDoorOpen(AudioSource source)
    {
        source.clip = doorOpenClip;
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioExplosion(AudioSource source)
    {
        source.clip = explosionClip;
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioMovement(AudioSource source)
    {
        source.clip = moveClips[Random.Range(0, moveClips.Length)];
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioGunClick(AudioSource source)
    {
        source.clip = gunClickClip;
        if (source.isPlaying == false)
            source.Play();
    }
    void AudioGunShot(AudioSource source)
    {
        source.clip = gunShotClip;
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioGunReload(AudioSource source)
    {
        source.clip = gunReloadClip;
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioHurt(AudioSource source)
    {
        source.clip = hurtClips[Random.Range(0, hurtClips.Length)];
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioJump(AudioSource source)
    {
        source.clip = jumpClips[Random.Range(0, jumpClips.Length)];
        if (source.isPlaying == false)
            source.Play();
    }

    void AudioButtonClick(AudioSource source)
    {
        source.clip = buttonClickClip;
        if (source.isPlaying == false)
            source.Play();
    }
}
