using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerController : MonoBehaviour
{
    [SerializeField]
    public HUD Hud;
    [SerializeField]
    public bool myTurn = true;
    [SerializeField]
    public GameObject ACooldownIndicator;
    [SerializeField]
    public GameObject BCooldownIndicator;
    [SerializeField]
    public GameObject YCooldownIndicator;

    protected int ACooldown = 100;
    protected int BCooldown = 100;
    protected int YCooldown = 100;

    public abstract IEnumerator AAttack();
    public abstract IEnumerator YAttack();
    public abstract IEnumerator BAttack();

}
