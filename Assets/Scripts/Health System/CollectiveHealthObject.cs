using System.Collections.Generic;
using Health_System.Definitions;
using UnityEngine;

namespace Health_System
{
    internal sealed class CollectiveHealthObject : MonoBehaviour
    {
        [HideInInspector]
        public CollectiveHealthSystem collectiveHealthSystem;

        private HealthObject m_HealthObject;

        [SerializeField]
        private Topology topology;

        private IList<HealthObject> m_HealthNodes;

        // Initialize collectiveHealthSystem
        // Add the health nodes to the collective, add root if star topology
        //
        private void Start()
        {
            collectiveHealthSystem = new CollectiveHealthSystem(topology);

            m_HealthNodes = GetComponentsInChildren<HealthObject>();

            foreach (var node in m_HealthNodes)
            {
                if (topology == Topology.Star && node.isRootNode)
                {
                    collectiveHealthSystem.AddRootSystem(node.healthNode);
                }

                collectiveHealthSystem.AddSystem(node.healthNode);
            }
        }

        // Remove nodes from the collective that have reached 0 health.
        // If all systems in a collect have been destroyed,
        // destroy the parent (collective)
        //
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Projectile"))
                return;

            foreach (var node in m_HealthNodes)
            {
                if (node.healthNode.IsDead)
                {
                    collectiveHealthSystem.Nodes.Remove(node.healthNode);
                }
            }

            if (collectiveHealthSystem.IsDead)
            {
                Destroy(gameObject);
            }
        }
    }
}