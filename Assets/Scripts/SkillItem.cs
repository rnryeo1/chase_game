using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    public GameObject rangeObject1;
    BoxCollider2D rangeCollider1;
    public GameObject rangeObject2;
    BoxCollider2D rangeCollider2;
    public GameObject rangeObject3;
    BoxCollider2D rangeCollider3;
    public GameObject rangeObject4;
    BoxCollider2D rangeCollider4;
    float Dist;
    public GameObject player;
    float resetPosTime = 0;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rangeObject1 = GameObject.Find("Spawners/ItemSpawner/ResetSpawner/MonsterSpawner1");
        rangeObject2 = GameObject.Find("Spawners/ItemSpawner/ResetSpawner/MonsterSpawner2");
        rangeObject3 = GameObject.Find("Spawners/ItemSpawner/ResetSpawner/MonsterSpawner3");
        rangeObject4 = GameObject.Find("Spawners/ItemSpawner/ResetSpawner/MonsterSpawner4");
        rangeCollider1 = rangeObject1.GetComponent<BoxCollider2D>();
        rangeCollider2 = rangeObject2.GetComponent<BoxCollider2D>();
        rangeCollider3 = rangeObject3.GetComponent<BoxCollider2D>();
        rangeCollider4 = rangeObject4.GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        StartCoroutine(LaunchDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator LaunchDelay()
    {

        yield return new WaitForSeconds(10f);//15초간있을경우 제자리에서 삭제 
        Destroy(gameObject);
    }
    void Update()
    {
        resetPosition();
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
