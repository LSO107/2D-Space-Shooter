using Health_System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    internal sealed class HealthBarUpdater : MonoBehaviour
    {
        [SerializeField]
        private Image healthFillBar;

        private HealthObject m_HealthObject;

        private void Start()
        {
            m_HealthObject = GetComponent<HealthObject>();
        }

        private void Update()
        {
            UpdateHealthBar(m_HealthObject.healthNode.CurrentHealth, m_HealthObject.healthNode.MaxHealth);
        }

        /// <summary>
        /// Set the health bar fill amount to the current health,
        /// divide it by maxHealth as the slider value is between
        /// 0 and 1 (0 - 100%)
        /// </summary>
        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            healthFillBar.fillAmount = (float)currentHealth / maxHealth;
            Debug.Log($"Enemy Health: {currentHealth} / {maxHealth}");
        }
    }
}
