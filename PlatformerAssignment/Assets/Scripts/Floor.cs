using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    PhysicsMaterial2D floorMaterial;
    private static float s_desiredFriction = 0.4f;

    // The purpose of this class is to make movement as smooth as possible by manipulating floor friction

    // While the player is sliding along the floor, we want it to have friction so the player comes to a stop (unless they're inputting more movement)
    // However, when the player lands on the floor, we want a friction of 0, because otherwise the player loses speed

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
