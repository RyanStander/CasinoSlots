using System;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Receives a bet amount and a win amount and displays it on the ui to the player
    /// </summary>
    public class UIScoreManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip coinCollectSfx;
        [SerializeField] private TextMeshProUGUI winText;
        [SerializeField] private TextMeshProUGUI paidText;
        [SerializeField] private TextMeshProUGUI creditText;
        [SerializeField] private TextMeshProUGUI betText;
        [SerializeField] private float payoutSpeed = 0.25f;
        private int payoutCounter;
        private bool isPayingOut;
        private float timeStamp;

        private void OnValidate()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(!isPayingOut)
                return;

            if (timeStamp < Time.time)
            {
                payoutCounter--;
                paidText.text = (int.Parse(paidText.text) + 1).ToString();
                creditText.text = (int.Parse(creditText.text) + 1).ToString();
                audioSource.PlayOneShot(coinCollectSfx);

                if(payoutCounter == 0)
                    isPayingOut = false;
                else
                    timeStamp = Time.time + payoutSpeed;
            }
        }

        public void SetAllText(int winAmount, int paidAmount, int creditAmount, int betAmount)
        {
            winText.text = winAmount.ToString();
            paidText.text = paidAmount.ToString();
            creditText.text = creditAmount.ToString();
            betText.text = betAmount.ToString();
        }

        public void SetCredits(int creditAmount)
        {
            creditText.text = creditAmount.ToString();
            isPayingOut = false;
            paidText.text = "0";
            winText.text = "0";
        }

        public void SetBet(int betAmount)
        {
            betText.text = betAmount.ToString();
        }

        public void VictoryPayout(int winAmount)
        {
            winText.text = winAmount.ToString();
            payoutCounter = winAmount;
            paidText.text = "0";
            timeStamp = Time.time + payoutSpeed;
            isPayingOut = true;
        }
    }
}
