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
    void Awake()
    {
        instance = this;
    }

    public void AudioDoorOpen(AudioSource source)
    {
        source.clip = doorOpenClip;
        if (source.isPlaying == false)
            source.Play();
    }

    public void AudioExplosion(AudioSource source)
    {
        source.clip = explosionClip;
        if (source.isPlaying == false)
            source.Play();
    }

    public void AudioMovement(AudioSource source)
    {
        if (source.isPlaying)
            return;

        var clip = moveClips[Random.Range(0, moveClips.Length)];
        source.clip = clip;
        source.Play();
    }

    public void AudioGunClick(AudioSource source)
    {
        source.clip = gunClickClip;
        if (source.isPlaying == false)
            source.Play();
    }
    public void AudioGunShot(AudioSource source)
    {
        source.clip = gunShotClip;
        source.Play();
    }

    public void AudioGunReload(AudioSource source)
    {
        source.clip = gunReloadClip;
        if (source.isPlaying == false)
            source.Play();
    }

    public void AudioHurt(AudioSource source)
    {
        source.clip = hurtClips[Random.Range(0, hurtClips.Length)];
        if (source.isPlaying == false)
            source.Play();
    }

    public void AudioJump(AudioSource source)
    {
        source.clip = jumpClips[Random.Range(0, jumpClips.Length)];
        if (source.isPlaying == false)
            source.Play();
    }

    public void AudioButtonClick(AudioSource source)
    {
        source.clip = buttonClickClip;
        if (source.isPlaying == false)
            source.Play();
    }
}
