using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SheepCrowdScript : MonoBehaviour, WeaponInterface
{
    private float AttackDuration = 10;

    protected bool isAttacking;

    protected float attackStartTime;

    private Transform sheeps;


    // Start is called before the first frame update
    void Start()
    {
        sheeps = transform.Find("Sheeps");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3 * Time.deltaTime);
        }
    }

    public void Attack()
    {
        isAttacking = true;
        attackStartTime = Time.realtimeSinceStartup;
        for(int i = 1; i <= 9; i++)
        {
            sheeps.Find("Sheep" + i).GetComponent<AttackSheep>().isAttacking = true;
        }
    }

    public IEnumerator FinishAttack()
    {
        var dust = transform.Find("SheepsSpawnDust");
        var audio = dust.GetComponent<AudioSource>();
        var part = dust.GetComponent<ParticleSystem>();
        part.Play();
        audio.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        sheeps.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.7f);
        Destroy(transform.gameObject);
    }

    public int getDamage()
    {
        return 100;
    }
}
