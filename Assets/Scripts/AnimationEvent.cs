using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayFootstep(float speedThreshhold)
    {
        float speed = _animator.GetFloat("Speed");
         
         //Walk is 5
        if (speedThreshhold == 5f)
        {
            if (speed <= 2.0f || speed > 5.1f)
            {
                return;
            }   
        }
        else if (speedThreshhold == 8.0f)  //run
        {
            if (speed <= 5.1f)
            {
                return;
            }
        }

        string ground = GetGround();
        string soundpath = "event:/SFX/Footstep_Grass";
        
        if (ground == "Grass")
        {
            soundpath = "event:/SFX/Footstep_Grass";
        }
        else if (ground == "Stone")
        {
            soundpath = "event:/SFX/Footstep_Stone";
        }
        
        
        RuntimeManager.PlayOneShot("event:/SFX/Footstep_Grass");
    }

    private string GetGround()
    {
        float distance = 1.0f;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        RaycastHit hit;
        Ray ray = new Ray(origin, Vector3.down);

        if (Physics.Raycast(ray, out hit, distance));
        else
        {
            return "";
        }
        {
            return hit.collider.tag;
        }
    }
}
