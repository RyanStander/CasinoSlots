using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Receives a bet amount and a win amount and displays it on the ui to the player
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI winText;
        [SerializeField] private TextMeshProUGUI paidText;
        [SerializeField] private TextMeshProUGUI creditText;
        [SerializeField] private TextMeshProUGUI betText;
        [SerializeField] private float payoutSpeed = 1f;
        private int payoutCounter;

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
        }

        public void SetBet(int betAmount)
        {
            betText.text = betAmount.ToString();
        }

        public void PlayerWin(int winAmount)
        {
            winText.text = winAmount.ToString();
            payoutCounter = winAmount;
            paidText.text = "0";
        }
    }
}
