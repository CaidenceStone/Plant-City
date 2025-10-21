using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody CharacterBody;
    public InputActionAsset MyInputActionMap;

    [Range(0, 2f)]
    public float TimeBetweenJumps;

    private float? curTimeBeforeNextJump { get; set; } = null;
    public bool CanJump
    {
        get
        {
            return !this.curTimeBeforeNextJump.HasValue;
        }
    }

    [Range(0, 25f)]
    public float JumpForce;

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

        //if (Input.GetAxis("Horizontal"))
        //{
        //    //
        //}
        //if (Input.GetAxis("Vertical"))
        //{
        //    //
        //}
        if (this.CanJump && this.MyInputActionMap.FindActionMap("Platforming").FindAction("Jump").WasPressedThisFrame())
        {
            this.CharacterBody.AddForce(Vector3.up * this.JumpForce, ForceMode.Impulse);
            this.curTimeBeforeNextJump = this.TimeBetweenJumps;
        }
    }
}
