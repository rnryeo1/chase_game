using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class directionArea : MonoBehaviour
{
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
    [SerializeField] int m_damageStack = 1;
    Transform m_tfTarget = null;
    int skillCount = 1;

    private bool firstTargetFounded = false;


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
        float fvalue = 1.0f * (0.07f * (float)skillCount);
        transform.localScale = new Vector3(fvalue, fvalue, fvalue);

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

    public void setSkillCount(int _count)
    {
        skillCount = _count;
    }

    IEnumerator LaunchDelay()
    {
        SearchEnemy();
        yield return new WaitForSeconds(1);//5초간있을경우 제자리에서 삭제 
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
            firstTargetFounded = true; //처음적을발견하면 타겟 사라져도 바로삭제 X
            Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
            float vx = t_dir.x * m_speed;
            float vy = t_dir.y * m_speed;
            m_rigid.linearVelocity = new Vector2(vx, vy);
            //transform.position = Vector3.Lerp(transform.position, t_dir, 0.25f);
        }
        else
        {
            if (!firstTargetFounded)
            {
                Destroy(gameObject);
            }
            else
            {

            }
        }


        // float vx = direction.x * m_speed;
        // float vy = direction.y * m_speed;
        // m_rigid.velocity = new Vector2(vx, vy);
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
    void rotate()
    {
        transform.Rotate(new Vector3(0, 0, m_RotationSpeed * Time.deltaTime));
    }
}
