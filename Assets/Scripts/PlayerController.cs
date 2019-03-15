using System.Collections;
using UnityEngine;

internal sealed class PlayerController : MonoBehaviour
{
    // Movement variables
    [SerializeField]
    private float movementSpeed = 2f;
    private float m_HorizontalMovement;
    private float m_VerticalMovement;

    private Vector2 m_Direction;

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject firePoint;

    private bool m_CanFire = true;

    private void Update()
    {
        Direction();
        Movement();

        if (Input.GetKey(KeyCode.Space) && m_CanFire)
        {
            FireProjectile();
        }
    }

    /// <summary>
    /// Checks the direction and moves horizontally accordingly
    /// </summary>
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
    /// Checks the direction based on the player input
    /// </summary>
    private void Direction()
    {
        m_Direction = Vector2.zero;

        if (Input.GetKey(KeyCode.D)) // Right
        {
            m_Direction += Vector2.right;
        }
        else if (Input.GetKey(KeyCode.A)) // Left
        {
            m_Direction += Vector2.left;
        }
    }

    /// <summary>
    /// Instantiates a projectile prefab
    /// </summary>
    private void FireProjectile()
    {
        Instantiate(projectile, firePoint.transform.position, Quaternion.identity);
        m_CanFire = false;
        StartCoroutine(nameof(FireDelay));
    }

    /// <summary>
    /// Delays the time between firing another projectile
    /// </summary>
    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(0.3f);
        m_CanFire = true;
    }
}