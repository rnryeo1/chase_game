
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider2D rangeCollider;
    [SerializeField] GameObject player;
    // Start is called before the first frame update

    [SerializeField] int TotalWaveNum = 1;

    [SerializeField] GameObject[] WaveEnemy;
    [SerializeField] GameObject[] WaveBossEnemy;
    [SerializeField] int wave1Num = 0, wave1NumSetting = 60;


    public GameObject rangeObject1;
    BoxCollider2D rangeCollider1;
    public GameObject rangeObject2;
    BoxCollider2D rangeCollider2;
    public GameObject rangeObject3;
    BoxCollider2D rangeCollider3;
    public GameObject rangeObject4;
    BoxCollider2D rangeCollider4;
    Vector3 randpos;
    int minutesWave = 0;
    int hourWave = 0;
    private void Start()
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
        WaveEnemy = Resources.LoadAll<GameObject>("MonsterPrefab");
        WaveBossEnemy = Resources.LoadAll<GameObject>("BossPrefab");
        StartCoroutine(WaveSpawn());
    }

    IEnumerator WaveSpawn()
    {
        while (true && WaveEnemy[minutesWave / 2] != null)
        {

            //IndexOutOfRangeException: Index was outside the bounds of the array. 시간다되면 에러남 
            if (minutesWave <= 5)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(6);
                yield return new WaitForSeconds(1.2f);
            }
            else if (minutesWave <= 10)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(7);
                yield return new WaitForSeconds(1.1f);
            }
            else if (minutesWave <= 20)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(8);
                yield return new WaitForSeconds(1.0f);
            }
            else if (minutesWave <= 24)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(9);
                yield return new WaitForSeconds(0.9f);
            }
            else if (minutesWave <= 28)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(10);
                yield return new WaitForSeconds(0.8f);
            }
            else if (minutesWave <= 36)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(11);
                yield return new WaitForSeconds(0.7f);
            }
            else if (minutesWave <= 40)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(12);
                yield return new WaitForSeconds(0.6f);
            }
            else if (minutesWave <= 44)
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(13);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                player.GetComponent<PlayerStatus>().setPlayerMaxHealth(14);
                yield return new WaitForSeconds(0.4f);
            }
            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            minutesWave = Timer.instance.getcurMinutes();
            // if (Timer.instance.getcurMinutes() == minutesWave) ///2 ,4 ,6 ,8 
            //  {
            //1분 0 2분 1 3분 1 4분 2 
            GameObject mon1 = Instantiate(WaveEnemy[minutesWave / 2], Return_RandomPosition(), Quaternion.identity); //짝 2,4,6,
            mon1.GetComponent<AIChase>().setMaxHp(((minutesWave + 1) * 6)); //12->1   
                                                                            //+ ((int)player.GetComponent<PlayerStatus>().getmaxSkillhp() - 1) * 5
                                                                            //스킬처음배우면 -1 * 5씩 체력증가..

            if (Timer.instance.getseconds() == 59) //홀   1,3,5  1분 1 2분 0 3분 1  && minutesWave % 2 == 1
            {
                GameObject monboss = Instantiate(WaveBossEnemy[minutesWave / 2], Return_RandomPosition(), Quaternion.identity);
                monboss.GetComponent<AIChase>().setMaxHp(40 * (minutesWave + 1));
                monboss.GetComponent<AIChase>().setIsBoss(true);
            }
            // }

            // if (Timer.instance.getcurMinutes() == 2)
            // {
            //     GameObject mon2 = Instantiate(Wave2Enemy[], Return_RandomPosition(), Quaternion.identity);
            //     mon2.GetComponent<AIChase>().setMonsterWaveLevel(GameManager.instance.CurrentWaveNum);
            //     mon2.GetComponent<AIChase>().setHp(GameManager.instance.CurrentWaveNum);
            //     if (Timer.instance.getseconds() == 60)
            //     {
            //         GameObject monboss = Instantiate(WaveBossEnemy[], Return_RandomPosition(), Quaternion.identity);
            //         monboss.GetComponent<AIChase>().setMaxHp(15);
            //         monboss.GetComponent<AIChase>().setIsBoss(true);
            //     }
            // }

        }
    }



    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        transform.position = player.transform.position;
    }


    Vector3 Return_RandomPosition()
    {
        // Vector3 originPosition = rangeObject.transform.position;
        // // 콜라이더의 사이즈를 가져오는 bound.size 사용
        // float range_X = rangeCollider.bounds.size.x;
        // float range_Y = rangeCollider.bounds.size.y;

        // range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        // range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        // Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        // Vector3 respawnPosition = originPosition + RandomPostion;

        // return respawnPosition;
        //==============================================================================

        float random = Random.Range(1, 5);
        if (random == 1)
        {


            randpos = RandomPosition1();
        }
        else if (random == 2)
        {

            randpos = RandomPosition2();
        }
        else if (random == 3)
        {

            randpos = RandomPosition3();
        }
        else if (random == 4)
        {

            randpos = RandomPosition4();
        }
        return randpos;
    }
    Vector3 RandomPosition1()
    {
        Vector3 originPosition = rangeObject1.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider1.bounds.size.x;
        float range_Y = rangeCollider1.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;

        return respawnPosition;
    }
    Vector3 RandomPosition2()
    {
        Vector3 originPosition = rangeObject2.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider2.bounds.size.x;
        float range_Y = rangeCollider2.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;


        return respawnPosition;
    }
    Vector3 RandomPosition3()
    {
        Vector3 originPosition = rangeObject3.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider3.bounds.size.x;
        float range_Y = rangeCollider3.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
    Vector3 RandomPosition4()
    {

        Vector3 originPosition = rangeObject4.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider4.bounds.size.x;
        float range_Y = rangeCollider4.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);
        Vector3 respawnPosition = originPosition + RandomPostion;

        return respawnPosition;
    }
}
