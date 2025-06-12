using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // private PlayerHealth thePlayerHealth;

    GameObject player;
    public bool patrol = true, gaurd = false, clockwise = false;
    public bool moving = true;
    public bool pursuingPlayer = false, goingToLastLoc = false;
    float distanceToTargetDeLTA = 0.0f;
    Vector3 target;
    Rigidbody2D rid;
    GameObject gm;

    public Vector3 playerLastPos;
    RaycastHit2D hit;
    float speed = 2.0f;
    int layerMask = 1 << 8;

    GameObject lastPos;
    public GameObject lastPosMarker;

    //ObjectManager obj;
    //UIController uc;
    GameObject[] weapons;
    //  EnemyWeaponController ewc;
    public GameObject weaponToGoTo;
    public bool goingToWeapon = false;
    public bool hasGun = false;

    public float Speed = 0f;
    public float ChaseRange = 0f;
    public float fPatrolResetTime = 1.0f;

    public bool bOPatorlOtherDir = false;
    public bool bOPatorlOtherDirStopRoom = false;



    public bool bWallCollision = false;
    public float fWallCollisionTime = 2.0f;

    public bool ballPreviousePosFind = false;
    public float fWallPreviousePosFindTime = 2.0f;

    private Vector3 EnemyPreviousePos;
    public bool bStun = false;
    public float fStunTime = 2.0f;



    //  AIPathFind aip;
    public int iRandomDir = 0;
    public Vector2 deltaPos;
    public Vector2 curPos;

    public bool botherdiredelta = false;
    public float fDeltaTimer = 2.0f;
    public float fDeltaTimerReset = 0.5f;

    public bool bstop;
    public float stopTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLastPos = this.transform.position;
        // obj = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        //    ewc = this.GetComponent<EnemyWeaponController>();
        rid = this.GetComponent<Rigidbody2D>();
        layerMask = ~layerMask;
        //uc = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        // thePlayerHealth = FindObjectOfType<PlayerHealth>();

        //gm = new GameObject();
        //gm.transform.parent = this.transform;
        //gm.AddComponent<SpriteRenderer>();
        //Color color;
        //color.r = 255.0f;
        //color.g = 0.0f;
        //color.b = 0.0f;
        //color.a = 255.0f;
        //gm.GetComponent<SpriteRenderer>().color = color;
        //gm.GetComponent<SpriteRenderer>().sortingOrder = 5;
        //layerMask = ~layerMask;
        //gm.transform.localScale = this.transform.localScale * 7;

        //gm.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
        //gm.layer = 8;



    }

    // Update is called once per frame
    void Update()
    {

        stopFunc();
        deltaPosFunc();
        if (/*PlayerHealth.dead == false &&*/ bStun == false)
        {
            //movement();
            // playerDetect();

            // canEnemyFindWeapon();
            if (!bstop)
                AImovement();
            //포지션

        }
        else
        {
            //   this.GetComponent<EnemyAnimate>().enabled = false;
        }
        if (bStun)
        {

            //StunFunc();
        }

    }


    void AImovement()
    {
        // 패트롤->플레이어발견->플레이어거리->다시패트롤->
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        Vector3 dir = player.transform.position - transform.position;

        Vector3 pos = this.transform.InverseTransformPoint(player.transform.position); //6강 10:12
                                                                                       //Vector3 fwt = this.transform.TransformDirection(Vector3.right);

        hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist, layerMask);
        Debug.DrawRay(transform.position, dir, Color.red);

        if (patrol == true)
        {
            fPatrolResetTime -= Time.deltaTime;
            if (!bOPatorlOtherDir)
            {


                if (iRandomDir == 0)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * Speed); //3.5f speed

                }
                else if (iRandomDir == 1)
                {
                    transform.Translate(Vector3.down * Time.deltaTime * Speed); //3.5f speed
                }
                else if (iRandomDir == 2)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * Speed); //3.5f speed
                }
                else if (iRandomDir == 3)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * Speed); //3.5f speed
                }

            }
            if (fPatrolResetTime <= 0)
            {

                iRandomDir = Random.Range(0, 3);

                bOPatorlOtherDir = true;
                fPatrolResetTime = 2.0f;


            }
            else if (fPatrolResetTime > 0)
            {

                bOPatorlOtherDir = false;
            }
        }

        if (pursuingPlayer == true)
        {

            Vector3 targetDir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180);
            // targetDir.Normalize();
            transform.Translate(Vector3.up * Time.deltaTime * Speed); //3.5f speed
        }

        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToTarget < ChaseRange /*&& thePlayerHealth.getinstop() == false*/ /*&& bWallCollision == false *//*&& distanceToTargetDeLTA <=0.1f*/)  //chase range 
        {

            pursuingPlayer = true;
            patrol = false;
        }
        else if (distanceToTarget < ChaseRange /*&& thePlayerHealth.getinstop() == true*/)
        {

            patrol = true;
            pursuingPlayer = false;
        }
        else if (distanceToTarget > ChaseRange /*&& bWallCollision == false*//* && distanceToTargetDeLTA <= 0.1f*/)
        {
            pursuingPlayer = true;
        }

        Vector2 dir2;
        Vector2 resultpos;
        if (bOPatorlOtherDirStopRoom /*&& thePlayerHealth.getinstop()*/)
        {

            dir2 = (transform.position - player.transform.position).normalized;
            resultpos = new Vector2(transform.position.x + (dir2.x * 8.0f), transform.position.y + (dir2.y * 8.0f));


            transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);
            bOPatorlOtherDirStopRoom = false;

        }
        //if (distanceToTarget < 1.5)
        //{
        //    bOPatorlOtherDirStopRoom = false;
        //}
    }

    void movement()
    {

        if (Vector3.Distance(this.transform.position, player.transform.position) < 5 && pursuingPlayer == true)
        {

        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        Vector3 dir = player.transform.position - transform.position;

        //여기..
        hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist, layerMask);
        Debug.DrawRay(transform.position, dir, Color.red);

        Vector3 fwt = this.transform.TransformDirection(Vector3.right);

        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(fwt.x, fwt.y), 1.0f, layerMask);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(fwt.x, fwt.y), Color.blue);

        if (moving == true)
        {
            //    if (hasGun == false)
            //    {
            //        transform.Translate(Vector3.right * speed * Time.deltaTime);
            //    }
            //    else
            //    {
            if (Vector3.Distance(this.transform.position, player.transform.position) < 5 && pursuingPlayer == true)
            {

            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            //  }
        }

        if (patrol == true)
        {

            speed = 2.0f;


            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
            //if (distanceToTarget < ChaseRange)  //chase range 3.0f
            //{
            //    Vector3 targetDir = player.transform.position - transform.position;
            //    float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            //    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180);
            //    targetDir.Normalize();


            //   // if(!bOPatorlOtherDir)
            //   // {

            //   // }

            //}
            //else
            //{
            fPatrolResetTime -= Time.deltaTime;
            if (!bOPatorlOtherDir)
            {

                if (iRandomDir == 0)
                {

                    transform.Translate(Vector3.up * Time.deltaTime * Speed); //3.5f speed

                }
                else if (iRandomDir == 1)
                {
                    transform.Translate(Vector3.down * Time.deltaTime * Speed); //3.5f speed
                }
                else if (iRandomDir == 2)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * Speed); //3.5f speed
                }
                else if (iRandomDir == 3)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * Speed); //3.5f speed
                }

            }
            if (fPatrolResetTime <= 0)
            {

                iRandomDir = Random.Range(0, 3);

                bOPatorlOtherDir = true;
                fPatrolResetTime = 2.0f;


            }
            else if (fPatrolResetTime > 0)
            {

                bOPatorlOtherDir = false;
            }
            //     patrol = true;
            //}






            //if(weaponToGoTo != null)
            //{
            //    //patrol = false;
            //    goingToWeapon = true;
            //}
        }
        //추가


        if (pursuingPlayer == true)
        {

            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToTarget < ChaseRange)  //chase range 3.0f
            {
                Vector3 targetDir = player.transform.position - transform.position;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180);
                // targetDir.Normalize();
                transform.Translate(Vector3.up * Time.deltaTime * Speed); //3.5f speed

            }
            else
            {
                patrol = true;
            }

            if (hit2.collider != null)
            {


                if (hit2.collider.tag == "Wall")
                {


                    if (clockwise == false)
                    {
                        transform.Rotate(0, 0, 90);
                        pursuingPlayer = false;
                        patrol = true;
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                        pursuingPlayer = false;
                        patrol = true;
                    }
                }

            }

            //rid.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((playerLastPos.y - transform.position.y), (playerLastPos.x - transform.position.x)) * Mathf.Rad2Deg);

            //if (hit.collider.gameObject.tag == "Player")
            //{

            //    playerLastPos = player.transform.position;
            //}
        }



        //if(goingToLastLoc == true)
        //{
        //    speed = 3.0f;
        //    rid.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((aip.returnNextPoint().y - transform.position.y), (aip.returnNextPoint().x - transform.position.x)) * Mathf.Rad2Deg);
        //    if (Vector3.Distance(this.transform.position, lastPos.transform.position) < 1.5f)
        //    {
        //        patrol = true;
        //        goingToLastLoc = false;

        //        //disableAstar();
        //    }
        //}


        if (goingToWeapon == true)
        {

            speed = 3.0f;
            rid.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((playerLastPos.y - transform.position.y), (playerLastPos.x - transform.position.x)) * Mathf.Rad2Deg);
            //if(ewc.getCur() != null)
            //{
            //    weaponToGoTo = null;
            //    patrol = true;
            //    goingToWeapon = false;
            //    pursuingPlayer = false;
            //    goingToLastLoc = false;
            //}
            if (weaponToGoTo == null)
            {
                weaponToGoTo = null;
                patrol = true;
                goingToWeapon = false;
                pursuingPlayer = false;
                goingToLastLoc = false;

            }
            else
            {
                if (weaponToGoTo.active == false)
                {
                    weaponToGoTo = null;
                    patrol = true;
                    goingToWeapon = false;
                    pursuingPlayer = false;
                    goingToLastLoc = false;
                }
            }

        }
    }

    void StunFunc()
    {
        if (bStun)
        {

            fStunTime -= Time.deltaTime;
            //stun
            patrol = false;
            pursuingPlayer = false;
            moving = false;
            if (fStunTime <= 0)
            {
                fStunTime = 2.0f;
                bStun = false;
            }
        }
    }



    public void heardGunshot(Vector3 lastPositionVec)
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 10.0f)
        {
            playerLastPos = lastPositionVec;
            goingToLastLoc = true;

        }
    }


    void setWeaponToGoTo(GameObject weapon)
    {
        weaponToGoTo = weapon;
        goingToWeapon = true;
        //patrol = false;
        pursuingPlayer = false;
        goingToLastLoc = false;
    }

    void canEnemyFindWeapon()
    {
        //if (/*ewc.getCur() == null &&*/ weaponToGoTo == null && goingToWeapon == false)
        //{
        //    //weapons = obj.getWeapons();
        //    for (int x = 0; x < weapons.Length; x++)
        //    {
        //        float distance = Vector3.Distance(this.transform.position, weapons[x].transform.position);
        //        if (distance < 10)
        //        {
        //            Vector3 dir = weapons[x].transform.position - transform.position;
        //            RaycastHit2D wepCheck = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), distance, layerMask);
        //            if (wepCheck.collider.gameObject.tag == "Weapon")
        //            {
        //                setWeaponToGoTo(weapons[x]);
        //            }
        //        }
        //    }
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Sword" /*&& 아이템사용*/
            /*&& uc.getItemCount() > 0*/ /*&& Input.GetKey(KeyCode.Alpha1) == true*/)
        {

            // int itemp = uc.getItemCount()-1; //아이템개수 감소
            // uc.SetItemCount(itemp);

            // bStun = true;
        }
        if (collision.gameObject.tag == "Wall")
        {
            patrol = true;
            //  bWallCollision = true;
            //pursuingPlayer =true ;
        }
        else
        {
            pursuingPlayer = true;
            //  bWallCollision = false;
            //  patrol = false;
        }
        if (collision.gameObject.tag == "Arrow")
        {
            Destroy(this.gameObject);
        }


    }

    public void deltaPosFunc()
    {
        if (fDeltaTimer >= 0.2f && fDeltaTimer <= 0.3f)
        {
            curPos = transform.position;
        }

        fDeltaTimer -= Time.deltaTime;
        if (fDeltaTimer <= 0)
        {
            deltaPos = transform.position;
            // botherdiredelta = true;
            fDeltaTimer = fDeltaTimerReset;
        }
        else
        {
            // botherdiredelta = false;
        }

        distanceToTargetDeLTA = Vector3.Distance(curPos, deltaPos);

        if (distanceToTargetDeLTA <= 0.1)
        {
            pursuingPlayer = false;
            patrol = true;
        }
    }

    public void playerDetect()
    {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        Vector3 dir = player.transform.position - transform.position;

        Vector3 pos = this.transform.InverseTransformPoint(player.transform.position); //6강 10:12
        Vector3 fwt = this.transform.TransformDirection(Vector3.right);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(fwt.x, fwt.y), 1.0f, layerMask);

        hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist, layerMask);


        fWallCollisionTime -= Time.deltaTime;
        fWallPreviousePosFindTime -= Time.deltaTime;
        if (hit.collider != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

            //if (fWallPreviousePosFindTime <= 0)
            //{
            //    ballPreviousePosFind = true;
            //    EnemyPreviousePos = this.transform.position;
            //    fWallPreviousePosFindTime = 2.0f;
            //}

            //if (fWallCollisionTime <= 0)
            //{

            //    //  iRandomDir = Random.Range(0, 3);

            //    // if (EnemyPreviousePos == this.transform.position)
            //    bWallCollision = false;
            //    // else
            //    //{
            //    //    bWallCollision = false;
            //    // }
            //    fWallCollisionTime = 2f;


            //}
            //else if (fWallCollisionTime > 0)
            //{


            //    bWallCollision = false;


            //}

            if ((hit.collider.gameObject.tag == "Player" && pos.x > 1.5f && Vector3.Distance(this.transform.position, player.transform.position) < 6)
                || (distanceToTarget < 6.0f /*&& hit.collider.gameObject.tag != "Wall" && bWallCollision == false*/)) //여기
            {

                patrol = false;
                pursuingPlayer = true;
                goingToWeapon = false;

                goingToLastLoc = false;

                //disableAstar();
            }
            //else if (hit.collider.gameObject.tag == "Wall")
            //{
            //    transform.Rotate(0, 0, 90);
            //    pursuingPlayer = false;
            //    patrol = true;  //여기
            //    if (pursuingPlayer == true)
            //    {
            //        goingToLastLoc = true;
            //        heardGunshot(playerLastPos);
            //        //pursuingPlayer = false;
            //        goingToWeapon = false;

            //    }
            //    //else
            //    //{
            //    //    patrol = false;
            //    //}
            //}

            if (Vector3.Distance(this.transform.position, player.transform.position) > 6)
            {
                patrol = true;
                pursuingPlayer = false;
            }
        }

    }



    public float getSpeed()
    {
        return speed;
    }

    public bool getStun()
    {
        return bStun;
    }
    public void SetStun(bool _bVar)
    {
        bStun = _bVar;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            // Destroy(collision.gameObject);
            bstop = true;

        }


    }

    private void stopFunc()
    {
        if (bstop)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= 1.0f)
            {
                stopTimer = 0.0f;
                bstop = false;
            }
        }
    }

}
