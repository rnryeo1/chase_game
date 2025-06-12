using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleParallaxer : MonoBehaviour
{

    public class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public float disappearTime = 0.0f;
        public float YMoveAmountTime = 0.0f;
        public PoolObject(Transform t) { transform = t; }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }


    }
    [System.Serializable]
    public struct YSpawnRange
    {
        public float min;
        public float max;
    }
    [System.Serializable]
    public struct ScaleRange
    {
        public float scalemin;
        public float scalemax;
    }
    public GameObject Prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public YSpawnRange ySpawnRange;
    public ScaleRange scaleRange;
    public Vector3 defaultSpawnPos;
    public bool spawnImmediate; //particle prewarm
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;
    public float playerApartPosX = 3.0f;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameObject player;
    // GameManager game;

    void Awake()
    {
        Configure();
    }
    void Start()
    {
        //   game = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnEnable()
    {
        //   GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable()
    {
        //   GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameOverConfirmed()
    {

        //Ladders[] ladders2 = this.GetComponentsInChildren<Ladders>();
        //for (int i = 0; i < ladders2.Length; i++)
        //{

        //    scorezone sz = ladders2[i].GetComponentInChildren<scorezone>();
        //    sz.GetComponent<SpriteRenderer>().enabled = true;
        //    sz.transform.localScale = new Vector3(0.14f, 0.13f, 0);
        //}
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.localPosition = Vector3.one * 1000;

        }

        for (int i = 0; i < poolObjects.Length; i++)
        {

            float ftemp = Random.Range(scaleRange.scalemin, scaleRange.scalemax) / 10;

            poolObjects[i].transform.localScale = new Vector3(0.1f, 0.1f, 0);


        }

        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    void Update()
    {
        //  if (game.GameOver) return;
        Shift();
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;

        }
    }

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            float ftemp = Random.Range(scaleRange.scalemin, scaleRange.scalemax) / 10;

            // t.localScale.Scale()//Set(1, 0.5f, 1);
            t.localScale = new Vector3(0.1f, 0.1f, 0);
            poolObjects[i] = new PoolObject(t);

        }

        if (spawnImmediate)
        {
            SpawnImmediate();
        }

    }
    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return; //if true , this indicates that poolsize is to small
        Vector3 pos = Vector3.zero;
        //pos.x = ((player.transform.position.x + playerApartPosX) * Camera.main.aspect) / targetAspect;
        pos.x = (player.transform.position.x + playerApartPosX);
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;

    }

    void SpawnImmediate()
    {

        Transform t = GetPoolObject();
        if (t == null) return; //if true , this indicates that poolsize is to small
        Vector3 pos = Vector3.zero;
        //pos.x = (immediateSpawnPos.x * Camera.main.aspect) / targetAspect;
        pos.x = (player.transform.position.x + playerApartPosX);
        pos.y = Random.Range(ySpawnRange.max, ySpawnRange.max);
        t.position = pos;

        Spawn();
    }

    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject poolobject)
    {
        //if (poolobject.transform.position.x < ((-defaultSpawnPos.x * Camera.main.aspect) / targetAspect) - 8)
        //{
        //    poolobject.Dispose();
        //    poolobject.transform.position = Vector3.one * 1000;
        //    //Ladders[] ladders2 = this.GetComponentsInChildren<Ladders>();
        //    //for (int i = 0; i < ladders2.Length; i++)
        //    //{

        //    //    scorezone sz = ladders2[i].GetComponentInChildren<scorezone>();
        //    //    sz.GetComponent<SpriteRenderer>().enabled = true;
        //    //    sz.transform.localScale = new Vector3(0.14f, 0.13f, 0);
        //    //}

        //}
        if (poolobject.inUse)
        {
            poolobject.YMoveAmountTime += Time.deltaTime;

            if (poolobject.YMoveAmountTime <= 0.5f)
            {
                poolobject.transform.localPosition -= Vector3.right * shiftSpeed * Time.deltaTime;
            }
            else if (poolobject.YMoveAmountTime > 1.0f)
            {
                poolobject.transform.localPosition -= Vector3.right * shiftSpeed * Time.deltaTime;
                poolobject.YMoveAmountTime = 0.0f;
            }

            poolobject.disappearTime += Time.deltaTime;
            if (poolobject.disappearTime >= 18.0f)
            {
                poolobject.disappearTime = 0.0f;
                poolobject.Dispose();
                poolobject.transform.transform.position = Vector3.one * 1000;

            }
        }

    }

    Transform GetPoolObject()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }

        }
        return null;
    }


}
