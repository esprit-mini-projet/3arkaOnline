using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HajPlayerController : AbstractPlayerController, PlayerInterface
{
    #region Private Members

    private Animator _animator;

    private CharacterController _characterController;

    private float Gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;

    private InventoryItemBase mCurrentItem = null;

    private HealthBar mHealthBar;

    private int startHealth;

    private bool isAttacking = false;

    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    public GameObject Hand;

    public float JumpSpeed = 7.0f;

    public GameObject spawnEffectDust;

    public GameObject Chicha;

    public GameObject ChichaSmokeAttack;

    public GameObject SheepCrowd;

    public GameObject Okez;

    public GameObject PlayerName;

    public GameObject PlayerHealthBar;

    public GameObject HeathCanvas;

    public bool isCharPlayer = true;

    public GameObject ChichaSmokeCollider;

    #endregion

    // Use this for initialization
    void Start()
    {
        Hud = GameObject.FindGameObjectWithTag("hud").GetComponent<HUD>();
        ACooldownIndicator = GameObject.FindGameObjectWithTag("ai");
        BCooldownIndicator = GameObject.FindGameObjectWithTag("bi");
        YCooldownIndicator = GameObject.FindGameObjectWithTag("yi");

        HeathCanvas.SetActive(isCharPlayer);

        Chicha.SetActive(false);
        Okez.SetActive(false);
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Min = 0;
        mHealthBar.Max = Health;
        startHealth = Health;
        mHealthBar.SetValue(Health);
        PlayerHealthBar.GetComponent<Image>().fillAmount = Health / 100.0f;

        InvokeRepeating("UpdateCooldown", 2f, 2f);
    }

    #region Inventory

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);
        goItem.transform.parent = null;

    }

    private void SetItemActive(InventoryItemBase item, bool active)
    {
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
        currentItem.transform.parent = active ? Hand.transform : null;
    }

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (e.Item.ItemType != EItemType.Consumable)
        {
            // If the player carries an item, un-use it (remove from player's hand)
            if (mCurrentItem != null)
            {
                SetItemActive(mCurrentItem, false);
            }

            InventoryItemBase item = e.Item;

            // Use item (put it to hand of the player)
            SetItemActive(item, true);

            mCurrentItem = e.Item;
        }

    }

    private int Attack_1_Hash = Animator.StringToHash("Base Layer.Attack_1");

    #endregion

    #region Health & Hunger

    [Tooltip("Amount of health")]
    public int Health = 100;

    public bool IsDead
    {
        get
        {
            return Health == 0;
        }
    }

    public void Rehab(int amount)
    {
        Health += amount;
        if (Health > startHealth)
        {
            Health = startHealth;
        }

        mHealthBar.SetValue(Health);
    }


    private bool deadAnimated = false;
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;

        mHealthBar.SetValue(Health);

        if (IsDead && !deadAnimated)
        {
            _animator.SetTrigger("death");
            deadAnimated = true;
            GetComponent<AudioSource>().Play();
        }

    }

    #endregion

    private bool mIsControlEnabled = true;

    public void EnableControl()
    {
        mIsControlEnabled = true;
    }

    public void DisableControl()
    {
        mIsControlEnabled = false;
    }

    private void UpdateCooldown()
    {
        if(ACooldown < 100)
        {
            ACooldown = Mathf.Min(100, ACooldown + 7);
        }
        if(YCooldown < 100)
        {
            YCooldown = Mathf.Min(100, YCooldown + 5);
        }
        if(BCooldown < 100)
        {
            BCooldown = Mathf.Min(100, BCooldown + 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ACooldownIndicator.GetComponent<Image>().fillAmount = ACooldown / 100.0f;
        YCooldownIndicator.GetComponent<Image>().fillAmount = YCooldown / 100.0f;
        BCooldownIndicator.GetComponent<Image>().fillAmount = BCooldown / 100.0f;

        PlayerHealthBar.GetComponent<Image>().fillAmount = Health / 100.0f;

        if (!IsDead && mIsControlEnabled && !isAttacking)
        {
            if(!myTurn)
            {
                _animator.SetBool("is_in_air", false);
                _animator.SetBool("run", false);
                transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            }

            // Get Input for axis
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            if (myTurn && GetComponent<PlayerConnection>().isLocalPlayer)
            {
                if (_characterController.isGrounded)
                {
                    _moveDirection = transform.forward * move.magnitude;

                    _moveDirection *= Speed;

                    var jumpButtonPressed = Input.GetButtonDown("Jump");

                    if (jumpButtonPressed && mInteractItem != null)
                    {
                        // Interact animation
                        mInteractItem.OnInteractAnimation(_animator);
                    }
                    else if (jumpButtonPressed)
                    {
                        _animator.SetBool("is_in_air", true);
                        _moveDirection.y = JumpSpeed;
                    }
                    else
                    {
                        _animator.SetBool("is_in_air", false);
                        _animator.SetBool("run", move.magnitude > 0);
                    }
                }

                if (Input.GetButtonDown("Mouse X") && YCooldown == 100)
                {
                    isAttacking = true;
                    StartCoroutine(YAttack());
                }
                else if (Input.GetButtonDown("Fire3") && BCooldown == 100)
                {
                    isAttacking = true;
                    StartCoroutine(BAttack());
                }
                else if (Input.GetButtonDown("Cancel") && ACooldown == 100)
                {
                    isAttacking = true;
                    StartCoroutine(AAttack());
                }

                // Get Euler angles
                float turnAmount = Mathf.Atan2(move.x, move.z);
                _moveDirection.y -= Gravity * Time.deltaTime;

                transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);
                
                _characterController.Move(_moveDirection * Time.deltaTime);
            }
        }
    }
    
    //Chicha
    public override IEnumerator YAttack()
    {
        var spanDustObject = Instantiate(spawnEffectDust, Chicha.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.3f);
        Chicha.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        var soundPlayer = Chicha.GetComponent<AudioSource>();
        soundPlayer.Play();
        yield return new WaitForSecondsRealtime(2.0f);
        ChichaSmokeAttack.GetComponent<ParticleSystem>().Play();
        ChichaSmokeAttack.GetComponent<AudioSource>().Play();

        var collider = Instantiate(ChichaSmokeCollider, ChichaSmokeAttack.transform.position, ChichaSmokeAttack.transform.rotation);
        collider.transform.parent = ChichaSmokeAttack.transform;
        var locPos = collider.transform.localPosition;
        collider.transform.localPosition = new Vector3(locPos.x, locPos.y, 1.89f);

        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(collider);
        spanDustObject.GetComponent<ParticleSystem>().Play();
        spanDustObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(0.5f);
        Chicha.SetActive(false);
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(spanDustObject);
        isAttacking = false;
        YCooldown = 0;
    }

    //Sheeps
    public override IEnumerator BAttack()
    {
        var crowd = Instantiate(SheepCrowd, transform.position, transform.rotation);
        crowd.transform.parent = transform;
        var crowdScript = crowd.GetComponent<SheepCrowdScript>();
        Okez.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _animator.SetTrigger("attack_1");
        yield return new WaitForSecondsRealtime(0.5f);
        crowdScript.Attack();
        yield return new WaitForSecondsRealtime(5f);
        StartCoroutine(crowdScript.FinishAttack());
        Okez.SetActive(false);
        isAttacking = false;
        BCooldown = 0;
    }

    //Zollat
    public override IEnumerator AAttack()
    {
        var spanDustObject = Instantiate(spawnEffectDust, Okez.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.3f);
        Okez.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _animator.SetTrigger("attack_1");
        yield return new WaitForSecondsRealtime(0.5f);
        spanDustObject.GetComponent<ParticleSystem>().Play();
        spanDustObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(0.5f);
        Okez.SetActive(false);
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(spanDustObject);
        isAttacking = false;
        ACooldown = 0;
    }

    public void InteractWithItem()
    {
        if (mInteractItem != null)
        {
            mInteractItem.OnInteract();

            if (mInteractItem is InventoryItemBase)
            {
                InventoryItemBase inventoryItem = mInteractItem as InventoryItemBase;
                inventoryItem.OnPickup();

                if (inventoryItem.UseItemAfterPickup)
                {
                    Health = Mathf.Clamp(Health + inventoryItem.GetComponent<FirstAid>().HealthPoints, 0, 100);
                    mHealthBar.SetValue(Health);
                }
            }
        }

        Hud.CloseMessagePanel();

        mInteractItem = null;
    }

    private InteractableItemBase mInteractItem = null;

    private void OnTriggerEnter(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();

        if (item != null)
        {
            if (item.CanInteract(other))
            {
                print(item);
                mInteractItem = item;
                print(mInteractItem);

                Hud.OpenMessagePanel(mInteractItem);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            Hud.CloseMessagePanel();
            mInteractItem = null;
        }
    }

    public int getHealth()
    {
        return Health;
    }

    public void takeDamage(int damage)
    {
        TakeDamage(damage);
    }
}
