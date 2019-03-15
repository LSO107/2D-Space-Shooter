using Health_System.Definitions;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Health_System
{
    internal sealed class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private Image playerHealthBar;

        private HealthBarUpdater m_HealthBarUpdater;
        public HealthNode healthNode;

        private void Start()
        {
            healthNode = new HealthNode();
        }

        private void Update()
        {
            UpdatePlayerHealthBar();
        }

        private void UpdatePlayerHealthBar()
        {
            playerHealthBar.fillAmount = (float)healthNode.CurrentHealth / healthNode.MaxHealth;
            Debug.Log($"Player Health: {healthNode.CurrentHealth} / {healthNode.MaxHealth}");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                healthNode.Damage();
            }

            if (healthNode.IsDead)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}
