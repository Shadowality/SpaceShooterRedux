using UnityEngine;

public class Shield : PowerUp
{
    #region Public Fields

    public float lifetime;
    public Sprite spriteActivated;
    public string clipDeactivate;

    #endregion Public Fields

    #region Private Fields

    private CircleCollider2D circleCollider;
    private int originalLayer;
    private static bool isActive;
    private Transform originalParent;

    #endregion Private Fields

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();
        originalParent = transform.parent;
        originalLayer = gameObject.layer;
        circleCollider = GetComponent<CircleCollider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive && collision.tag == "Player")
        {
            Activate(collision);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void Activate(Collider2D collision)
    {
        isActive = true;
        animator.SetBool("isActive", isActive);
        print("Activate " + name);

        AudioManager.Instance.GetSFXAudioSource(clipActivate).Play();

        myBody.velocity = Vector2.zero;
        spriteRenderer.sprite = spriteActivated;

        boxCollider.enabled = false;
        circleCollider.enabled = true;

        // Same layer for collisions.
        gameObject.layer = collision.gameObject.layer;

        transform.position = collision.transform.position;
        transform.parent = collision.transform;

        Invoke("Deactivate", lifetime);
    }

    private void Deactivate()
    {
        isActive = false;
        animator.SetBool("isActive", isActive);

        AudioManager.Instance.GetSFXAudioSource(clipDeactivate).Play();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Init();

        transform.parent = originalParent;
        gameObject.layer = originalLayer;

        boxCollider.enabled = true;
        circleCollider.enabled = false;
    }

    #endregion Private Methods
}