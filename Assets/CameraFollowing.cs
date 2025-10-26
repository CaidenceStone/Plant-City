using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform Following;
    public Vector3 CameraDistanceAnchor;
    /// <summary>
    /// Every second, the Camera moves towards <see cref="CameraFollowingTarget"/> by this amount of units.
    /// </summary>
    public float AnchorFollowingSpeedPerSecond;
    /// <summary>
    /// Every second, the Camera's position is lerped towards <see cref="CameraFollowingTarget"/> by this percent.
    /// </summary>
    public float AnchorFollowingLerpPerFrame;

    /// <summary>
    /// Every second, <see cref="CurrentPlayerLookAtLocation"/> moves towards the player by this amount of units.
    /// </summary>
    public float LookAtPlayerSpeedPerSecond;
    /// <summary>
    /// Every second, <see cref="CurrentPlayerLookAtLocation"/> is lerped towards the player by this percent.
    /// </summary>
    public float LookAtPlayerLerpPerFrame;

    private Vector3 CurrentPlayerLookAtLocation;

    private Vector3 CameraFollowingTarget
    {
        get
        {
            return this.Following.position + this.CameraDistanceAnchor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.CurrentPlayerLookAtLocation = this.Following.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // CurrentPlayerLookAtLocation is a virtual location that is trying to closely track the player's actual location
        // It moves towards the player at LookAtPlayerSpeedPerSecond, as well as being lerped each frame by a flat amount
        this.CurrentPlayerLookAtLocation = Vector3.MoveTowards(this.CurrentPlayerLookAtLocation, this.Following.position, this.LookAtPlayerSpeedPerSecond * Time.deltaTime);
        this.CurrentPlayerLookAtLocation = Vector3.Lerp(this.CurrentPlayerLookAtLocation, this.Following.position, this.LookAtPlayerLerpPerFrame);

        // The camera tries to go towards an offset from this location, based on the CameraDistanceAnchor
        Vector3 newPosition = Vector3.MoveTowards(this.transform.position, this.CameraFollowingTarget, this.AnchorFollowingSpeedPerSecond * Time.deltaTime);
        newPosition = Vector3.Lerp(newPosition, this.CameraFollowingTarget, this.AnchorFollowingLerpPerFrame);

        this.transform.position = newPosition;
        transform.LookAt(CurrentPlayerLookAtLocation);
    }
}
