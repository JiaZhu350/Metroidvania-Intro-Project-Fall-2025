using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTongueGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public PlayerTongueRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] LayerMask grappableLayer;

    [Header("Main Camera:")]

    [SerializeField] Camera m_camera;

    private GameObject mainCamera;

    [Header("Transform Ref:")]

    public Transform player;
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;
    public PlayerMovement playerMovement;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [UnityEngine.Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;
    [SerializeField] private AudioClip grappleShootSound;
    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    public InputSystem_Actions actions;

    bool HasPerformed = false;

    public Vector2 mousePos;

    public RaycastHit2D hit;

    public float updatedVelocity;

    public Vector2 playerPosition;

    public Vector2 cameraPosition;
    public Vector2 mouseClickPosition;

    private Vector2 diff;

    private void Awake()
    {
        actions = new InputSystem_Actions();
    }
    public void OnEnable()
    {
        actions.Enable();
        actions.Player.Tongue.performed += GrappleAction;
        actions.Player.Tongue.canceled += GrappleAction;
    }
    public void OnDisable()
    {
        actions.Disable();
        actions.Player.Tongue.performed -= GrappleAction;
        actions.Player.Tongue.canceled -= GrappleAction;
    }
    void GrappleAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 gun = gunPivot.position;
            SoundEffectManager.Instance.PlaySoundFXClip(grappleShootSound, transform);
            mousePos = m_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            playerPosition = player.position;
            if(mainCamera == null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            if(mainCamera != null)
            {
                cameraPosition = mainCamera.GetComponent<CameraFollowScript>().transform.position;
            }
            if(cameraPosition != playerPosition)
            {
                diff = playerPosition - cameraPosition;
                mousePos = mousePos - diff;
            }
            mouseClickPosition = mousePos + playerPosition;
            hit = Physics2D.Raycast(firePoint.position, (mouseClickPosition - gun).normalized, maxDistnace, grappableLayer);
            Debug.DrawRay(firePoint.position, (mouseClickPosition - gun).normalized * maxDistnace, Color.red);
            HasPerformed = true;
            playerMovement.enabled = false;
        }
        if (context.canceled)
        {
            HasPerformed = false;
            playerMovement.enabled = true;
        }
    }
    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if(m_rigidbody.linearVelocityX != 0)
        {
            updatedVelocity = m_rigidbody.linearVelocityX;
        }
        if (HasPerformed)
        {
            SetGrapplePoint();
        }
        if (HasPerformed)
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (!HasPerformed)
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RotateGun(mousePos, true);
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        //Vector2 distanceVector = m_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gunPivot.position;
        if (hit)
        {
            //Debug.Log("Hit Something");
            //Debug.Log("Grapple Point Set To: " + hit.point);
            grapplePoint = hit.point;
            grappleDistanceVector = hit.point - (Vector2)gunPivot.position;
            grappleRope.enabled = true;
        }
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.linearVelocity = Vector2.zero;
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}
