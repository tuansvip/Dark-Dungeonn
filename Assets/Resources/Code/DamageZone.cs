using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    float damageZoneCD;
    private void Awake()
    {
        damageZoneCD = 0;
    }
    private void FixedUpdate()
    {
        damageZoneCD -= Time.fixedDeltaTime;
        if (damageZoneCD < 0 ) damageZoneCD = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if ( damageZoneCD <= 0)
        {
            Player player = collision.GetComponent<Player>();
            player.health -= 5f;
            damageZoneCD = 1;
            SFXPlay.instance.PlayBurn();
        }
    }
}
