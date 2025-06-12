using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Ricochet : MonoBehaviour
{
    [SerializeField] GameObject m_recochet = null;
    //public GameObject m_recochet = null;
    [SerializeField] Transform m_recohetSpawn = null;
    // Start is called before the first frame update

    [SerializeField] bool isAutoSpawn = true;
    float spawnTime = 0.1f;
    private float timetoRespawn = 0.0f;
    // Update is called once per frame
    [SerializeField] LayerMask m_layerMask = 0;

    public Vector2 size;
    [SerializeField] float cooldownAmount = 0.2f;
    int cooldownCount = 7;
    int IncreaseskillCount = 10;
    int skillCount = 1;

    bool m_bskillLearned = false;// 첫스킬은 true
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
                        StartCoroutine(delaySpawn(0.1f));
                        timetoRespawn = 0.0f;
                    }
                }
            }
        }


        SkillRemainText.text = skillCount.ToString() + "     " + IncreaseskillCount.ToString();
        CoolRemainText.text = cooldownCount.ToString();

        //t_missile.GetComponent<Rigidbody2D>().velocity = Vector3.up * 1f;
        //}
    }

    IEnumerator delaySpawn(float waitTime)
    {
        if (m_recochet != null)
        {
            for (int i = 0; i < skillCount; i++)
            {
                yield return new WaitForSeconds(waitTime); //waitTime 만큼 딜레이후 다음 코드가 실행된다.
                GameObject t_recochet = Instantiate(m_recochet, m_recohetSpawn.position, Quaternion.identity);
            }
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
