using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody CharacterBody;
    public InputActionAsset MyInputActionMap;

    [Range(0, .6f)]
    public float TimeBetweenJumps;

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

        Vector3 movementInputForce = new Vector3(this.MyInputActionMap.FindActionMap("Platforming").FindAction("Horizontal").ReadValue<float>(),
            0,
            this.MyInputActionMap.FindActionMap("Platforming").FindAction("Vertical").ReadValue<float>())
            * this.MovementForce;

        this.CharacterBody.AddForce(movementInputForce, ForceMode.Force);
        

        if (this.CanJump && this.MyInputActionMap.FindActionMap("Platforming").FindAction("Jump").WasPressedThisFrame())
        {
            this.CharacterBody.AddForce(Vector3.up * this.JumpForce, ForceMode.Impulse);
            this.curTimeBeforeNextJump = this.TimeBetweenJumps;
        }
    }
}
