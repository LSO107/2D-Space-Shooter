using Health_System;
using UnityEngine;

internal sealed class Projectile : MonoBehaviour
{
    [SerializeField]
    private Sprite projectileSprite;
    [SerializeField]
    private float maxTimeToLive = 6f;
    
    [SerializeField]
    private Vector2 m_InitialVelocity;

    private Camera m_MainCamera;

    private Rigidbody2D m_Rb;
    private SpriteRenderer m_SpriteRenderer;

    private float m_Timer;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteRenderer.sprite = projectileSprite;
        m_MainCamera = Camera.main;

        SetVelocity();
    }

    // Projectile has a set time to live
    // before it is destroyed.
    //
    private void Update()
    {
        // Destroy ourselves if we have gone off screen
        if (!IsOnScreen())
        {
            Destroy(gameObject);
        }

        m_Timer += Time.deltaTime;

        // Destroy ourselves if we have run out of time
        if (m_Timer >= maxTimeToLive)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sends the projectile using direction and speed
    /// </summary>
    private void SetVelocity()
    {
        m_Rb.velocity = m_InitialVelocity;
    }

    // If projectile collides with enemy, player or 
    // another projectile, destroy the projectile instance
    // and tell whatever we collided with to handle its own
    // bullet collision logic
    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.HandleBulletCollision();
        }

        if (other.gameObject.CompareTag("EnemyChild"))
        {
            var healthObject = other.GetComponent<HealthObject>();
            healthObject.HandleBulletCollision();
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(other);
        }

        Destroy(gameObject);
    }

    private bool IsOnScreen()
    {
        var position = transform.position;

        // Get coordinates on the screen between 0 and 1
        // ensures this method will work consistently on all resolutions
        var screenCoordinates = m_MainCamera.WorldToViewportPoint(position);

        // Check the returned coordinates to see if we're on screen
        return screenCoordinates.x < 1
            && screenCoordinates.x > 0
            && screenCoordinates.y < 1
            && screenCoordinates.y > 0;
    }
}
