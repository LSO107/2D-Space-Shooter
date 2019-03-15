using Enemy.Definitions;
using Health_System.Definitions;
using UI;
using UnityEngine;

namespace Health_System
{
    [RequireComponent(typeof(HealthBarUpdater))]
    internal class HealthObject : MonoBehaviour
    {
        public HealthNode healthNode;

        [SerializeField]
        private EnemySize enemySize;
        [SerializeField]
        private EnemyType enemyType;
        [SerializeField]
        public bool isRootNode;
        [SerializeField]
        private bool canHeal = true;

        private float m_SlowerHealTime = 3f;
        private float m_FasterHealTime = 1.5f;
        private float m_TimeSinceLastHeal;

        private const float TimeBeforeDamage = 0.75f;
        private float m_CanBeDamagedTimer;
        private bool m_CanBeDamaged;

        private bool m_IsOnFasterTimer;

        // GetMaxHealth returns health value based on enemy size/type
        // Create an instance of HealthNode passing the health value
        //
        private void Start()
        {
            var health = GetMaxHealth();
            healthNode = new HealthNode(health, health);

            if (isRootNode)
            {
                healthNode.IsRootNode = true;
            }

            if (canHeal)
            {
                healthNode.CanHeal = true;
            }
        }

        // Manage timers & healing
        //
        private void Update()
        {
            Debug.Log($"{healthNode.CurrentHealth} / {healthNode.MaxHealth}");

            m_TimeSinceLastHeal += Time.deltaTime;

            if (!m_CanBeDamaged)
            {
                m_CanBeDamagedTimer += Time.deltaTime;

                if (m_CanBeDamagedTimer > TimeBeforeDamage)
                {
                    m_CanBeDamaged = true;
                }
            }

            if (!m_IsOnFasterTimer)
            {
                if (m_TimeSinceLastHeal >= m_SlowerHealTime)
                {
                    m_IsOnFasterTimer = true;
                    healthNode.Heal();
                    m_TimeSinceLastHeal -= m_TimeSinceLastHeal;
                }
            }
            else
            {
                if (m_TimeSinceLastHeal >= m_FasterHealTime)
                {
                    m_IsOnFasterTimer = false;
                    healthNode.Heal();
                    m_TimeSinceLastHeal -= m_TimeSinceLastHeal;
                }
            }
        }

        /// <summary>
        /// Base health of 100, then determine additional health
        /// of the object based on the enemy type and its size
        /// </summary>
        private int GetMaxHealth()
        {
            var health = 100;

            switch (enemySize)
            {
                case EnemySize.Medium:
                    health += 100;
                    break;
                case EnemySize.Large:
                    health += 200;
                    break;
            }

            switch (enemyType)
            {
                case EnemyType.LightWeight:
                    health += 100;
                    break;
                case EnemyType.MediumWeight:
                    health += 200;
                    break;
                case EnemyType.HeavyWeight:
                    health += 300;
                    break;
            }

            return health;
        }

        // Check whether we can be damaged.
        // Ensure colliding object is "Projectile".
        // Handles damage and resets timers.
        //
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_CanBeDamaged)
            {
                return;
            }

            if (!other.gameObject.CompareTag("Projectile"))
            {
                return;
            }

            HandleDamage();
            ResetHealthTimers();
        }

        /// <summary>
        /// Resets the timers
        /// </summary>
        private void ResetHealthTimers()
        {
            m_IsOnFasterTimer = false;
            m_TimeSinceLastHeal = 0;
            m_CanBeDamagedTimer = 0;
        }

        /// <summary>
        /// Ensures we can't be immediately damaged again.
        /// Calls the damage method and checks whether the health
        /// has reached 0, if yes, destroy gameObject.
        /// </summary>
        private void HandleDamage()
        {
            m_CanBeDamaged = false;
            healthNode.Damage();

            if (healthNode.IsDead)
            {
                Destroy(gameObject);
            }
        }
    }
}