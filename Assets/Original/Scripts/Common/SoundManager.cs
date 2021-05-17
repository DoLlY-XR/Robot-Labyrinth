using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSource;
    public AudioClip[] audioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // プレイヤーの動作
    void FootSteps()
    {
        audioSource[0].volume = 0.1f;
        audioSource[0].pitch = 2.0f;
        audioSource[0].PlayOneShot(audioClip[0]);
    }

    void FootJump()
    {
        audioSource[0].volume = 0.2f;
        audioSource[0].pitch = 1.0f;
        audioSource[0].PlayOneShot(audioClip[0]);
    }

    public void Damage()
    {
        audioSource[0].volume = 0.1f;
        audioSource[0].pitch = 1.0f;
        audioSource[0].PlayOneShot(audioClip[1]);
    }

    void NormalShot()
    {
        audioSource[1].pitch = 1.0f;
        audioSource[1].PlayOneShot(audioClip[2]);
    }

    public void SpecialShot()
    {
        audioSource[1].pitch = 1.5f;
        audioSource[1].PlayOneShot(audioClip[3]);
    }

    void Reloading()
    {
        audioSource[1].PlayOneShot(audioClip[4]);
    }

    // コックピット
    public void Decision()
    {
        audioSource[0].PlayOneShot(audioClip[0]);
    }

    public void Recovering()
    {
        audioSource[0].PlayOneShot(audioClip[1]);
    }

    // ゲームマネージャー
    public void GameStart()
    {
        audioSource[0].PlayOneShot(audioClip[0]);
    }

    public void GameOver()
    {
        audioSource[0].PlayOneShot(audioClip[1]);
    }

    public void GameClear()
    {
        audioSource[0].PlayOneShot(audioClip[2]);
    }

    // 敵
    public void EnemyShot()
    {
        audioSource[0].PlayOneShot(audioClip[0]);
    }

    void EnemyDead()
    {
        audioSource[0].PlayOneShot(audioClip[1]);
    }
}
