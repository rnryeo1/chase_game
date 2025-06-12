using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ArrowOrb : MonoBehaviour
{
    [SerializeField] GameObject m_orbArrow = null;
    [SerializeField] Transform m_orbArrowSpawn = null;
    Rigidbody2D m_rigid = null;
    Transform m_tfTarget = null;
    [SerializeField] int Count = 1;


    [SerializeField] int CountLastHit = 12;
    [SerializeField] float m_speed = 10f;
    [SerializeField] float m_RotationSpeed = 200f;
    float m_currentSpeed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] ParticleSystem m_psEffect = null;
    public Vector2 size;

    [SerializeField] float startAngle = 0f, endAngle = 360f;

    [SerializeField] bool isAutoSpawn = true;
    float spawnTime = 0.1f;
    private float timetoRespawn = 0.0f;
    float deg; //각도
    float angleStep;
    float angleShoot;

    private bool onceSpawn = false;

    [SerializeField] float destroyTime = 4.0f;
    private float countingTime = 0.0f;
    [SerializeField] int m_damageStack = 1;

    int skillCount = 1;

    void Start()
    {
        angleShoot = startAngle;
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

        yield return new WaitForSeconds(destroyTime);//5초간있을경우 제자리에서 삭제 

        shootBeforeDestroy();
        Destroy(gameObject);

    }


    // Update is called once per frame
    void Update()
    {
        countingTime += Time.deltaTime;
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
            if (!onceSpawn)
            {
                StartCoroutine(shoot());
                onceSpawn = true;
            }
        }

        // timetoRespawn += Time.deltaTime;
        // if (isAutoSpawn && timetoRespawn >= spawnTime)
        // {
        //     if (m_orbArrow != null)
        //     {

        //         timetoRespawn = 0.0f;
        //     }
        // }
        rotate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);

            //Destroy(gameObject);

        }

        if (other.transform.CompareTag("Boss"))
        {
            int hp = other.gameObject.GetComponent<AIChase>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<AIChase>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<AIChase>().setHp(hp);

            //Destroy(gameObject);

        }
        if (other.transform.CompareTag("Obstacle"))
        {
            int hp = other.gameObject.GetComponent<Obstacle>().getHp();
            hp -= m_damageStack;
            other.gameObject.GetComponent<Obstacle>().TakeDamage(m_damageStack);

            other.gameObject.GetComponent<Obstacle>().setHp(hp);

            //Destroy(gameObject);

        }
    }

    IEnumerator shoot()
    {
        while (countingTime <= destroyTime)
        {
            angleStep = (endAngle - startAngle) / Count;

            //for (int i = 0; i < Count; i++)
            //atan->각도나옴 ,sin,cos 좌표 나옴 acos asin 이면 각도 그냥이면 좌표 
            float bulDirX = transform.position.x + Mathf.Sin((angleShoot * Mathf.PI) / 180f);  //라디안을 도로 변환하기  pi/180
            float bulDirY = transform.position.y + Mathf.Cos((angleShoot * Mathf.PI) / 180f);

            //다른문양
            // float bulDirX2 = transform.position.x + Mathf.Sin((angle + 180f * i) * Mathf.PI / 180f);
            // float bulDirY2 = transform.position.y + Mathf.Cos((angle + 180f * i) * Mathf.PI / 180f); 

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;
            GameObject go = Instantiate(m_orbArrow, m_orbArrowSpawn.position, Quaternion.identity);

            Orbs mr = go.GetComponent<Orbs>();
            mr.direction = bulDir;

            angleShoot += angleStep;
            //angle += 10f;//다른패턴
            // if(angle>=360f) //다른패턴 2 
            // {
            //     angle = 0f;
            // }
            // }
            if (angleShoot >= 360f) //다른패턴 2 
            {
                angleShoot = 0f;
            }
            yield return new WaitForSeconds(1.0f - (skillCount * 0.1f));
        }
        //}
    }

    void shootBeforeDestroy()
    {
        float angleStep = (endAngle - startAngle) / CountLastHit;
        float anglebeforeDestroy = startAngle;
        for (int i = 0; i < CountLastHit; i++)
        {
            //atan->각도나옴 ,sin,cos 좌표 나옴 acos asin 이면 각도 그냥이면 좌표 
            float bulDirX = transform.position.x + Mathf.Sin((anglebeforeDestroy * Mathf.PI) / 180f);  //라디안을 도로 변환하기  pi/180
            float bulDirY = transform.position.y + Mathf.Cos((anglebeforeDestroy * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;
            GameObject go = Instantiate(m_orbArrow, m_orbArrowSpawn.position, Quaternion.identity);
            Orbs mr = go.GetComponent<Orbs>();
            mr.direction = bulDir;
            anglebeforeDestroy += angleStep;
        }
    }



    void rotate()
    {
        transform.Rotate(new Vector3(0, 0, m_RotationSpeed * Time.deltaTime));
    }


    public void setSkillCount(int _count)
    {
        skillCount = _count;
    }
}
