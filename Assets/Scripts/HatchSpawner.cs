using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchSpawner : MonoBehaviour
{
    public static HatchSpawner instance;
    public GameObject rangeObject;
    BoxCollider2D rangeCollider;
    [SerializeField] GameObject player;
    // Start is called before the first frame update


    [SerializeField] GameObject hatchObj;
    [SerializeField] int SkillItem = 0, SkillItemSetting = 40;
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
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        rangeObject1 = GameObject.Find("Spawners/HatchSpawner/ResetSpawner/HatchSpawner1");
        rangeObject2 = GameObject.Find("Spawners/HatchSpawner/ResetSpawner/HatchSpawner2");
        rangeObject3 = GameObject.Find("Spawners/HatchSpawner/ResetSpawner/HatchSpawner3");
        rangeObject4 = GameObject.Find("Spawners/HatchSpawner/ResetSpawner/HatchSpawner4");
        rangeCollider1 = rangeObject1.GetComponent<BoxCollider2D>();
        rangeCollider2 = rangeObject2.GetComponent<BoxCollider2D>();
        rangeCollider3 = rangeObject3.GetComponent<BoxCollider2D>();
        rangeCollider4 = rangeObject4.GetComponent<BoxCollider2D>();

        StartCoroutine(WaveSpawn());
    }
    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
        rangeCollider = rangeObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (GameManager.instance.getisMainScene() == false)
        {
            transform.position = player.transform.position;
        }
    }
    IEnumerator WaveSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            //if (SkillItem < SkillItemSetting)
            //{
            //StartCoroutine(delaySpawn(0.1f));
            //}
            minutesWave = Timer.instance.getcurMinutes();
            GameObject obcs = Instantiate(hatchObj, Return_RandomPosition(), Quaternion.identity); //짝 2,4,6,
            obcs.GetComponent<Obstacle>().setMaxHp(15 * (minutesWave + 1)); //12->1   
        }
    }

    IEnumerator delaySpawn(float waitTime)
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(waitTime); //waitTime 만큼 딜레이후 다음 코드가 실행된다.

            GameObject hatchObject = Instantiate(hatchObj, Return_RandomPosition(), Quaternion.identity);
            //SkillItem++;
        }

        //While문을 빠져 나가지 못하여 waitTime마다 Shot함수를 반복실행 됩니다.
    }

    Vector3 Return_RandomPosition()
    {
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
