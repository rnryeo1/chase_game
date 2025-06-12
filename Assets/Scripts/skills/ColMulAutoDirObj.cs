using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColMulAutoDirObj : MonoBehaviour
{

    Rigidbody2D m_rigid = null;
    Transform m_tfTarget = null;

    [SerializeField] float m_speed = 10f;
    float m_currentSpeed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] ParticleSystem m_psEffect = null;
    public Vector2 size;
    public float angle;
    [SerializeField] int m_damageStack = 1;

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
    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    IEnumerator LaunchDelay()
    {
        SearchEnemy();
        //m_psEffect.Play();

        yield return new WaitForSeconds(5f);//5초간있을경우 제자리에서 삭제 
        Destroy(gameObject);
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
        }

        if (other.transform.CompareTag("Boss"))
        {
            spawnEffect(other);

            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);

            Destroy(gameObject);
        }
        if (other.transform.CompareTag("Obstacle"))
        {
            spawnEffect(other);
            int hp = other.gameObject.GetComponent<Obstacle>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<Obstacle>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<Obstacle>().setHp(hp);

            Destroy(gameObject);
        }

    }
}
