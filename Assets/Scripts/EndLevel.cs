using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private StateManager _mStateManager;

    void Start() {
        _mStateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player") {
            _mStateManager.NotifyEndLevel();

        }
    }
}
