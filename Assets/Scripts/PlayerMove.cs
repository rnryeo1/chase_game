using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public FixedJoystick Joystick;

    public VariableJoystick joystick;
    public Canvas inputCanvas;
    public bool isJoystick;

    Rigidbody2D rb;
    Vector2 move;

    public Sprite[] sprites;

    SpriteRenderer sr;

    Vector2 m_moveLimit = new Vector2(0.0f, 6.0f);
    public float moveSpeed;
    public float rotationspeed;

    [SerializeField] bool ismoving = false;
    Vector2 bulDir;
    private int movespeedCount = 10;
    [SerializeField] TextMeshProUGUI TextMovespped;
    // Start is called before the first frame update
    private int m_IndexSprite;

    private float m_Speed = 0.2f;
    Coroutine m_CorotineAnim;
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage")
        {
            moveSpeed = 2.0f;
            movespeedCount = 10;//10번만 증가하게 
        }
    }

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        joystick = GameObject.Find("StageCanvas/Variable Joystick").GetComponent<VariableJoystick>();
    }





    public void EnableJoystickInput()
    {
        isJoystick = true;
    }

    // Update is called once per frame
    void Update()
    {
        TextMovespped.text = "Move Speed " + movespeedCount.ToString();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.getisMainScene() == false)
        {
            move.x = joystick.Horizontal;
            move.y = joystick.Vertical;
            float hAxis = move.x;
            float vAxis = move.y;
            float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
            // transform.eulerAngles = new Vector3(0f, 0f, -zAxis);
            if (zAxis > 0)
            {
                sr.sprite = sprites[0];

            }
            else
            {
                sr.sprite = sprites[1];

            }

            if (ismvoing())
            {
                rb.MovePosition(rb.position + joystick.Direction * moveSpeed * Time.deltaTime);
            }

            if (isJoystick)
            {
                var movementDirection = new Vector3(joystick.Direction.x, joystick.Direction.y, 0.0f);
                // if (movementDirection.sqrMagnitude <= 0)
                // {
                //     return;
                // }
                //var targetDirection = Vector3.RotateTowards(transform.right, movementDirection, rotationspeed * Time.deltaTime, 0.0f);
                //transform.rotation = Quaternion.LookRotation(targetDirection);
            }
        }
    }
    public Vector2 getTouchDirectionVector()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(touch.position);

            float inputX = worldpos.x;
            float inputY = worldpos.y;
            Vector3 InputVector = new Vector3(inputX, inputY, 0f);
            bulDir = (InputVector - transform.position).normalized;
            return bulDir;
        }
        return bulDir;
    }


    public bool ismvoing()
    {
        if (move.x != 0 || move.y != 0)
        {
            ismoving = true;
        }
        return ismoving;
    }

    public void PlayermovespeedUp()
    {
        if (movespeedCount >= 1)
        {
            moveSpeed += 0.1f + (0.1f * MainSceneManager.instance.getMoveSpeed()); //10번만 증가하게 
            GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            movespeedCount--;
        }
    }


}
