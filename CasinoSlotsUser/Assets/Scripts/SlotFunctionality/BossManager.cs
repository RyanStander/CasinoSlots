using System;
using Events;
using UI;
using UnityEngine;

namespace SlotFunctionality
{
    public class BossManager : MonoBehaviour
    {
        [SerializeField] private SliderBar healthBar;
        [SerializeField] private SpriteRenderer bossSprite;
        [SerializeField] private GameObject bossBattleTitle;
        [SerializeField] private int maxHealth = 30;
        [SerializeField]private int turnsToKill = 3;
        [SerializeField] private int killReward = 100;

        private int currentHealth;
        private int turnsLeft;
        private bool isDead;

        public bool IsDead => isDead;

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventIdentifiers.BossSummon, OnBossSummon);
        }

        private void OnValidate()
        {
            if (bossSprite == null)
                bossSprite = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            ChangeActive(false);
        }

        private void SetupBoss()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxValue(maxHealth);
            turnsLeft = turnsToKill;
            isDead = false;
        }

        private void ChangeActive(bool isActive)
        {
            bossSprite.enabled=isActive;
            bossBattleTitle.SetActive(isActive);
            healthBar.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// When the player scores a match, reduce the boss's health by the score
        /// </summary>
        private void OnTakeDamage(EventData eventData)
        {
            if (!eventData.IsEventOfType(out SendScore bossDamage))
                return;

            currentHealth -= bossDamage.Score;
            healthBar.SetCurrentValue(currentHealth);

            if (currentHealth > 0&& isDead)
                return;

            //give player a big score reward for killing the boss
            EventManager.currentManager.AddEvent(new SendScore(killReward));

            ChangeActive(false);
            EventManager.currentManager.Unsubscribe(EventIdentifiers.SendScore, OnTakeDamage);
            isDead = true;
        }

        private void OnBossSummon(EventData eventData)
        {
            if (!eventData.IsEventOfType(out BossSummon _) && !isDead)
                return;

            EventManager.currentManager.Subscribe(EventIdentifiers.SendScore, OnTakeDamage);
            SetupBoss();
            ChangeActive(true);
        }

        /// <summary>
        /// Reduce the amount of turns left to kill the boss
        /// </summary>
        public void OnNewSpin(EventData eventData)
        {
            turnsLeft--;
            if (turnsLeft == 0)
            {
                ChangeActive(false);
                EventManager.currentManager.Unsubscribe(EventIdentifiers.SendScore, OnTakeDamage);
            }
        }
    }
}
