using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class DragonSound : MonoBehaviour
{
    [SerializeField] private AudioClip BossWakeUp;
    [SerializeField] private AudioClip BossGrowl;
    [SerializeField] private AudioClip BossRoar;
    [SerializeField] private AudioClip BossFootStep;
    [SerializeField] private AudioClip BossClaw;
    [SerializeField] private AudioClip BossBite;
    [SerializeField] private AudioClip BossTail;
    [SerializeField] private AudioClip BossWing;
    [SerializeField] private AudioClip BossGroundAttack;
    [SerializeField] private AudioClip BossBreath;
    [SerializeField] private AudioClip BossDeath;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void StartBossWakeUp()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossWakeUp);
    }

    public void StartBossGrowl()
    {
        //GetComponent<AudioSource>().Stop();
        if(!audioSource.isPlaying)
        {
            GetComponent<AudioSource>().PlayOneShot(BossGrowl);
        }
    }
    public void StartBossGrowl2()
    {

         GetComponent<AudioSource>().PlayOneShot(BossGrowl);

    }

    public void StartBossRoar()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossRoar);
    }

    public void StartBossFootStep()
    {
        //GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossFootStep);
    }
    public void StartBossAttack()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossClaw);
    }
    public void StartBossBite()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossBite);
    }
    public void StartBossTail()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossTail);
    }
    public void StartBossTail2()
    {
        GetComponent<AudioSource>().PlayOneShot(BossTail);
    }
    public void StartBossWing()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossWing);
    }
    public void StartBossGroundAttack()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossGroundAttack);
    }
    public void StartBossBreath()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossBreath);
    }
    public void StartBossDeath()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(BossDeath);
    }
}
