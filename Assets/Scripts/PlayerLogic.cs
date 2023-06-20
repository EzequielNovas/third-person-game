using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    public float movSpeed = 5.0f;
    public float rotSpeed = 5.0f;
    public float fuerzaDeSalto = 8f;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public Animator anim;
    public Rigidbody rb;
    public float x, y;
    public bool Jump;
    public bool attacking;
    public int noOflicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    public int HP = 100;
    public float currentHP;
    public int damPunch = 10;
    public int Damheader = 5;
    public int damKick = 15;
    public int damSword = 25;
    public bool sword;
    public MenuPause menuPause;
    public AudioClip hit;
    public AudioClip hitSword;
    public AudioClip hitAxe;
    private AudioSource audioSource;
    public Image imageHP;
    public GameObject menuDead;
    public MusicManager _musicManager;
    public CameraShakeEnemy cameraShake;

    void Start()
    {
        currentHP = HP;
        Jump = false;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FixedUpdate()
    {
        if (!attacking)
        {
            transform.Rotate(0, x * Time.deltaTime * rotSpeed, 0);
            transform.Translate(0, 0, y * Time.deltaTime * movSpeed);
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOflicks = 0;
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetButton("Fire1"))
            {
                OnClick();
            }
        }
    }
    void Update()
    {
        imageHP.fillAmount = HP / currentHP;
        Fists();
        Sword();
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        if (Jump)
        {
            if (!attacking)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetBool("Jump", true);
                    rb.AddForce(new Vector3(0, fuerzaDeSalto, 0), ForceMode.Impulse);
                }

            }
            anim.SetBool("TouchingGround", true);
        }
        else
        {
            FallingDown();
        }
        if (Input.GetKeyDown(KeyCode.F) && Jump && !attacking)
        {
            anim.SetTrigger("Kicking");
            attacking = true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && Jump && !attacking)
        {
            anim.SetTrigger("Headbutt");
            attacking = true;
        }
    }
    public void FallingDown()
    {
        anim.SetBool("Jump", false);
        anim.SetBool("TouchingGround", false);
    }

    private void OnTriggerStay(Collider other) => Jump = true;
    private void OnTriggerExit(Collider other) => Jump = false;

    public void StopHitting() => attacking = false;

    public void OnClick()
    {
        menuPause.juegoPausado = false;
        lastClickedTime = Time.time;
        noOflicks++;

        if (sword)
        {
            if (noOflicks == 1)
            {
                anim.CrossFadeInFixedTime("SwordHit1", 0.1f);
            }
            noOflicks = Mathf.Clamp(noOflicks, 0, 4);

            if (noOflicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit1"))
            {
                anim.SetBool("SwordHit1", false);
                anim.SetBool("SwordHit2", true);
            }
            if (noOflicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit2"))
            {
                anim.SetBool("SwordHit2", false);
                anim.SetBool("SwordHit3", true);
            }
            if (noOflicks >= 4 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit3"))
            {
                anim.SetBool("SwordHit3", false);
                anim.SetBool("SwordHit4", true);
            }
        }
        else
        {
                if (noOflicks == 1)
                {
                    anim.CrossFadeInFixedTime("Hit1", 0.1f);
                }
                noOflicks = Mathf.Clamp(noOflicks, 0, 4);

                if (noOflicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
                {
                    anim.SetBool("Hit1", false);
                    anim.SetBool("Hit2", true);
                }
                if (noOflicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
                {
                    anim.SetBool("Hit2", false);
                    anim.SetBool("Hit3", true);
                }
                if (noOflicks >= 4 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit3"))
                {
                    anim.SetBool("Hit3", false);
                    anim.SetBool("Hit4", true);
                }
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Punch"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hit);
            HP -= FindObjectOfType<EnemyLogic>().damageP;
            if (HP >= FindObjectOfType<EnemyLogic>().damageP)
            {
                anim.CrossFadeInFixedTime("Reaction", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("DeadForPunch", 0.1f);
                menuDead.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _musicManager.SwitchMusic(menuDead);
            }
        }
        if (coll.CompareTag("Kick"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hit);
            HP -= FindObjectOfType<EnemyLogic>().damageK;
            if (HP >= FindObjectOfType<EnemyLogic>().damageK)
            {
                anim.CrossFadeInFixedTime("ReactionKick", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("DeadForKick", 0.1f);
                menuDead.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _musicManager.SwitchMusic(menuDead);
            }
        }
        if (coll.CompareTag("Kick2"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hit);
            HP -= FindObjectOfType<EnemyAvancedLogic>().damageK;
            if (HP >= FindObjectOfType<EnemyAvancedLogic>().damageK)
            {
                anim.CrossFadeInFixedTime("ReactionKick", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("DeadForKick", 0.1f);
                menuDead.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _musicManager.SwitchMusic(menuDead);
            }
        }
        if (coll.CompareTag("SwordEnemy"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hitSword);
            HP -= FindObjectOfType<EnemyAvancedLogic>().damageSword;
            if (HP >= FindObjectOfType<EnemyAvancedLogic>().damageSword)
            {
                anim.CrossFadeInFixedTime("Reaction", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("DeadForPunch", 0.1f);
                menuDead.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _musicManager.SwitchMusic(menuDead);
            }
        }
        if (coll.CompareTag("Axe"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hitAxe);
            HP -= FindObjectOfType<Boss>().damageAxe;
            if (HP >= FindObjectOfType<Boss>().damageAxe)
            {
                anim.CrossFadeInFixedTime("Reaction", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("DeadForPunch", 0.1f);
                menuDead.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _musicManager.SwitchMusic(menuDead);
            }
        }
    }
    public void Fists()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
        {
            anim.SetBool("Hit1", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
        {
            anim.SetBool("Hit1", false);
            anim.SetBool("Hit2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit3"))
        {
            anim.SetBool("Hit1", false);
            anim.SetBool("Hit3", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit4"))
        {
            anim.SetBool("Hit4", false);
            anim.SetBool("Hit1", false);
            noOflicks = 0;
        }
    }

    public void Sword()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit1"))
        {
            anim.SetBool("SwordHit1", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit2"))
        {
            anim.SetBool("SwordHit1", false);
            anim.SetBool("SwordHit2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit3"))
        {
            anim.SetBool("SwordHit1", false);
            anim.SetBool("SwordHit3", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("SwordHit4"))
        {
            anim.SetBool("SwordHit1", false);
            anim.SetBool("SwordHit4", false);
            noOflicks = 0;
        }
    }
}
