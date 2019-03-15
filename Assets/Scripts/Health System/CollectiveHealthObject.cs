using System.Collections.Generic;
using Health_System.Definitions;
using UnityEngine;

namespace Health_System
{
    internal sealed class CollectiveHealthObject : MonoBehaviour
    {
        private CollectiveHealthSystem m_CollectiveHealthSystem;
        private HealthObject m_HealthObject;

        [SerializeField]
        private Topology topology;

        private IList<HealthObject> m_HealthNodes;

        private SpriteRenderer m_SpriteRenderer;

        // Initialize collectiveHealthSystem
        // Add the health nodes to the collective, add root if star topology
        //
        private void Start()
        {
            m_CollectiveHealthSystem = new CollectiveHealthSystem(topology);

            m_HealthNodes = GetComponentsInChildren<HealthObject>();

            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            foreach (var node in m_HealthNodes)
            {
                if (topology == Topology.Star && node.isRootNode)
                {
                    m_CollectiveHealthSystem.AddRootSystem(node.healthNode);
                }

                m_CollectiveHealthSystem.AddSystem(node.healthNode);
            }

            SetShipColour();
        }

        /// <summary>
        /// Set the colour of the ship depending on
        /// the health of the collective system
        /// </summary>
        private void SetShipColour()
        {
            var health = m_CollectiveHealthSystem.MaxHealth;

            if (health <= 600)
            {
                m_SpriteRenderer.color = new Color(90, 165, 80);
            }
            else if (health > 600 && health < 1200)
            {
                m_SpriteRenderer.color = new Color(165, 95, 20);
            }
            else if (health >= 1500)
            {
                m_SpriteRenderer.color = new Color(165, 80, 100);
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
                    m_CollectiveHealthSystem.Nodes.Remove(node.healthNode);
                }
            }

            if (m_CollectiveHealthSystem.IsDead)
            {
                Destroy(gameObject);
            }
        }
    }
}