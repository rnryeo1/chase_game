using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MissileLauncher : MonoBehaviour
{
    [SerializeField] GameObject m_goMissile = null;

    [SerializeField] Transform m_tfMissileSpawn = null;
    // Start is called before the first frame update

    [SerializeField] bool isAutoSpawn = true;
    float spawnTime = 0.1f;
    private float timetoRespawn = 0.0f;
    // Update is called once per frame
    [SerializeField] LayerMask m_layerMask = 0;

    [SerializeField] float cooldownAmount = 0.2f;
    int cooldownCount = 5;
    int IncreaseskillCount = 12;
    int skillCount = 1;
    bool maxSkillCounted = false;
    private bool maxCooldownCounted = false;
    bool m_bskillLearned = true;// 첫스킬은 true
    public Vector2 size;



    [SerializeField] TextMeshProUGUI SkillRemainText;
    [SerializeField] TextMeshProUGUI CoolRemainText;
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
            cooldownCount = 5;
            IncreaseskillCount = 12;
            skillCount = 1;
            m_bskillLearned = true;
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
        if (!onoffTest)
        {
            if (m_bskillLearned)
            {
                if (SearchEnemy())
                {
                    timetoRespawn += Time.deltaTime;
                    if (isAutoSpawn && timetoRespawn >= spawnTime)
                    {
                        // for (int i = 0; i < skillCount; i++)
                        // {
                        //     GameObject t_missile = Instantiate(m_goMissile, m_tfMissileSpawn.position, Quaternion.identity);
                        // }
                        StartCoroutine(delaySpawn(0.1f));
                        timetoRespawn = 0.0f;
                    }
                }

            }
        }

        SkillRemainText.text = skillCount.ToString() + "     " + IncreaseskillCount.ToString();
        CoolRemainText.text = cooldownCount.ToString();
    }
    IEnumerator delaySpawn(float waitTime)
    {

        for (int i = 0; i < skillCount; i++)
        {
            yield return new WaitForSeconds(waitTime); //waitTime 만큼 딜레이후 다음 코드가 실행된다.

            GameObject t_missile = Instantiate(m_goMissile, m_tfMissileSpawn.position, Quaternion.identity);
        }
        //While문을 빠져 나가지 못하여 waitTime마다 Shot함수를 반복실행 됩니다.
    }
    public void MissileAtkButton()
    {
        StartCoroutine(delaySpawn(0.01f));
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
