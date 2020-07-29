using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip mJumpSound;
    public AudioClip mHitSound;
    public AudioClip mPickupSound;

    private AudioSource _mAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _mAudioSource = gameObject.GetComponent<AudioSource>();

    }

    public void NotifyJump()
    {
        _mAudioSource.PlayOneShot(mJumpSound);
    }

    public void NotifyHit()
    {
        _mAudioSource.PlayOneShot(mHitSound);
    }

    public void NotifyPickup()
    {
        _mAudioSource.PlayOneShot(mPickupSound);
    }
}
