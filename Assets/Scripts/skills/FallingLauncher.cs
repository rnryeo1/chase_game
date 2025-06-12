using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;

public class FallingLauncher : MonoBehaviour
{
    [SerializeField] GameObject m_goFalling = null;

    [SerializeField] Transform m_tFallingSpawn = null;
    // Start is called before the first frame update

    [SerializeField] bool isAutoSpawn = true;
    float spawnTime = 0.1f;
    private float timetoRespawn = 0.0f;
    // Update is called once per frame
    [SerializeField] LayerMask m_layerMask = 0;

    public Vector2 size;

    [SerializeField] int curStatCount = 1; //기본1개
    private int curstack;

    [SerializeField] float cooldownAmount = 0.2f;
    int cooldownCount = 7;
    int IncreaseskillCount = 20;
    int skillCount = 1;

    bool maxSkillCounted = false;

    bool m_bskillLearned = false;// 첫스킬은 false



    [SerializeField] TextMeshProUGUI SkillRemainText;

    [SerializeField] TextMeshProUGUI CoolRemainText;
    [SerializeField] bool onoffTest = false;
    private bool maxCooldownCounted = false;

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
            IncreaseskillCount = 20;
            skillCount = 1;
            m_bskillLearned = false;// 첫스킬은 false
            maxSkillCounted = false;
        }
    }

    private void Awake()
    {
        curstack = curStatCount;
    }
    bool SearchEnemy()
    {

        Collider2D[] t_cols = Physics2D.OverlapBoxAll(transform.position, size, 0, m_layerMask);
        if (t_cols.Length > 0)
        {
            if (curstack >= t_cols.Length)
            {

                curstack = t_cols.Length;
                if (curstack >= skillCount)
                {
                    curstack = skillCount;
                }

            }
            else
            {
                curstack = skillCount;
            }
            //return true;
            timetoRespawn += Time.deltaTime;
            if (isAutoSpawn && timetoRespawn >= spawnTime)
            {
                for (int i = 0; i < curstack; i++)
                {
                    if (t_cols[i] != null)
                    {
                        GameObject t_missile = Instantiate(m_goFalling, t_cols[i].transform.position, Quaternion.identity);
                        t_missile.transform.position = new Vector3(t_cols[i].transform.position.x, t_cols[i].transform.position.y + 1, t_cols[i].transform.position.z);
                        timetoRespawn = 0.0f;
                    }
                }
            }
        }
        return false;
    }
    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    void Update()
    {
        if (!onoffTest)
        {
            if (m_bskillLearned)
            {
                SearchEnemy();
            }
            SkillRemainText.text = skillCount.ToString() + "     " + IncreaseskillCount.ToString();
            CoolRemainText.text = cooldownCount.ToString();
        }
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
