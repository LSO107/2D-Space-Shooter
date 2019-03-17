using Extensions;
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

        private Camera m_MainCamera;

        private float m_HorizontalMovement;
        private float m_VerticalMovement;

        private bool m_IsMovingLeft;

        private bool m_CanFire = true;

        private BoxCollider2D m_BoxCollider;
        private float m_HalfWidth;

        private void Start()
        {
            m_BoxCollider = GetComponent<BoxCollider2D>();
            m_HalfWidth = m_BoxCollider.GetHalfWidth();

            m_MainCamera = Camera.main;
        }

        private void Update()
        {
            Movement();
            // Call out of bounds check here
            HandleOutOfBoundsChecks();

            if (m_CanFire)
            {
                FireProjectile();
            }
        }

        private void HandleOutOfBoundsChecks()
        {
            if(!m_MainCamera.FitsOnScreen(transform.position, m_HalfWidth))
            {
                m_IsMovingLeft = !m_IsMovingLeft;
            }
        }

        private void Movement()
        {
            if (m_IsMovingLeft)
            {
                m_HorizontalMovement = -movementSpeed * Time.deltaTime;
                transform.Translate(m_HorizontalMovement, m_VerticalMovement, 0);
            }
            else
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
