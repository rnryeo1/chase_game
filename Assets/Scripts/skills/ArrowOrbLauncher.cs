using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;


public class ArrowOrbLauncher : MonoBehaviour
{
    [SerializeField] GameObject m_goOrb = null;

    [SerializeField] Transform m_orbSpawnPos = null;
    // Start is called before the first frame update

    [SerializeField] bool isAutoSpawn = true;
    float spawnTime = 0.1f;
    private float timetoRespawn = 0.0f;
    // Update is called once per frame
    [SerializeField] LayerMask m_layerMask = 0;

    public Vector2 size;

    [SerializeField] float cooldownAmount = 0.2f;
    int cooldownCount = 7;
    int IncreaseskillCount = 9;
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
            IncreaseskillCount = 9;
            skillCount = 1;
            m_bskillLearned = false;// 첫스킬은 false
            maxSkillCounted = false;
        }
    }

    bool SearchEnemy()
    {

        Collider2D[] t_cols = Physics2D.OverlapBoxAll(transform.position, size, 0, m_layerMask);
        if (t_cols.Length > 0)
        {
            return true;
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
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        if (!onoffTest)
        {
            if (m_bskillLearned)
            {
                if (SearchEnemy())
                {
                    timetoRespawn += Time.deltaTime;
                    if (isAutoSpawn && timetoRespawn >= spawnTime)
                    {
                        GameObject t_missile = Instantiate(m_goOrb, m_orbSpawnPos.position, Quaternion.identity);
                        t_missile.GetComponent<ArrowOrb>().setSkillCount(skillCount);
                        timetoRespawn = 0.0f;
                    }
                }
            }
        }
        //t_missile.GetComponent<Rigidbody2D>().velocity = Vector3.up * 1f;
        //}
        SkillRemainText.text = skillCount.ToString() + "     " + IncreaseskillCount.ToString();
        CoolRemainText.text = cooldownCount.ToString();
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

    public bool getmaxSkillCounted()
    {
        return maxSkillCounted;
    }
    public bool getmaxCooldownCounted()
    {
        return maxCooldownCounted;
    }
}
