using UnityEngine;

namespace Enemy.Definitions
{
    internal class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 0.75f;
        [SerializeField]
        private float firingDelay = 1f;
        [SerializeField]
        private GameObject projectile;
        [SerializeField]
        private GameObject firePoint;

        private float m_HorizontalMovement;
        private float m_VerticalMovement;

        private Vector2 m_Direction = Vector2.left;

        private bool m_CanFire = true;

        private void Update()
        {
            Movement();

            if (m_CanFire)
            {
                FireProjectile();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Right Barrier"))
            {
                m_Direction = Vector2.right;
            }
            else if (other.gameObject.CompareTag("Left Barrier"))
            {
                m_Direction = Vector2.left;
            }
        }

        private void Movement()
        {
            if (m_Direction == Vector2.left)
            {
                m_HorizontalMovement = -movementSpeed * Time.deltaTime;
                transform.Translate(m_HorizontalMovement, m_VerticalMovement, 0);
            }
            else if (m_Direction == Vector2.right)
            {
                m_HorizontalMovement = movementSpeed * Time.deltaTime;
                transform.Translate(m_HorizontalMovement, m_VerticalMovement, 0);
            }
        }

        /// <summary>
        /// Uses a Raycast to check for the player, if found we
        /// instantiate a projectile and delay the time before 
        /// we can fire another
        /// </summary>
        private void FireProjectile()
        {
            var hit = Physics2D.Raycast(firePoint.transform.position, Vector2.down);

            if (hit.collider)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
                    m_CanFire = false;
                    Invoke(nameof(SetCanFire), firingDelay);
                }
            }
        }

        /// <summary>
        /// Sets m_CanFire to true
        /// </summary>
        private void SetCanFire()
        {
            m_CanFire = true;
        }
    }
}
