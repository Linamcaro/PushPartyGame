using System;
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

    private void Start()
    {
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
        Hide();

    }

    private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(PushPartyGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
            SoundManager.Instance.PlayCountdownSound();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        //convert float to the smallest interger.
        int countdownNumber = Mathf.CeilToInt(PushPartyGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;

            //animator.SetTrigger(NUMBER_POPUP);
        }
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
