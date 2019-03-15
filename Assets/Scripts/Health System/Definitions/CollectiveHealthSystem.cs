using System;
using System.Collections.Generic;
using System.Linq;

namespace Health_System.Definitions
{
    internal sealed class CollectiveHealthSystem
    {
        public int CurrentHealth => Nodes.Sum(n => n.CurrentHealth);
        public int MaxHealth => Nodes.Sum(n => n.MaxHealth);
        public int DamageReceived => Nodes.Sum(n => n.DamageReceived);
        public int DamageHealed => Nodes.Sum(n => n.DamageHealed);
        public bool IsDead => Nodes.All(n => n.IsDead);
        public Topology Topology { get; }
        public HealthNode RootNode { get; private set; }

        // Readonly to ensure list reference can't 
        // be changed once it has been created
        //
        public readonly IList<HealthNode> Nodes;

        public CollectiveHealthSystem()
        {
            Nodes = new List<HealthNode>();
            Topology = Topology.FullyConnected;
        }

        public CollectiveHealthSystem(Topology topology)
        {
            Nodes = new List<HealthNode>();
            Topology = topology;
        }

        /// <summary>
        /// Adds an individual health system to the overall health collection
        /// </summary>
        public void AddSystem(HealthNode healthNode)
        {
            if (Nodes.Contains(healthNode))
                return;

            Nodes.Add(healthNode);
        }

        /// <summary>
        /// Add a health system specified as the root to the overall health collection
        /// </summary>
        public void AddRootSystem(HealthNode healthNode)
        {
            if (Topology != Topology.Star)
            {
                throw new NotSupportedException("Must be using the star topology to add a root system.");
            }

            if (RootNode != null)
            {
                throw new NotSupportedException("Root Node already exists in this topology.");
            }

            RootNode = healthNode;
            RootNode.IsRootNode = true;
            Nodes.Add(RootNode);
        }

        /// <summary>
        /// Damages each health system by their default value
        /// </summary>
        public void DamageCollective()
        {
            foreach (var healthNode in Nodes)
            {
                healthNode.Damage();
            }

            if (Topology == Topology.Star && RootNode == null || Topology != Topology.Star)
                return;

            if (RootNode.CurrentHealth <= 0)
                ChainReaction();
        }

        /// <summary>
        /// Damages each health system by the given value
        /// </summary>
        public void DamageCollective(int damage)
        {
            foreach (var healthNode in Nodes)
            {
                healthNode.Damage(damage);
            }

            if (Topology == Topology.Star && RootNode == null || Topology != Topology.Star)
                return;

            if (RootNode.CurrentHealth <= 0)
                ChainReaction();
        }

        /// <summary>
        /// Heals all the health systems that are part
        /// of the collection by the default amount of 5%
        /// </summary>
        public void HealCollective()
        {
            foreach (var healthNode in Nodes)
            {
                healthNode.Heal();
            }
        }

        /// <summary>
        /// If root health reaches 0, reduce all health systems to 0
        /// </summary>
        private void ChainReaction()
        {
            if (Topology != Topology.Star)
                return;

            if (RootNode.CurrentHealth <= 0)
            {
                foreach (var healthNode in Nodes)
                {
                    healthNode.Damage(healthNode.CurrentHealth);
                }
            }
        }
    }
}
