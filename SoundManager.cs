using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource mainBGM;
    [SerializeField]
    private AudioClip jumpAudio, hurtAudio, deathAudio;
    [SerializeField]
    private AudioClip cherryAudio, gemAudio;
    [SerializeField]
    private AudioClip enemyDeathAudio;

    private void Awake()
    {
        soundManager = this;
    }

    public void stopMainBGM()
    {
        mainBGM.Stop();
    }

    public void startMainBGM()
    {
        mainBGM.Play();
    }

    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void DeathAudio()
    {
        audioSource.clip = deathAudio;
        audioSource.Play();
    }

    public void CherryAudio()
    {
        audioSource.clip = cherryAudio;
        audioSource.Play();
    }

    public void GemAudio()
    {
        audioSource.clip = gemAudio;
        audioSource.Play();
    }

    public void EnemyDeathAudio()
    {
        audioSource.clip = enemyDeathAudio;
        audioSource.Play();
    }
}
