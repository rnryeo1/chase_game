using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    float speed;
    public float distanceBetween;

    private float distance;
    private float attack_distance;

    private bool m_bchasing = true;
    public float fStunTime = 2.0f;
    public float fStunResetTime = 2.0f;

    public bool m_bStun = false;
    private int monsterWaveLevel = 1;

    [SerializeField] GameObject SkillItem = null;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] int curHp, maxHealth = 1;

    private bool isboss = false;
    public GameObject rangeObject1;
    BoxCollider2D rangeCollider1;
    public GameObject rangeObject2;
    BoxCollider2D rangeCollider2;
    public GameObject rangeObject3;
    BoxCollider2D rangeCollider3;
    public GameObject rangeObject4;
    BoxCollider2D rangeCollider4;
    float Dist;

    float resetPosTime = 0;
    public GameObject attackObj;
    float spawnTime = 5.0f;
    private float timetoRespawn = 0.0f;
    [SerializeField] float startAngle = 0f, endAngle = 360f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rangeObject1 = GameObject.Find("Spawners/MonsterSpawner/ResetSpawner/MonsterSpawner1");
        rangeObject2 = GameObject.Find("Spawners/MonsterSpawner/ResetSpawner/MonsterSpawner2");
        rangeObject3 = GameObject.Find("Spawners/MonsterSpawner/ResetSpawner/MonsterSpawner3");
        rangeObject4 = GameObject.Find("Spawners/MonsterSpawner/ResetSpawner/MonsterSpawner4");
        rangeCollider1 = rangeObject1.GetComponent<BoxCollider2D>();
        rangeCollider2 = rangeObject2.GetComponent<BoxCollider2D>();
        rangeCollider3 = rangeObject3.GetComponent<BoxCollider2D>();
        rangeCollider4 = rangeObject4.GetComponent<BoxCollider2D>();

        attackObj = Resources.Load<GameObject>("MonsterMissile/MonsterMissile");
    }
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.8f;
        //if (isboss)
        //{
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        //}
        curHp = maxHealth;

        fStunTime = fStunResetTime;
    }


    // Update is called once per frame
    void Update()
    {
        //if (isboss)
        //{
        healthBar.UpdateHealthBar(curHp, maxHealth);
        //}
        if (m_bchasing)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (distance < distanceBetween)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            }
        }

        StunFunc();
        destoryMonster();
        resetPosition();
        if (!isboss)
        {
            playerAttack();
        }
        else
        {
            bossAttack();
        }

    }

    private void playerAttack()
    {
        attack_distance = Vector2.Distance(transform.position, player.transform.position);
        timetoRespawn += Time.deltaTime;
        if (attack_distance < 4 && timetoRespawn >= spawnTime) //2초마다.
        {
            Vector2 bulDir = (player.transform.position - transform.position).normalized;
            GameObject go = Instantiate(attackObj, transform.position, Quaternion.identity);
            MonsterMissile mm = go.GetComponent<MonsterMissile>();
            mm.direction = bulDir;
            timetoRespawn = 0.0f;
        }
    }
    void bossAttack()
    {

        attack_distance = Vector2.Distance(transform.position, player.transform.position);
        timetoRespawn += Time.deltaTime;
        if (attack_distance < 4 && timetoRespawn >= spawnTime) //2초마다.
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                StartCoroutine(delaySpawn(0.1f));
                timetoRespawn = 0.0f;

            }
            else if (rand == 1)
            {
                BossShoot();
                timetoRespawn = 0.0f;
            }

        }


    }
    void destoryMonster()
    {
        // if (monsterWaveLevel != GameManager.instance.CurrentWaveNum)
        // {
        //     //Destroy(gameObject);
        // }
    }



    IEnumerator delaySpawn(float waitTime)
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("어택3");
            yield return new WaitForSeconds(waitTime); //waitTime 만큼 딜레이후 다음 코드가 실행된다.

            Vector2 bulDir = (player.transform.position - transform.position).normalized;
            GameObject go = Instantiate(attackObj, transform.position, Quaternion.identity);
            MonsterMissile mm = go.GetComponent<MonsterMissile>();
            mm.direction = bulDir;
        }
        //While문을 빠져 나가지 못하여 waitTime마다 Shot함수를 반복실행 됩니다.
    }

    public void BossShoot()
    {
        float angleStep = (endAngle - startAngle) / 12;//multiarrowCount;
        float angle = startAngle;
        for (int i = 0; i < 12; i++) //multiarrowCount
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);  //라디안을 도로 변환하기  pi/180
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;
            GameObject go = Instantiate(attackObj, transform.position, Quaternion.identity);
            MonsterMissile mm = go.GetComponent<MonsterMissile>();
            mm.direction = bulDir;

            angle += angleStep;
        }
    }



    public void TakeDamage(int damageAmount)
    {
        curHp -= damageAmount;
        healthBar.UpdateHealthBar(curHp, maxHealth);

    }
    void Die()
    {
        Destroy(gameObject);
    }

    public void setMonsterWaveLevel(int _waveLevel)
    {
        monsterWaveLevel = _waveLevel;
    }
    public void setHp(int _curHp)
    {
        curHp = _curHp;
        if (gameObject.CompareTag("Boss"))
        {
            if (curHp <= 0)
            {
                ItemSpawner.instance.dropskillItemandStealHealth(transform);
                Destroy(gameObject);
            }
        }
        if (gameObject.CompareTag("Enemy"))
        {
            if (curHp <= 0)
            {
                ItemSpawner.instance.stealHealth();
                Destroy(gameObject);
            }
        }

    }
    public void setMaxHp(int _curHp)
    {
        maxHealth = _curHp;
    }
    public int getHp()
    {
        return curHp;
    }

    void StunFunc()
    {
        if (m_bStun)
        {
            m_bchasing = false;
            fStunTime -= Time.deltaTime;

            if (fStunTime <= 0)
            {
                fStunTime = fStunResetTime;
                m_bchasing = true;
                m_bStun = false;
            }
        }
    }

    void resetPosition()
    {

        resetPosTime += Time.deltaTime;
        Dist = Vector3.Distance(player.transform.position, transform.position);
        if (Dist >= 5 && resetPosTime >= 5.0f)//5초마다 위치바꿈 
        {
            float random = Random.Range(1, 5);
            if (random == 1)
            {
                RandomPosition1();
            }
            else if (random == 2)
            {
                RandomPosition2();
            }
            else if (random == 3)
            {
                RandomPosition3();
            }
            else if (random == 4)
            {
                RandomPosition4();
            }
            resetPosTime = 0;
        }
    }

    public void setIsBoss(bool _isBoss)
    {
        isboss = _isBoss;
    }


    void RandomPosition1()
    {
        Vector3 originPosition = rangeObject1.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider1.bounds.size.x;
        float range_Y = rangeCollider1.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;

        transform.position = respawnPosition;
    }
    void RandomPosition2()
    {
        Vector3 originPosition = rangeObject2.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider2.bounds.size.x;
        float range_Y = rangeCollider2.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;

        transform.position = respawnPosition;
    }
    void RandomPosition3()
    {
        Vector3 originPosition = rangeObject3.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider3.bounds.size.x;
        float range_Y = rangeCollider3.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;

        transform.position = respawnPosition;
    }
    void RandomPosition4()
    {
        Vector3 originPosition = rangeObject4.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider4.bounds.size.x;
        float range_Y = rangeCollider4.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;

        transform.position = respawnPosition;
    }
}
