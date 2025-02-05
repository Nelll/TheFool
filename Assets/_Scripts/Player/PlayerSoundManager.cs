using System.Collections;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip footstepSound;
    public AudioClip runstepSound;
    public AudioClip rollSound;
    public AudioClip hitSound;
    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip attackSound3;
    public AudioClip attackSound4;

    // * 추가한 부분
    private bool isSoundEnabled = false;

    private void Awake()
    {
        // 싱글톤 패턴 설정
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        Instance = this;
    }

    // * 추가한 부분
    private void Start()
    {
        sfxSource.mute = true;  // 게임 시작 시 소리를 비활성화
        StartCoroutine(EnableSoundAfterDelay(1.0f)); // 1초 후 소리 활성화
    }

    // * 추가한 부분
    private IEnumerator EnableSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.mute = false;  // 일정 시간 후 소리 활성화
        isSoundEnabled = true;
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
    public void PlayRunstepSound() => PlaySound(runstepSound);
    public void PlayRollSound() => PlaySound(rollSound);
    public void PlayHitSound() => PlaySound(hitSound);
    public void PlayAttackSound1() => PlaySound(attackSound1,0.3f);
    public void PlayAttackSound2() => PlaySound(attackSound2, 0.3f);
    public void PlayAttackSound3() => PlaySound(attackSound3, 0.3f);
    public void PlayAttackSound4() => PlaySound(attackSound4, 0.6f);
}