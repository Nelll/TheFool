using UnityEngine;

public class MonsterSoundManager : MonoBehaviour
{
    public static MonsterSoundManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip die;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;
    public AudioClip idle1;
    public AudioClip idle2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            sfxSource.clip = clip;
            sfxSource.volume = volume;
            sfxSource.Play();
        }
    }

    public void PlayAttackSound()
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

            case 2:
                PlaySound(attack3);
                break;
        }
    }

    public void PlayDieSound()
    {
        PlaySound(die);
    }

    public void PlayHitSound()
    {
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                PlaySound(hit1);
                break;

            case 1:
                PlaySound(hit2);
                break;

            case 2:
                PlaySound(hit3);
                break;
        }
    }

    public void PlayIdleSound()
    {
        int randomIndex = Random.Range(0, 2);

        switch (randomIndex)
        {
            case 0:
                PlaySound(idle1);
                break;

            case 1:
                PlaySound(idle2);
                break;
        } 
    }
}