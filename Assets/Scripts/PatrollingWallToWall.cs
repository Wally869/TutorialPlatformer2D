using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingWallToWall : MonoBehaviour
{
    public float mMovementSpeed = 3.0f;
    public bool bIsGoingRight = true;

    public float mRaycastingDistance = 1f;

    private SpriteRenderer _mSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mSpriteRenderer.flipX = bIsGoingRight;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionTranslation = (bIsGoingRight) ? transform.right : -transform.right;
        directionTranslation *= Time.deltaTime * mMovementSpeed;

        transform.Translate(directionTranslation);


        CheckForWalls();
    }

    private void CheckForWalls()
    {
        Vector3 raycastDirection = (bIsGoingRight) ? Vector3.right : Vector3.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + raycastDirection * mRaycastingDistance - new Vector3(0f, 0.25f, 0f), raycastDirection, 0.075f);

        if (hit.collider != null)
        {
            if (hit.transform.tag == "Terrain")
            {
                bIsGoingRight = !bIsGoingRight;
                _mSpriteRenderer.flipX = bIsGoingRight;

            }
        }
    }

}
