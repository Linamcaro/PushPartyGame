using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDown : MonoBehaviour
{

    //private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countdownText;


    private Animator animator;
    private int previousCountdownNumber;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        /*int countdownNumber = Mathf.CeilToInt(PushPartyGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            //animator.SetTrigger(NUMBER_POPUP);
            //SoundManager.Instance.PlayCountdownSound();
        }*/
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
