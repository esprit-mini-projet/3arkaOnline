using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : InventoryItemBase
{
    public int HealthPoints = 20;

    public override void OnUse()
    {
        GameManager.Instance.Player.Rehab(HealthPoints);

        Destroy(this.gameObject);
    }
}
