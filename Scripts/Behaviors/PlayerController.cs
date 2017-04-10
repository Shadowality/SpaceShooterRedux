using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Fields

    public float gamespaceClampX;
    public float gamespaceClampY;
    public float laserOffsetX;
    public float laserOffsetY;
    public float laserSpeed;
    public float padding = 1;
    public GameObject laserChargePrefab;
    public GameObject laserPrefab;
    public static float fireRate = .25f;
    public static float speed = 10;
    public static PlayerController Instance;
    public VirtualJoystick joystick;

    #endregion Public Fields

    #region Private Fields

    private float lastShot;
    private float maxClampX;
    private float maxClampY;
    private float minClampX;
    private float minClampY;
    private Rigidbody2D myBody;

    #endregion Private Fields

    #region Public Delegates

    public delegate void HealthUpdateEvent(int amount);

    #endregion Public Delegates

    #region Public Events

    public static event HealthUpdateEvent HealthUpdate;

    #endregion Public Events

    #region Public Properties

    public static float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    public static float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    #endregion Public Properties

    #region Public Methods

    // TODO
    public void Charge()
    {
        if (Time.time > fireRate + lastShot)
        {
            GameObject laser = ObjectPoolerManager.Instance.GetPooledObject(laserChargePrefab.name);
            laser.SetActive(true);
            laserOffsetX *= -1;
            laser.transform.position = new Vector2(transform.GetChild(0).position.x - laserOffsetX, transform.GetChild(0).position.y - laserOffsetY);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            lastShot = Time.time;
        }
    }

    public void Shoot()
    {
        if (Time.time > fireRate + lastShot)
        {
            GameObject laser = ObjectPoolerManager.Instance.GetPooledObject(laserPrefab.name);
            laser.SetActive(true);
            laserOffsetX *= -1;
            laser.transform.position = new Vector2(transform.GetChild(0).position.x - laserOffsetX, transform.GetChild(0).position.y - laserOffsetY);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            lastShot = Time.time;
        }
    }

    #endregion Public Methods

    #region Private Methods

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 movement;

        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
            movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
            movement = new Vector3(joystick.Horizontal(), joystick.Vertical());

        //print(movement);
        //if (movement.x > 0) movement.x = 1;
        //else if (movement.x < 0) movement.x = -1;
        //if (movement.y > 0) movement.y = 1;
        //else if (movement.y < 0) movement.y = -1;

        myBody.velocity = movement * Speed;

        float myBodyPosX = Mathf.Clamp(myBody.position.x, minClampX, maxClampX);
        float myBodyPosY = Mathf.Clamp(myBody.position.y, minClampY, maxClampY);

        myBody.position = new Vector3(myBodyPosX, myBodyPosY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Player has been hit");
            HealthUpdate(-1);
        }
    }

    // Use this for initialization
    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

        Camera cam = Camera.main;
        float distance = transform.position.z - cam.transform.position.z;
        minClampX = cam.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + padding + gamespaceClampX;
        maxClampX = cam.ViewportToWorldPoint(new Vector3(1, 0, distance)).x - padding - gamespaceClampX;
        minClampY = cam.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + padding + gamespaceClampY;
        maxClampY = cam.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - padding - gamespaceClampY;
    }

    // Update is called once per frame
    private void Update()
    {
        // For debug.
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    #endregion Private Methods
}