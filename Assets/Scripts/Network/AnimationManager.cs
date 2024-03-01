using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Network;
using Photon.Pun;
using UnityEngine;

public class AnimationManager : NetworkClass, IPunObservable
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] Movement movement;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform spine;
    
    private float xRotation;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player, send others our data
            xRotation = cameraHolder.localEulerAngles.x;
            stream.SendNext(xRotation);
        }
        else
        {
            // Network player, receive data
            xRotation = (float) stream.ReceiveNext();
        }
    }

    private void Update()
    {
        spine.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        anim.SetBool("isWalking", rb.velocity.magnitude > 0.1f);
        anim.SetBool("inAir", !movement.IsGrounded());
    }
}
