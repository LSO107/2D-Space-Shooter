using System.Collections;
using Extensions;
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

    private Camera m_MainCamera;
    private float m_HalfWidth;

    private bool m_CanFire = true;

    private void Start()
    {
        m_MainCamera = Camera.main;
        var box = GetComponent<BoxCollider2D>();
        m_HalfWidth = box.GetHalfWidth();
    }

    private void Update()
    {
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
        var pos = transform.position;
        if (Input.GetKey(KeyCode.D)) // Right
        {
            var movement = movementSpeed * Time.deltaTime;
            var newPos = new Vector2(pos.x + movement, pos.y);

            if(m_MainCamera.FitsOnScreen(newPos, m_HalfWidth))
            {
                transform.Translate(movement, m_VerticalMovement, 0);
            }
        }
        else if (Input.GetKey(KeyCode.A)) // Left
        {
            var movement = -movementSpeed * Time.deltaTime;
            var newPos = new Vector2(pos.x + movement, pos.y);

            if (m_MainCamera.FitsOnScreen(newPos, m_HalfWidth))
            {
                transform.Translate(movement, m_VerticalMovement, 0);
            }
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
        yield return new WaitForSeconds(0.01f);
        m_CanFire = true;
    }
}