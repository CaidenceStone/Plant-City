using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody CharacterBody;
    public InputActionAsset MyInputActionMap;
    public GameObject SeedPF;

    [Range(0, .6f)]
    public float TimeBetweenJumps;

    public float DistanceForMovementForcesFromCenter = .5f;
    public float DistanceAboveCenterMovementForcesFromCenter = .5f;

    private float? curTimeBeforeNextJump { get; set; } = null;
    public bool CanJump
    {
        get
        {
            return !this.curTimeBeforeNextJump.HasValue;
        }
    }

    [Range(0, 2500f)]
    public float JumpForce;

    [Range(0, 1000f)]
    public float MovementForce;

    public LayerMask GroundMask;

    private Vector3 LocalVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        this.MyInputActionMap.FindActionMap("Platforming").Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (this.curTimeBeforeNextJump.HasValue)
        {
            this.curTimeBeforeNextJump -= Time.deltaTime;
            if (this.curTimeBeforeNextJump.Value <= 0)
            {
                this.curTimeBeforeNextJump = null;
            }
        }

        this.HandleMovementInput();        

        if (this.CanJump && this.MyInputActionMap.FindActionMap("Platforming").FindAction("Jump").WasPressedThisFrame())
        {
            this.CharacterBody.AddForce(Vector3.up * this.JumpForce, ForceMode.Impulse);
            this.curTimeBeforeNextJump = this.TimeBetweenJumps;
        }

        if (this.MyInputActionMap.FindActionMap("Platforming").FindAction("Plant Seed").WasPressedThisFrame())
        {
            this.PlantSeed();
        }
    }

    void HandleMovementInput()
    {

        if (!this.MyInputActionMap.FindActionMap("Platforming").FindAction("Horizontal").IsInProgress()
            && !this.MyInputActionMap.FindActionMap("Platforming").FindAction("Vertical").IsInProgress())
        {
            return;
        }

        Vector3 movementInputForce = new Vector3(this.MyInputActionMap.FindActionMap("Platforming").FindAction("Horizontal").ReadValue<float>(),
            0,
            this.MyInputActionMap.FindActionMap("Platforming").FindAction("Vertical").ReadValue<float>())
            * this.MovementForce;

        Vector3 movementInputForcePosition = movementInputForce.normalized * this.DistanceForMovementForcesFromCenter + Vector3.up * DistanceAboveCenterMovementForcesFromCenter;

        this.CharacterBody.AddForceAtPosition(movementInputForce, this.CharacterBody.transform.position + movementInputForcePosition, ForceMode.Force);
    }

    void PlantSeed()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(this.MyInputActionMap.FindActionMap("Platforming").FindAction("Mouse Position").ReadValue<Vector2>()), out hit, float.MaxValue, GroundMask))
        {
            Debug.Log("Can't plant seed, I don't see anything");
            return;
        }

        Debug.Log($"Planting seed at {hit.point} with normal {hit.normal}!");
        Instantiate(SeedPF, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
    }
}
