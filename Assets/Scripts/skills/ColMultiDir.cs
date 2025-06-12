using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColMultiDir : MonoBehaviour
{
    [SerializeField] GameObject m_colmuldirarrow = null;
    //public GameObject m_recochet = null;
    [SerializeField] Transform m_colmuldirSpawn = null;
    Rigidbody2D m_rigid = null;
    [SerializeField] float m_speed = 10f;

    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] ParticleSystem m_psEffect = null;

    public Vector2 size;
    public float angle;
    public Vector3 direction = new Vector3(1, 0);

    [SerializeField] float roamingSecond = 0.5f;
    [SerializeField] Vector2 velocity;

    [SerializeField] float m_RotationSpeed = 200f;
    [SerializeField] int multiarrowCount = 1;
    [SerializeField] float startAngle = 0f, endAngle = 360f;
    [SerializeField] int m_damageStack = 1;
    [SerializeField] int IncreaseskillCount = 10;
    int skillCount = 1;

    [SerializeField] bool maxSkillCounted = false;
    Transform m_tfTarget = null;

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
        m_rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(LaunchDelay());
    }

    void SearchEnemy()
    {

        Collider2D[] t_cols = Physics2D.OverlapBoxAll(transform.position, size, 0, m_layerMask);
        if (t_cols.Length > 0)
        {
            //검출된 대상중 랜덤으로 표적 

            m_tfTarget = t_cols[Random.Range(0, t_cols.Length)].transform;
        }
    }
    IEnumerator LaunchDelay()
    {
        SearchEnemy();
        yield return new WaitForSeconds(5);//5초간있을경우 제자리에서 삭제 

        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (m_tfTarget != null)
        {

            //if (m_currentSpeed <= m_speed)
            //   m_currentSpeed += m_speed * Time.deltaTime;

            //transform.position += transform.position * m_currentSpeed * Time.deltaTime;
            Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
            float vx = t_dir.x * m_speed;
            float vy = t_dir.y * m_speed;
            m_rigid.linearVelocity = new Vector2(vx, vy);
            //transform.position = Vector3.Lerp(transform.position, t_dir, 0.25f);
        }
        else
        {
            Destroy(gameObject);
        }
        //float vx = direction.x * m_speed;
        //float vy = direction.y * m_speed;
        //m_rigid.velocity = new Vector2(vx, vy);
        rotate();
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
        if (other.transform.CompareTag("Enemy"))
        {

            spawnEffect(other);

            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);

            Destroy(gameObject);


            initDirObjbeforeDestroy();
            Destroy(gameObject);
        }

        if (other.transform.CompareTag("Boss"))
        {
            spawnEffect(other);

            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);


            initDirObjbeforeDestroy();
            Destroy(gameObject);
        }
        if (other.transform.CompareTag("Obstacle"))
        {
            spawnEffect(other);
            int hp = other.gameObject.GetComponent<Obstacle>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<Obstacle>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<Obstacle>().setHp(hp);

            initDirObjbeforeDestroy();
            Destroy(gameObject);
        }

    }
    void rotate()
    {
        transform.Rotate(new Vector3(0, 0, m_RotationSpeed * Time.deltaTime));
    }

    void initDirObjbeforeDestroy()
    {
        float angleStep = (endAngle - startAngle) / skillCount;
        float angle = startAngle;
        for (int i = 0; i < skillCount; i++)
        {
            //atan->각도나옴 ,sin,cos 좌표 나옴 acos asin 이면 각도 그냥이면 좌표 
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);  //라디안을 도로 변환하기  pi/180
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;
            GameObject go = Instantiate(m_colmuldirarrow, m_colmuldirSpawn.position, Quaternion.identity);
            ColMultiDirObj mr = go.GetComponent<ColMultiDirObj>();
            mr.direction = bulDir;
            angle += angleStep;

        }
    }
    public void setSkillCount(int _count)
    {
        skillCount = _count;
    }

}
