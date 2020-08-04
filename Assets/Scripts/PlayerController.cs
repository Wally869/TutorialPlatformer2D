using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CharacterState
{
    IDLE,
    RUNNING,
    JUMPING,
    DEAD
}

public class PlayerController : MonoBehaviour
{
    public CharacterState mPlayerState = CharacterState.IDLE;

    [Header("Movement Settings")]
    public float mSpeed = 5.0f;
    public float mJumpStrength = 10.0f;

    

    [Header("State Sprites")]
    public RuntimeAnimatorController mIdleController;
    public RuntimeAnimatorController mRunningController;
    public RuntimeAnimatorController mJumpingController;


    private Animator _mAnimatorComponent;
    private bool _bIsGoingRight = true;
    private bool _bPlayerStateChanged = false;

    private bool _bInputsDisabled = false;

    private bool _bPlayerInvincible = false;


    private SoundManager _mSoundManager;


    void Start()
    {
        _mAnimatorComponent = gameObject.GetComponent<Animator>();
        _mAnimatorComponent.runtimeAnimatorController = mIdleController;

        _mSoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

    }

    // Use state machine, much better
    void Update()
    {
        if (!_bInputsDisabled)
        {

            _bPlayerStateChanged = false;
            // check state changes
            if (mPlayerState == CharacterState.IDLE)
            {
                if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.LeftArrow)))
                {
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.RUNNING;
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        _bIsGoingRight = true;
                    }
                    else
                    {
                        _bIsGoingRight = false;
                    }
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    _mSoundManager.NotifyJump();
                    gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.JUMPING;
                    StartCoroutine("CheckGrounded");
                }
            }
            else if (mPlayerState == CharacterState.RUNNING)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _mSoundManager.NotifyJump();
                    gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * mJumpStrength;
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.JUMPING;
                    StartCoroutine("CheckGrounded");

                }
                else if (!Input.GetKey(KeyCode.RightArrow) && (!Input.GetKey(KeyCode.LeftArrow)))
                {
                    _bPlayerStateChanged = true;
                    mPlayerState = CharacterState.IDLE;
                }
            }



            if (mPlayerState == CharacterState.JUMPING || mPlayerState == CharacterState.RUNNING)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    _bIsGoingRight = true;
                    transform.Translate(transform.right * Time.deltaTime * mSpeed);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _bIsGoingRight = false;
                    transform.Translate(-transform.right * Time.deltaTime * mSpeed);
                }
            }

            gameObject.GetComponent<SpriteRenderer>().flipX = !_bIsGoingRight;
            if (_bPlayerStateChanged)
            {
                ChangeAnimator();
            }
        }

        // Check if close to wall
        CheckWall();
    }

    public void ChangeAnimator()
    {
        RuntimeAnimatorController newAnimator = mIdleController;

        if (mPlayerState == CharacterState.RUNNING || mPlayerState == CharacterState.JUMPING)
        {
            newAnimator = mRunningController;
            if (_bIsGoingRight)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        gameObject.GetComponent<Animator>().runtimeAnimatorController = newAnimator;
    }

    IEnumerator CheckGrounded()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 1f, -Vector2.up, 0.05f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain")
                {
                    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        mPlayerState = CharacterState.RUNNING;
                    }
                    else
                    {
                        mPlayerState = CharacterState.IDLE;
                    }
                    break;
                }
            }

            yield return new WaitForSeconds(0.05f);

        }

        ChangeAnimator();
        yield return null;
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Monster" && !_bPlayerInvincible)
        {
            _mSoundManager.NotifyHit();
            Vector3 heading = other.transform.position - transform.position;
            float magnitude = heading.magnitude;
            gameObject.GetComponent<Rigidbody2D>().velocity = -10f * heading / magnitude;
            StartCoroutine("Coroutine_BlockPlayerInputs");
            StartCoroutine("Coroutine_SetPlayerInvincible");

        }
    }

    IEnumerator Coroutine_BlockPlayerInputs()
    {
        _bInputsDisabled = true;
        yield return new WaitForSeconds(0.5f);
        _bInputsDisabled = false;
        yield return null;
    }

    IEnumerator Coroutine_SetPlayerInvincible()
    {
        _bPlayerInvincible = true;
        yield return new WaitForSeconds(1.5f);

        _bPlayerInvincible = false;
        yield return null;
    }

    public void BlockPlayerInputs()
    {
        _bInputsDisabled = true;
    }

    public void SetStatePlayerInvincible(bool newState)
    {
        _bPlayerInvincible = newState;
    }

    public void CheckWall() {
        List<float> directions = new List<float>{-1, 1};

        for (int i = 0; i < directions.Count; i++) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * directions[i], transform.right * directions[i], 0.05f);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Terrain") {
                    transform.Translate(-1f * transform.right * directions[i] * 0.025f);
                }
            }
        }
            
    }

}
