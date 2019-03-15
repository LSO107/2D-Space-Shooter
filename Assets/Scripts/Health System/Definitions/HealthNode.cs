using System;

namespace Health_System.Definitions
{
    internal sealed class HealthNode
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; }
        public int DamageReceived { get; private set; }
        public int DamageHealed { get; private set; }
        public bool CanHeal { get; set; }
        public bool IsRootNode { get; set; }
        public bool IsDead => CurrentHealth <= 0;

        public HealthNode()
        {
            CurrentHealth = 100;
            MaxHealth = 100;
        }

        public HealthNode(int currentHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = 100;

            ClampHealth();
        }

        public HealthNode(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;

            ClampHealth();
        }

        /// <summary>
        /// Prevents the current health being set below 0
        /// and above the maximum health
        /// </summary>
        private void ClampHealth()
        {
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        /// <summary>
        /// Adds a default of 5% of the maximum health value
        /// </summary>
        public void Heal()
        {
            if (IsDead || !CanHeal)
                return;

            var healAmount = (decimal)MaxHealth / 100 * 5;
            CurrentHealth += Convert.ToInt32(healAmount);

            if (CurrentHealth >= MaxHealth)
            {
                CurrentHealth = MaxHealth;
                return;
            }

            DamageHealed += (int)healAmount;
        }

        /// <summary>
        /// Subtracts a default of 10% of the maximum health value
        /// </summary>
        public void Damage()
        {
            var damage = MaxHealth / 100 * 10;

            Damage(damage);
        }

        /// <summary>
        /// Subtracts a specified amount of damage from the current health
        /// </summary>
        public void Damage(int damage)
        {
            if (IsDead)
                return;

            CurrentHealth -= damage;

            ClampHealth();

            DamageReceived = (MaxHealth - CurrentHealth) + DamageHealed;
        }
    }
}
