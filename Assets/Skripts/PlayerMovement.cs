using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -29.43f;

    public Transform handPosition;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Camera cam;

    // Jumping
    [Header("Jump Settings")]
    public float jumpHeight = 3f;
    private bool _canDoubleJump;


    // Jump Booster
    [Header("Jump Booster Settings")]
    public LayerMask jumpBoosterMask;
    public float jumpBoosterVelocity = 10f;
    private bool _isOnJumpBooster;

    // Dashing
    [Header("Dash Settings")]
    public float dashTime = 0.2f;
    public float dashSpeed = 30f;
    public float dashCooldown = 2f;
    private bool _dashing;
    private float _remainingDashTime;
    private float _remainingDashCooldown;


    // Grappling Hook
    [Header("Grappling Hook Settings")]
    public LayerMask grapplingTargets;
    public float grapplingDistance = 40f;
    public float grapplingCooldown = 2f;
    public float grapplingSpeed = 35;
    public float grapplingBreakDistance = 1f;
    private float _currentGrapplingSpeed;
    private float _remainingGrapplingTime;
    private float _remainingGrapplingCooldown;
    private bool _isGrappling;
    private Vector3 grapplingTargetPosition;



    // Ladder
    [Header("Climb  Settings")]
    public LayerMask climbableSurface;
    public float climbSpeed = 8f;
    private bool isOnClimbableSurface;
    private bool isClimbing;

    // Bomb
    [Header("Bomb throwing Settings")]
    public GameObject bombPrefab;
    public float throwingPower;
    public float throwingCooldown = 10f;
    private float throwingTimer;

    private bool _isGrounded;
    Vector3 velocity;
    LineRenderer lineRenderer;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        Material lineMaterial = new Material(Shader.Find("Standard"));
        lineMaterial.color = new Color(0, 0, 0, 1);
        lineRenderer.material = lineMaterial;
        throwingTimer = throwingCooldown;
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Colliding");
    }
    void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        _isOnJumpBooster = Physics.CheckSphere(groundCheck.position, groundDistance, jumpBoosterMask);
        isOnClimbableSurface = Physics.CheckCapsule(transform.position - Vector3.up, transform.position + Vector3.up, 0.6f, climbableSurface);

        // Jump Booster
        if (_isOnJumpBooster)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * jumpBoosterVelocity * -2f * gravity);
            _canDoubleJump = true;
        }

        if (_isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        RaycastHit hit;
        // Climbing
        if (isOnClimbableSurface && Input.GetKey(KeyCode.Mouse1))
        {
            int climbDir = cam.transform.localRotation.x <= 0 ? 1 : -1;
            transform.position += Vector3.up * Input.GetAxis("Vertical") * climbSpeed * climbDir * Time.deltaTime;
            _canDoubleJump = true;
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        // Bomb throwing
        throwingTimer += Time.deltaTime;
        if (!isClimbing && Input.GetKeyDown(KeyCode.Mouse1) && throwingTimer >= throwingCooldown)
        {
            GameObject bomb = Instantiate(bombPrefab, handPosition.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwingPower, ForceMode.Impulse);
            throwingTimer = 0;
        }



        // Grappling Hook
        if (Input.GetMouseButtonDown(2) && !_isGrappling && _remainingGrapplingCooldown <= 0 && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, grapplingDistance, grapplingTargets))
        {
            grapplingTargetPosition = hit.point;
            lineRenderer.enabled = true;
            _remainingGrapplingCooldown = grapplingCooldown;
            _isGrappling = true;
            _remainingGrapplingTime = 0.1f * Vector3.Distance(hit.point, transform.position);
            _canDoubleJump = true;

        }
        else if (Input.GetMouseButtonDown(2) && _isGrappling)
        {
            _isGrappling = false;
            velocity.y = 0;
        }
        if (_isGrappling && _remainingGrapplingTime > 0 && Vector3.Distance(transform.position, grapplingTargetPosition) > grapplingBreakDistance)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _isGrappling = false;
                _canDoubleJump = true;
                velocity.y = 0;
            }
            _remainingGrapplingTime -= Time.deltaTime;
            controller.Move((grapplingTargetPosition - transform.position).normalized * Mathf.Lerp(_currentGrapplingSpeed, grapplingSpeed, 1f) * Time.deltaTime);
            lineRenderer.SetPositions(new Vector3[] { transform.position, grapplingTargetPosition });
            //velocity.y -= gravity * Time.deltaTime; // cancel out gravity
        }
        else
        {
            _remainingGrapplingCooldown -= Time.deltaTime;
            _isGrappling = false;
            lineRenderer.enabled = false;
        }

        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_dashing && _remainingDashCooldown <= 0)
        {
            _remainingDashCooldown = dashCooldown;
            _remainingDashTime = dashTime;
            _dashing = true;

        }
        if (_remainingDashTime > 0)
        {
            _remainingDashTime -= Time.deltaTime;
            controller.Move(move.normalized * dashSpeed * Time.deltaTime);
        }
        else
        {
            _remainingDashCooldown -= Time.deltaTime;
            _dashing = false;
        }






        // Jumping
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _canDoubleJump = true;


            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);

        } // Double jump
        else if (Input.GetButtonDown("Jump") && !_isGrounded && _canDoubleJump)
        {
            _canDoubleJump = false;
            velocity.y += Mathf.Sqrt(jumpHeight * 1.5f * -2f * gravity);
        }

        if (!isClimbing && !_isGrappling)
        {
            controller.Move(move * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }




    }
    void CancelGrappling()
    {

    }
    void applyGravity()
    {

    }

    void climbAction()
    {

    }
    void grapplingAction()
    {

    }
    void jumpAction()
    {

    }
    void walkAction()
    {

    }
}
