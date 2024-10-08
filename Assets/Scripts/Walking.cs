using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    private Animator anim;

    private bool isWalking;

    public bool move;

    private bool idle;

    public float speed = 1.0F;

    public int TicksPerSecond = 60;

    private float timer;

    private float random;

    private bool canRotate = true;

    public bool canTurnAround = true;

    private float animationTime;

    public bool colliderFront;

    private GameObject player;

    public bool lookAtPlayer;

    public GameObject nextEvolutionStep;

    public bool canWalk = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        lookAtPlayer = false;
        isWalking = true;
        timer = 0f;
        move = false;
        idle = false;
        random = 0f;
        colliderFront = false;
        animationTime = 2f;
        player = GameObject.Find("Player");
        if (canWalk) startWalking();

        //Debug.Log(Mathf.Atan2(0f, 8f) * Mathf.Rad2Deg);
        float time = TimeManager.hourAquivalence * 24f;
        //TimedAction.Create(grow, time, false, "Grow");
    }

    void FixedUpdate()
    {
        if (canWalk)
        {
            if (lookAtPlayer)
            {
                //transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                if (
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >
                    0.90f &&
                    move == true
                )
                {
                    move = false;

                    startWalking();
                }

                if (/*transform.GetChild(0).GetComponent<AnimalCollision>().hitsPlayer == true &&*/
                    move == true)
                {
                    move = false;
                }
                else /*if (transform.GetChild(0).GetComponent<AnimalCollision>().hitsPlayer == false
                )*/
                {
                    move = true;
                }
            }

            if (move == true)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            if (idle == false && isWalking == true)
            {
                if (
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >
                    0.90f &&
                    move == true &&
                    lookAtPlayer == false
                )
                {
                    move = false;
                    StartCoroutine(changeDirection());
                }
                else if (
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >
                    0.50f &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.90f
                )
                {
                    move = true;
                }
                else if (
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >
                    0.35f &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.50f
                )
                {
                    move = false;
                }
            }
            else if (idle == true && isWalking == false)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
                {
                    idle = false;
                    StartCoroutine(changeDirection());
                }
            }
        }else{
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.5f)
                {
                    anim.Play("Base Layer.Idle" ,  -1, 0f);
                }
        }
    }

    public void startWalking()
    {
        anim.Play("Base Layer.Walk", 0);
        anim.speed = 1f;
        isWalking = true;
        move = true;
        idle = false;
        canRotate = true;
    }

    public void startIdle()
    {
        anim.Play("Base Layer.Idle", 0);
        anim.speed = 1f;
        idle = true;
        move = false;
        isWalking = false;
        canRotate = true;
    }

    public void IdleWhileMilking()
    {
        anim.Play("Base Layer.Idle", 0);
        anim.speed = 0.6f;
        idle = false;
        move = false;
        isWalking = false;
        canRotate = false;
    }

    public void disableMovement()
    {
        move = false;
        idle = false;
        isWalking = false;
        canRotate = false;
    }

    public IEnumerator lerpToRotation()
    {
        if (canTurnAround)
        {
            canTurnAround = false;
            move = false;
            float elapsedTime = 0;
            Quaternion start = transform.rotation;

            Vector3 endRot = transform.rotation.eulerAngles;
            endRot = new Vector3(-endRot.x, endRot.y + 180f, -endRot.z);
            Quaternion end = Quaternion.Euler(endRot);

            while (elapsedTime < animationTime)
            {
                float time = elapsedTime / animationTime;
                transform.rotation = Quaternion.Slerp(start, end, time);
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            transform.rotation = end;
            transform.position += new Vector3(0f, 0.05f, 0f);
            float timee = TimeManager.minuteAquivalence * 4f;
            //TimedAction.Create(RotationCooldown, timee, false, "RotationCooldown");
            yield return null;
        }
    }

    private void RotationCooldown()
    {
        canTurnAround = true;
    }

    private IEnumerator changeDirection()
    {
        if (canRotate)
        {
            canRotate = false;
            move = false;
            isWalking = false;
            idle = false;
            timer = 0f;
            random = Random.Range(5f, 60f);
            WaitForSeconds wait = new WaitForSeconds(1f / TicksPerSecond);

            if (
                random < 40 //Rund 70% eintrittswahrscheinlichkeit sich zu drehen
            )
            {
                while (true)
                {
                    if (
                        timer > random //60f    Hier Random sp�ter
                    )
                    {
                        startWalking();
                        break;
                    }
                    else if ((int) random % 2 == 0)
                    {
                        transform.Rotate(0, 1, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -1, 0);
                    }

                    timer++;

                    yield return wait;
                }
            }
            else
            {
                startIdle();
            }
        }
        else
        {
            startIdle();
        }
    }

    IEnumerator lookPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / TicksPerSecond);

        float xrot = -360f + transform.localEulerAngles.x;
        float zrot = -360f + transform.localEulerAngles.z;
        //Debug.Log (xrot);

        while (lookAtPlayer)
        {
            xrot = -360f + transform.localEulerAngles.x;
            zrot = -360f + transform.localEulerAngles.z;

            transform
                .LookAt(new Vector3(player.transform.position.x,
                    transform.position.y,
                    player.transform.position.z));

            Vector3 endRot = transform.rotation.eulerAngles;
            endRot = new Vector3(xrot, endRot.y, zrot);
            Quaternion end = Quaternion.Euler(endRot);

            transform.rotation = end;

            yield return wait;
        }
    }

    public void followPlayer()
    {
        //checken ob rechte Hand leer ist

        if (
            player != null &&
            player.transform.GetChild(0).GetChild(1).childCount < 1
        )
        {
            if (
                (
                gameObject.GetComponent<Cow>() != null &&
                gameObject.GetComponent<Cow>().snapon == false
                ) ||
                gameObject.GetComponent<Cow>() == null
            )
            {
                //                Debug.Log("FollowPlayer");
                if (lookAtPlayer == false)
                {
                    speed = 1.5f;
                    lookAtPlayer = true;
                    StartCoroutine(lookPlayer());
                    startWalking();
                }
                else if (lookAtPlayer == true)
                {
                    speed = 1.0f;
                    lookAtPlayer = false;
                    changeDirection();
                }
            }
        }
    }

    void grow()
    {
        if (nextEvolutionStep != null)
        {
            Instantiate(nextEvolutionStep,
            transform.position,
            transform.rotation);

            //Play Particle Effect
            Destroy (gameObject);
        }
    }

    public bool getIdleState()
    {
        return idle;
    }
}
