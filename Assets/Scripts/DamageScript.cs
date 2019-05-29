using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Entered");

        if(other.transform.tag == "Weapon")
        {
            WeaponInterface weapon = other.gameObject.GetComponent(typeof(WeaponInterface)) as WeaponInterface;
            if(weapon != null)
            {
                var playerIntercae = transform.parent.GetComponent(typeof(PlayerInterface)) as PlayerInterface;
                if(playerIntercae != null)
                {
                    playerIntercae.takeDamage(weapon.getDamage());
                }
            }
        }
    }
}
