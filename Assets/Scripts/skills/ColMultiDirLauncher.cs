using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ColMultiDirLauncher : MonoBehaviour
{
    [SerializeField] GameObject m_colmularrow = null;
    //public GameObject m_recochet = null;
    [SerializeField] Transform m_colmulSpawn = null;
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

    GameObject player;


    [SerializeField] float cooldownAmount = 0.2f;
    int cooldownCount = 7;
    int IncreaseskillCount = 10;
    int skillCount = 1;

    bool m_bskillLearned = false;// 첫스킬은 false

    bool maxSkillCounted = false;
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
            IncreaseskillCount = 10;
            skillCount = 1;
            m_bskillLearned = false;// 첫스킬은 false
            maxSkillCounted = false;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                    if (m_colmularrow != null)
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
        //Vector2 bulDir = player.GetComponent<PlayerMove>().getTouchDirectionVector();
        GameObject go = Instantiate(m_colmularrow, m_colmulSpawn.position, Quaternion.identity);
        ColMultiDir mr = go.GetComponent<ColMultiDir>();
        mr.setSkillCount(skillCount);
        //mr.direction = bulDir;
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
