using UnityEngine;

public class BugSoundManager : MonoBehaviour
{
    public static BugSoundManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip die;
    public AudioClip hit;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip walk3;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null && !sfxSource.isPlaying)
        {
            sfxSource.clip = clip;
            sfxSource.volume = volume;
            sfxSource.Play();
        }
    }

    public void PlayAttackSound()
    {
        if (!sfxSource.isPlaying)
        {
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    PlaySound(attack1);
                    break;

                case 1:
                    PlaySound(attack2);
                    break;
            }
        }
    }

    public void PlayDieSound()
    {
        if (!sfxSource.isPlaying)
        {
            PlaySound(die);
        }
    }

    public void PlayHitSound()
    {
        if (!sfxSource.isPlaying)
        {
            PlaySound(hit);
        }
    }

    public void PlayWalkSound()
    {
        if (!sfxSource.isPlaying)
        {
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    PlaySound(walk1);
                    break;

                case 1:
                    PlaySound(walk2);
                    break;

                case 2:
                    PlaySound(walk3);
                    break;

            }
        }
    }
}