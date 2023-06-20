using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public int routine;
    public float chronometer;
    public Animator anim;
    public Quaternion angle;
    public float grado;
    public GameObject target;
    public GameObject MenuWin;
    public bool fight = false;
    public int HP = 200;
    public int damageAxe = 40;
    public NavMeshAgent agent;
    public float radioVision;
    public float speed = 1.0f;
    public float angularSpeed = 120;
    public AudioClip hit;
    public AudioClip hitSword;
    private AudioSource audioSource;
    public MusicManager _musicManager;
    public CameraShakePlayer cameraShake;

    public void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        EnemyBehavior();

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            anim.SetBool("Punch", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch2"))
        {
            anim.SetBool("Punch", false);
            anim.SetBool("Punch2", false);
        }
    }

    public void EnemyBehavior()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > radioVision && !fight)
        {
            agent.enabled = false;
            anim.SetBool("Run", false);
            chronometer += 1 * Time.deltaTime;

            if (chronometer >= 4)
            {
                routine = Random.Range(0, 2);
                chronometer = 0;
            }

            switch (routine)
            {
                case 0:
                    anim.SetBool("Walk", false);
                    break;

                case 1:
                    grado = Random.Range(0, 360);
                    angle = Quaternion.Euler(0, grado, 0);
                    routine++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    anim.SetBool("Walk", true);
                    break;
            }
        }
        else
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            agent.enabled = true;
            agent.SetDestination(target.transform.position);
            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !fight)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
            }
            else if (Vector3.Distance(transform.position, target.transform.position) > 0.5)
            {
                fight = true;
                agent.enabled = false;
                agent.speed = 0;
                agent.angularSpeed = 0;
                anim.SetBool("Punch", true);
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                StopFight();

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
                {
                    anim.SetBool("Punch", false);
                    anim.SetBool("Punch2", true);
                }
            }
        }
    }
    public void StopFight()
    {
        fight = false;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Punch"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hit);
            HP -= FindObjectOfType<PlayerLogic>().damPunch;
            if (HP >= FindObjectOfType<PlayerLogic>().damPunch)
            {
                anim.CrossFadeInFixedTime("Reaction", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("Death", 0.1f);
                agent.isStopped = true;
                Destroy(gameObject, 5f);
                Invoke("Win", 3f);
            }
        }
        if (coll.CompareTag("Kick"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hit);
            HP -= FindObjectOfType<PlayerLogic>().damKick;
            if (HP >= FindObjectOfType<PlayerLogic>().damKick)
            {
                anim.CrossFadeInFixedTime("ReactionKick", 0.1f);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("Death", 0.1f);
                agent.isStopped = true;
                Destroy(gameObject, 5f);
                Invoke("Win", 3f);
            }
        }
        if (coll.CompareTag("Sword"))
        {
            StartCoroutine(cameraShake.Shake());
            audioSource.PlayOneShot(hitSword);
            HP -= FindObjectOfType<PlayerLogic>().damSword;
            if (HP >= FindObjectOfType<PlayerLogic>().damSword)
            {
                anim.CrossFadeInFixedTime("Reaction", 0.1f);
                transform.Translate(Vector3.forward * 0 * Time.deltaTime);
            }
            else if (HP <= 0)
            {
                anim.CrossFadeInFixedTime("Death", 0.1f);
                agent.isStopped = true;
                Destroy(gameObject, 5f);
                Invoke("Win", 3f);
            }
        }
            if (coll.CompareTag("Header"))
            {
                StartCoroutine(cameraShake.Shake());
                audioSource.PlayOneShot(hit);
                HP -= FindObjectOfType<PlayerLogic>().Damheader;
                if (HP >= FindObjectOfType<PlayerLogic>().Damheader)
                {
                    anim.CrossFadeInFixedTime("Reaction", 0.1f);
                    transform.Translate(Vector3.forward * 0 * Time.deltaTime);
                }
                else if (HP <= 0)
                {
                    anim.CrossFadeInFixedTime("Death", 0.1f);
                    agent.isStopped = true;
                    Invoke("Win", 3f);
                }
            }
    }
    public void Win()
    {
        MenuWin.SetActive(true);
        Time.timeScale = 0f;
        _musicManager.SwitchMusic(MenuWin);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
