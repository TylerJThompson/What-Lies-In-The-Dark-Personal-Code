using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    
    [HeaderAttribute("Sounds")]
    public AudioClip[] attackClips = new AudioClip[0];
    public AudioClip[] hitClips = new AudioClip[0];

    private EnemyController enemy;
    private AudioSource audioSource = null;

    void OnEnable()
    {
        enemy = GetComponent<EnemyController>();
        audioSource = GetComponent<AudioSource>();
        enemy.onSelfAttacks += PlayAttackSound;
        enemy.onSelfHitStarts += PlayHitSound;
    }

    void OnDisable()
    {
        enemy.onSelfAttacks -= PlayAttackSound;
        enemy.onSelfHitStarts -= PlayHitSound;
    }

    void PlayAttackSound()
    {
        if(attackClips != null && attackClips.Length > 0 && !audioSource.isPlaying)
        {
            var index = Random.Range(0, attackClips.Length);
            var clip = attackClips[index];
            audioSource.PlayOneShot(clip);
        }
    }

    void PlayHitSound()
    {   
        if(hitClips != null && hitClips.Length > 0 && !audioSource.isPlaying)
        {
            var index = Random.Range(0, hitClips.Length);
            var clip = hitClips[index];
            audioSource.PlayOneShot(clip);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
