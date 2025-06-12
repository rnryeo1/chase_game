using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    public static SkillUI instance;
    public GameObject player;

    MissileLauncher skill1missile;
    [SerializeField] GameObject cooldownpage;
    [SerializeField] GameObject SkillUpgradePage;

    // Start is called before the first frame update
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

    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        skill1missile = player.GetComponentInChildren<MissileLauncher>();

        //cooldownpage = GameObject.Find("SkillUICanvas/CooldownPage");
        //cooldownpage.SetActive(false);
        SkillUpgradePage = GameObject.Find("SkillUICanvas/SkillUpgradePage");
        SkillUpgradePage.SetActive(false);
    }


    public void backcooldownpage()
    {
        cooldownpage.SetActive(false);
    }
    public void opencooldownpage()
    {
        cooldownpage.SetActive(true);
    }
    public void backSkillUpgradepage()
    {
        SkillUpgradePage.SetActive(false);
    }
    public void openSkillUpgradepage()
    {
        SkillUpgradePage.SetActive(true);
    }
}
