using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip footstepSound;
    public AudioClip rollSound;
    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip attackSound3;
    public AudioClip attackSound4;

    private void Awake()
    {
        // ½Ì±ÛÅæ ÆÐÅÏ ¼³Á¤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

    public void PlayFootstepSound() => PlaySound(footstepSound);
    public void PlayRollSound() => PlaySound(rollSound);
    //public void PlayDeathSound() => PlaySound(deathSound);
    //public void PlayHitSound() => PlaySound(hitSound);
    public void PlayAttackSound1() => PlaySound(attackSound1,0.3f);
    public void PlayAttackSound2() => PlaySound(attackSound2, 0.3f);
    public void PlayAttackSound3() => PlaySound(attackSound3, 0.3f);
    public void PlayAttackSound4() => PlaySound(attackSound4, 0.3f);
}