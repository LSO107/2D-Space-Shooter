using UnityEngine;

internal sealed class Projectile : MonoBehaviour
{
    [SerializeField]
    private Sprite projectileSprite;
    [SerializeField]
    private float maxTimeToLive = 6f;
    [SerializeField]
    private float projectileSpeed = 2f;
    [SerializeField]
    private bool enemyProjectile;

    private Rigidbody2D m_Rb;
    private SpriteRenderer m_SpriteRenderer;

    private float m_Timer;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteRenderer.sprite = projectileSprite;
    }

    // Projectile has a set time to live
    // before it is destroyed.
    //
    private void Update()
    {
        FireProjectile();

        m_Timer += Time.deltaTime;

        if (m_Timer >= maxTimeToLive)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sends the projectile using direction and speed
    /// </summary>
    private void FireProjectile()
    {
        if (enemyProjectile)
        {
            m_Rb.velocity = Vector3.down * projectileSpeed;
        }
        else
        {
            m_Rb.velocity = Vector2.up * projectileSpeed;
        }
    }

    // If projectile collides with enemy, player or 
    // another projectile, destroy the projectile instance
    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject, 0.1f);
        }
    }

    // Destroys the gameObject (projectile) if it
    // leaves the camera view.
    //
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
