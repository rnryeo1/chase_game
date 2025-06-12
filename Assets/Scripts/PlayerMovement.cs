using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 touchPosition;
    Vector3 touchPositiondelta;
    Touch touch;
    bool bTouchFirstDir = false;
    public float speed = 0.8f;
    public float deltaposamount = 4.0f;
    public float deltaSpeed = 1.0f;
    public float groundDistance = 0.4f;
    public Vector2 currentPosition; //= Vector2.zero;
    public Vector2 moveDeltaPos; //= Vector2.zero;
    public Vector2 firstPoint; //= Vector2.zero;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float fgravity = 9.8f;

    private float jumpTime = 0.0f;
    bool isGrounded;
    Vector2 resultpos;

    public bool startleft;
    public bool startright;
    public bool startTop;
    public bool startBottom;
    public float JumpCoolTime = 0.0f;
    public float jumpForce;
    public float moveSpeed = 5;

    float ftimer = 0.0f;
    float firstTimerResetTimer = 0.0f;
    bool breset = false;
    public float smoothTime = 300.3f;
    private Vector3 velocity;

    private float xvel = 0.0f;
    private float yvel = 0.0f;

    public bool jumpstart = false;
    private static bool playerExists;
    private float deltaX, deltaY;
    private Rigidbody2D rb;

    public Vector2 nowPos, prePos;
    public Vector3 movePos;


    private Vector2 playerposition;

    public bool baimposchange = false;
    public float forwardspeed = 0.0f;





    // Start is called before the first frame update
    void Start()
    {
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(Restfirstfos());
        rb = GetComponent<Rigidbody2D>();

    }



    // Update is called once per frame
    void Update()
    {
        GetCurrentTouchPos();
        movementCheckNew();
        moveForward();

        Jump();
    }

    public Vector2 GetmoveDeltaPos() { return moveDeltaPos; }
    public Vector2 getposition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
    public void movementCheckNew()
    {

        moveDeltaPos = (currentPosition - firstPoint).normalized;

        resultpos = new Vector2(transform.position.x /*+ (moveDeltaPos.x * deltaSpeed)*/, transform.position.y + (moveDeltaPos.y * deltaSpeed));

        transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);

    }



    public void GetCurrentTouchPos()
    {

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).position.x > 0 && Input.GetTouch(0).position.x < 100 && Input.GetTouch(0).position.y > 0 & Input.GetTouch(0).position.y < 800)
            {
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)    // 딱 처음 터치 할때 발생한다
                {
                    firstPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    //transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);
                    //Debug.Log(" TouchPhase.Began x:" + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x);
                    // Debug.Log(" TouchPhase.Began y:" + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
                    currentPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    //   moveDeltaPos = (currentPosition - firstPoint).normalized;

                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)    // 터치하고 움직이믄 발생한다. firstpoint 갱신(몇초마다)
                {
                    //if (breset) //when reset firstPoint
                    //{
                    //    //firstPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position); //갱신시에또 firstpoint 바꿔어서 문제다. 갱신이되는데처음포지션이바뀜

                    //    //transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);
                    //    //breset = false;
                    //}

                    currentPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    //Debug.Log(" TouchPhase.Moved x:" + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x);
                    //Debug.Log(" TouchPhase.Moved y:" + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
                    rb.linearVelocity = new Vector2(speed * 3, rb.linearVelocity.y);
                    //  moveDeltaPos = (currentPosition - firstPoint).normalized;

                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)    // 터치 따악 떼면 발생한다.
                {

                    firstPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//transform.position;//Input.GetTouch(0).position;    // 터치한 위치
                                                                                            //transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);
                    currentPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);                                                      // 할거 해라.       
                                                                                                                                                       // Debug.Log(" TouchPhase.Ended x:" + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x);
                                                                                                                                                       // Debug.Log(" TouchPhase.Ended y:" + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);

                    // moveDeltaPos = (currentPosition - firstPoint).normalized;

                }
            }
        }



    }
    IEnumerator Restfirstfos()
    {
        while (true)
        {

            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).position.x > 0 && Input.GetTouch(0).position.x < 100 && Input.GetTouch(0).position.y > 0 & Input.GetTouch(0).position.y < 800)
                {
                }
                else
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)    // 터치하고 움직이믄 발생한다. firstpoint 갱신(몇초마다)
                    {
                        firstPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position); //갱신시에또 firstpoint 바꿔어서 문제다. 갱신이되는데처음포지션이바뀜

                        //transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);
                        transform.position = Vector3.Lerp(transform.position, resultpos, Time.deltaTime * speed);

                    }
                }
            }
            // breset = true;
            yield return new WaitForSeconds(0.000000000001f);


        }
    }

    public void moveForward()
    {

        transform.Translate(Vector3.right * Time.deltaTime * forwardspeed);

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).position.x > 0 && Input.GetTouch(0).position.x < 100 && Input.GetTouch(0).position.y > 0 & Input.GetTouch(0).position.y < 800)
            {
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)    // 터치하고 움직이믄 발생한다. firstpoint 갱신(몇초마다)
                {

                }
            }
        }
    }

    public void Jump()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (jumpstart)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime >= 0.5f)
            {
                jumpTime = 0.0f;
                jumpstart = false;
            }
        }
        else
        {

        }
        //if ((jumpstart && startleft && JumpCoolTime <= 0.4f))
        //{
        //    //startleft = false;
        //    //startright = false;
        //    //startTop = false;
        //    // startBottom = false;

        //    transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(-1.0f, 0, 0), 2.5f * Time.deltaTime);
        //}
        if ((jumpstart && startright && jumpTime <= 0.25f))
        {
            //startleft = false;

            // startTop = false;
            // startBottom = false;

            //transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(1.0f, 0, 0), 2.5f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0.25f, 0, 0), 1.5f * Time.deltaTime);
            this.transform.Translate(Vector3.up * Time.smoothDeltaTime * jumpForce);
        }
        else if (jumpstart && startright && jumpTime <= 0.5f)
        {
            this.transform.Translate(Vector3.down * Time.smoothDeltaTime * jumpForce);
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0.25f, 0, 0), 1.5f * Time.deltaTime);
        }

    }




    public void onRight()
    {
        // if (isGrounded)
        //  {
        //  JumpCoolTime = 0.0f;
        if (jumpstart == false)
        {

            startright = true;
            startleft = false;

            //rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, jumpForce);


            jumpstart = true;
        }
        //transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(1.0f, 0, 0), 1.5f * Time.deltaTime);
        // }
    }



}
