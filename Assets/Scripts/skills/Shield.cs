using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Rigidbody2D m_rigid = null;
    Transform m_tfTarget = null;

    [SerializeField] float m_speed = 10f;
    float m_currentSpeed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] ParticleSystem m_psEffect = null;


    public Vector2 size;
    public float angle;

    private int currentTargetNum = 0;
    private int curNum = 0;
    public Vector3 direction = new Vector3(1, 0);
    [SerializeField] Vector2 velocity;
    [SerializeField] GameObject player;
    [SerializeField] float circleR = 1.0f; //반지름
    [SerializeField] int m_damageStack = 1;
    float deg; //각도

    private bool collided = false;

    [SerializeField] GameObject m_psEffect1 = null;
    [SerializeField] GameObject m_psEffect2 = null;
    [SerializeField] GameObject m_psEffect3 = null;
    [SerializeField] GameObject m_psEffect4 = null;
    [SerializeField] GameObject m_psEffect5 = null;
    [SerializeField] GameObject m_psEffect6 = null;
    [SerializeField] GameObject m_psEffect7 = null;


    void Start()
    {
        m_psEffect1 = Resources.Load<GameObject>("bloodprefab/bloodeff_01");
        m_psEffect2 = Resources.Load<GameObject>("bloodprefab/bloodeff_02");
        m_psEffect3 = Resources.Load<GameObject>("bloodprefab/bloodeff_03");
        m_psEffect4 = Resources.Load<GameObject>("bloodprefab/bloodeff_04");
        m_psEffect5 = Resources.Load<GameObject>("bloodprefab/bloodeff_05");
        m_psEffect6 = Resources.Load<GameObject>("bloodprefab/bloodeff_06");
        m_psEffect7 = Resources.Load<GameObject>("bloodprefab/bloodeff_07");
        player = GameObject.FindGameObjectWithTag("Player");
        m_rigid = GetComponent<Rigidbody2D>();
        //StartCoroutine(LaunchDelay());
        float vx = direction.x;
        float vy = direction.y;
        m_rigid.linearVelocity = new Vector2(vx, vy);
    }


    IEnumerator LaunchDelay()
    {

        yield return new WaitForSeconds(5);//5초간있을경우 제자리에서 삭제 
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        // float vx = direction.x; //* m_speed
        // float vy = direction.y;

        deg += Time.deltaTime * m_speed;
        // float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);  //라디안을 도로 변환하기  pi/180
        // float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);
        if (deg < 360)
        {
            var rad = Mathf.Deg2Rad * (deg);
            var x = circleR * Mathf.Sin(rad);
            var y = circleR * Mathf.Cos(rad);
            transform.position = player.transform.position + new Vector3(x, y);
            //transform.rotation = Quaternion.Euler(0, 0, deg * -1); //가운데를 바라보게 각도 조절
            //transform.RotateAround(player.transform.position, Vector2.right, m_speed * Time.deltaTime);
        }
        else
        {
            deg = 0;
        }
        collided = false;
        //m_rigid.velocity = new Vector2(vx, vy);

    }
    void spawnEffect(Collider2D other)
    {
        int rand = Random.Range(1, 8);
        //Debug.Log("랜덤값:" + rand);
        if (rand == 1)
        {
            Instantiate(m_psEffect1, other.transform.position, other.transform.rotation);
        }
        else if (rand == 2)
        {
            Instantiate(m_psEffect2, other.transform.position, other.transform.rotation);
        }
        else if (rand == 3)
        {
            Instantiate(m_psEffect3, other.transform.position, other.transform.rotation);
        }
        else if (rand == 4)
        {
            Instantiate(m_psEffect4, other.transform.position, other.transform.rotation);
        }
        else if (rand == 5)
        {
            Instantiate(m_psEffect5, other.transform.position, other.transform.rotation);
        }
        else if (rand == 6)
        {
            Instantiate(m_psEffect6, other.transform.position, other.transform.rotation);
        }
        else if (rand == 7)
        {
            Instantiate(m_psEffect7, other.transform.position, other.transform.rotation);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        collided = true;
        if (other.transform.CompareTag("Enemy") && collided)
        {
            spawnEffect(other);

            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);
        }

        if (other.transform.CompareTag("Boss"))
        {

            spawnEffect(other);

            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);

        }
        if (other.transform.CompareTag("Obstacle"))
        {
            spawnEffect(other);
            int hp = other.gameObject.GetComponent<Obstacle>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<Obstacle>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<Obstacle>().setHp(hp);


        }

    }
}
