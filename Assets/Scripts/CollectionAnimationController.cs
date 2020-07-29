using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionAnimationController : MonoBehaviour
{
    public Animator mAnimator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("OnCompleteAnimation");
    }

    // Update is called once per frame
    IEnumerator OnCompleteAnimation()
    {
        while(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        Destroy(gameObject);
    }

}
