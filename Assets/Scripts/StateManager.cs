using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    [Header("State Tracking")]
    public int mNbPickUps = 0;
    public TMP_Text mNbPickupsTextComponent;

    [Header("PickUp Notifications")]
    public float mLengthNotification;
    public float mLengthDecay;
    public TMP_Text mIncrementNbPickupsTextComponent;


    [Header("Player Controller")]
    public PlayerController mPlayerController;


    [Header("End Level")]
    public GameObject mEndLevelDisplay;

    public void IncrementNbPickups()
    {
        mNbPickUps += 1;
        mNbPickupsTextComponent.text = (mNbPickUps < 10) ? "0" + mNbPickUps.ToString() : mNbPickUps.ToString();
        StartCoroutine("ShowIncrementComponent");
    }

    IEnumerator ShowIncrementComponent()
    {
        Color col = mIncrementNbPickupsTextComponent.color;
        col.a = 1.0f;
        mIncrementNbPickupsTextComponent.color = col;
        yield return new WaitForSeconds(mLengthNotification);
        for (float elapsed = 0f; elapsed < mLengthDecay; elapsed += 0.05f)
        {
            col.a = (mLengthDecay - elapsed) / mLengthDecay;
            mIncrementNbPickupsTextComponent.color = col;
            yield return new WaitForSeconds(0.05f);
        }

        col.a = 0.0f;
        mIncrementNbPickupsTextComponent.color = col;

        yield return null;
    }

    public void NotifyEndLevel()
    {
        Debug.Log("Congratulations! You have reached the end of the level!");
        mPlayerController.BlockPlayerInputs();
        mPlayerController.SetStatePlayerInvincible(true);
        //mEndLevelDisplay.SetActive(true);
        StartCoroutine("ShowEndLevelDisplay");
    }

    IEnumerator ShowEndLevelDisplay() {
        Image backgroundImage = mEndLevelDisplay.transform.GetChild(0).GetComponent<Image>();
        Color backgroundImageColor = backgroundImage.color;
        for (float elapsed = 0; elapsed < 1.5f; elapsed += 0.05f) {
            backgroundImageColor.a = elapsed / 1.5f;
            backgroundImage.color = backgroundImageColor;
            yield return new WaitForSeconds(0.05f);
        }

        mEndLevelDisplay.transform.GetChild(1).gameObject.SetActive(true);
        mEndLevelDisplay.transform.GetChild(2).gameObject.SetActive(true);


        yield return null;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(0);

    }

}
