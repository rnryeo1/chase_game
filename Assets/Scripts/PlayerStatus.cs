using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;
    // Start is called before the first frame update

    public GameObject player;

    // Start is called before the first frame update
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] float Skillhp, maxSkillhp = 1f;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] SkillHealthBar SkillBar;

    [SerializeField] GameObject skillcanvas;
    [SerializeField] GameObject settingCanvas;

    private float lifestealAmount = 0.0f;

    private float lifestealAmountvalue = 0.01f;

    private int lifesteanlCount = 10;



    [SerializeField] TextMeshProUGUI TextLifeSteal;

    GameObject[] monObjs;
    GameObject[] skillObjs;
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage")
        {
            Debug.Log("확인111111");
            lifesteanlCount = 10;
            lifestealAmount = 0.0f;
            maxHealth = 3f;
            health = maxHealth;
            healthBar.UpdateHealthBar(health, maxHealth);
            maxSkillhp = 1.0f;
            Skillhp = 0.0f;
        }
    }

    void Awake()
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
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        SkillBar = GetComponentInChildren<SkillHealthBar>();
        skillcanvas = GameObject.Find("SkillUICanvas");
        skillcanvas.SetActive(false);
        settingCanvas = GameObject.Find("SettingCanvas");
        settingCanvas.SetActive(false);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        Skillhp = 0;
        SkillBar.UpdateSkillBar(Skillhp, maxSkillhp);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SkillItem")
        {
            UpSkillStat(0.1f);
        }
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }
    public void stealHealth()
    {
        health += lifestealAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }
    public void stealHealth(float count)
    {
        health += count + (0.1f * MainSceneManager.instance.getincreaseHp());
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }
    public void UpSkillStat(float SkillAmount)
    {
        Skillhp += SkillAmount + (0.01f + MainSceneManager.instance.getskillup());
        SkillBar.UpdateSkillBar(Skillhp, maxSkillhp);
        if (Skillhp >= maxSkillhp)
        {
            //스킬업
            skillcanvas.SetActive(true);
            Skillhp = 0;
            maxSkillhp += 1.0f; // 10개 더부수기.
            Time.timeScale = 0.0f;
        }
    }

    void Die()
    {
        //destroyResource();
        Timer.instance.saveTimeRecord();
        settingCanvas.SetActive(true);
        settingCanvas.GetComponent<SettingCanvas>().openResultCanvas();
        //SceneManager.LoadScene("MainScene");

    }
    public void ReviveFromAds()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        Time.timeScale = 1.0f;
        //settingCanvas.SetActive(true);
        //SceneManager.LoadScene("MainScene");

    }
    public void Restart()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        Time.timeScale = 1.0f;
        RewardAdsMgr.instance.setlifecount(3);
        //settingCanvas.SetActive(true);
        //SceneManager.LoadScene("MainScene");

    }
    public void destroyResource()
    {
        monObjs = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < monObjs.Length; i++)
        {
            Destroy(monObjs[i].gameObject);
        }
        skillObjs = GameObject.FindGameObjectsWithTag("SkillItem");
        for (int i = 0; i < skillObjs.Length; i++)
        {
            Destroy(skillObjs[i].gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TextLifeSteal.text = "Life Steal " + lifesteanlCount.ToString();
    }

    public GameObject getSkillUiCanvas()
    {
        return skillcanvas;
    }
    public SkillHealthBar getSkillbar()
    {
        return SkillBar;
    }
    public FloatingHealthBar getHealthbar()
    {
        return healthBar;
    }

    public void uplifestealAmount()
    {
        if (lifesteanlCount >= 1)//10번만
        {
            lifesteanlCount--;
            lifestealAmount += lifestealAmountvalue;  ///0.01 최대 0.1 
            skillcanvas.SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
        }
    }

    public float getmaxSkillhp()
    {
        return maxSkillhp;
    }
    public float getlifestealAmount()
    {
        return lifestealAmount;
    }

    public void setPlayerMaxHealth(float _value)
    {
        maxHealth = _value;
    }
}
