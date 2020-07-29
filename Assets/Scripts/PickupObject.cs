using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public GameObject mCollectionAnimationObject;

    private StateManager _mStateManager;
    private SoundManager _mSoundManager;

    void Start() {
        _mStateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Player") {
            _mStateManager.IncrementNbPickups();
            _mSoundManager.NotifyPickup();
            Instantiate(mCollectionAnimationObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
