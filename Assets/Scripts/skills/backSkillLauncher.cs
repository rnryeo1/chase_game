using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class backSkillLauncher : MonoBehaviour
{
    [SerializeField] GameObject m_backarrow = null;
    //public GameObject m_recochet = null;
    [SerializeField] Transform m_backSpawn = null;
    // Start is called before the first frame update

    [SerializeField] bool isAutoSpawn = true;
    float spawnTime = 0.1f;
    private float timetoRespawn = 0.0f;
    // Update is called once per frame
    [SerializeField] LayerMask m_layerMask = 0;

    public Vector2 size;

    Vector2 direction;

    [SerializeField] int multiarrowCount = 1;
    Quaternion lookRotation;
    [SerializeField] float startAngle = 0f, endAngle = 360f;

    private Vector2 bulletMoveDirection;

    [SerializeField] float cooldownAmount = 0.2f;
    int cooldownCount = 7;
    int IncreaseskillCount = 12;
    int skillCount = 1;

    bool maxSkillCounted = false;
    bool m_bskillLearned = false;// 첫스킬은 false

    [SerializeField] TextMeshProUGUI SkillRemainText;
    [SerializeField] TextMeshProUGUI CoolRemainText;
    bool maxCooldownCounted = false;
    [SerializeField] bool onoffTest = false;

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage")
        {
            maxCooldownCounted = false;
            cooldownCount = 7;
            IncreaseskillCount = 12;
            skillCount = 1;
            m_bskillLearned = false;// 첫스킬은 false
            maxSkillCounted = false;
        }
    }


    void Start()
    {
        direction = (transform.localRotation * Vector2.right).normalized;
        lookRotation = Quaternion.Euler(Vector3.right);
    }



    void Update()
    {
        if (!onoffTest)
        {
            if (m_bskillLearned)
            {
                timetoRespawn += Time.deltaTime;
                if (isAutoSpawn && timetoRespawn >= spawnTime)
                {
                    if (m_backarrow != null)
                    {

                        shoot();
                        timetoRespawn = 0.0f;
                    }
                }
            }
        }
        SkillRemainText.text = skillCount.ToString() + "     " + IncreaseskillCount.ToString();
        CoolRemainText.text = cooldownCount.ToString();
    }

    public void shoot()
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
            GameObject go = Instantiate(m_backarrow, m_backSpawn.position, Quaternion.identity);
            backSkill mr = go.GetComponent<backSkill>();
            mr.direction = bulDir;
            angle += angleStep;
        }

        //angle += angleStep;
        //}

    }

    public void setcooldownAmount()
    {
        cooldownCount--;
        if (cooldownCount >= 1)
        {
            spawnTime -= cooldownAmount;
        }
        else
        {
            maxCooldownCounted = true;
        }
    }
    public void setskillUpgradeAmount()
    {
        IncreaseskillCount--; //최대개수 
        if (IncreaseskillCount >= 1)
        {
            skillCount++;
        }
        else
        {
            maxSkillCounted = true;
        }
    }

    public void setFirstSkill(bool _first)
    {
        m_bskillLearned = _first;
    }
    public bool getFirstSkill()
    {
        return m_bskillLearned;
    }
    public int getSkillCountStack()
    {
        return skillCount;
    }
    public bool getmaxSkillCounted()
    {
        return maxSkillCounted;
    }
    public bool getmaxCooldownCounted()
    {
        return maxCooldownCounted;
    }
}
