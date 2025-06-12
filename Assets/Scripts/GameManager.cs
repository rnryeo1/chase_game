using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int CurrentWaveNum = 1;


    public GameObject player;
    [SerializeField] GameObject skillcanvas;
    MissileLauncher skill1missile;
    Ricochet skill2ricochet;
    directionAreaLauncher drLuncher;
    ShieldLauncher shildLuncher;
    FallingLauncher fallingLauncher;
    backSkillLauncher backLuncher;
    SparkLauncher sparkLauncher;
    MultiArrowLauncher mrLuncher;

    ColMulAutoDiirLauncher colMulAutoDiirLauncher;
    ColMultiDirLauncher colMultiDirLauncher;

    ArrowOrbLauncher orbLauncher;
    private bool isMainScene = false;
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public bool getisMainScene()
    {
        return isMainScene;
    }
    public void setisMainScene(bool _value)
    {
        isMainScene = _value;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            Color tmp = player.GetComponent<SpriteRenderer>().color;
            tmp.a = 0f;
            player.GetComponent<SpriteRenderer>().color = tmp;

            player.GetComponent<PlayerStatus>().getSkillbar().GetComponent<Slider>().enabled = false;
            player.GetComponent<PlayerStatus>().getHealthbar().GetComponent<Slider>().enabled = false;

            //player.SetActive(false);
            isMainScene = true;
        }
        else if (scene.name == "Stage")
        {
            Color tmp = player.GetComponent<SpriteRenderer>().color;
            tmp.a = 255f;
            player.GetComponent<SpriteRenderer>().color = tmp;
            player.GetComponent<PlayerStatus>().getSkillbar().GetComponent<Slider>().enabled = true;
            player.GetComponent<PlayerStatus>().getHealthbar().GetComponent<Slider>().enabled = true;

            //player.SetActive(true);
            Time.timeScale = 1.0f;
            Timer.instance.resetTime();
            isMainScene = false;
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        skill1missile = player.GetComponentInChildren<MissileLauncher>();
        skill2ricochet = player.GetComponentInChildren<Ricochet>();
        drLuncher = player.GetComponentInChildren<directionAreaLauncher>();
        shildLuncher = player.GetComponentInChildren<ShieldLauncher>();
        fallingLauncher = player.GetComponentInChildren<FallingLauncher>();
        backLuncher = player.GetComponentInChildren<backSkillLauncher>();
        sparkLauncher = player.GetComponentInChildren<SparkLauncher>();
        mrLuncher = player.GetComponentInChildren<MultiArrowLauncher>();
        colMulAutoDiirLauncher = player.GetComponentInChildren<ColMulAutoDiirLauncher>();
        colMultiDirLauncher = player.GetComponentInChildren<ColMultiDirLauncher>();
        orbLauncher = player.GetComponentInChildren<ArrowOrbLauncher>();
        mrLuncher = player.GetComponentInChildren<MultiArrowLauncher>();


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

    public void donotlearn()
    {
        player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false);
        Time.timeScale = 1.0f;
    }



    public void missile_UpgradeStackUp()
    {
        if (skill1missile.getmaxSkillCounted() == false)
        {
            bool first = skill1missile.getFirstSkill();
            if (first)
            {
                skill1missile.setskillUpgradeAmount();
            }
            skill1missile.setFirstSkill(true);

            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backSkillUpgradepage();
        }


    }

    public void missileAttackSpeedUp()
    {
        if (skill1missile.getmaxCooldownCounted() == false)
        {
            skill1missile.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void ricochet_UpgradeStackUp()  ///ricohet 3개이상
    {
        if (skill2ricochet.getmaxSkillCounted() == false)
        {
            if (skill1missile.getSkillCountStack() >= 3)
            {

                bool first = skill2ricochet.getFirstSkill();
                if (first)
                {
                    skill2ricochet.setskillUpgradeAmount();
                }
                skill2ricochet.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }
        }

    }
    public void ricochetAttackSpeedUp()
    {
        if (skill2ricochet.getFirstSkill() == true && skill2ricochet.getmaxCooldownCounted() == false)
        {
            skill2ricochet.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void drLuncher_UpgradeStackUp() //direction
    {
        if (drLuncher.getmaxSkillCounted() == false)
        {
            if (skill1missile.getSkillCountStack() >= 3)
            {
                bool first = drLuncher.getFirstSkill();
                if (first)
                {
                    drLuncher.setskillUpgradeAmount();
                }
                drLuncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }

        }

    }
    public void drLuncherAttackSpeedUp()
    {
        if (drLuncher.getFirstSkill() == true && drLuncher.getmaxCooldownCounted() == false)
        {
            drLuncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void shildLuncher_UpgradeStackUp()
    {
        if (shildLuncher.getmaxSkillCounted() == false)
        {
            if (skill1missile.getSkillCountStack() >= 3) //shildLuncher
            {
                bool first = shildLuncher.getFirstSkill();
                if (first)
                {
                    shildLuncher.setskillUpgradeAmount();
                }
                shildLuncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }
        }
    }
    public void shildLuncherAttackSpeedUp()
    {
        if (shildLuncher.getFirstSkill() == true && shildLuncher.getmaxCooldownCounted() == false)
        {
            shildLuncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void falling_UpgradeStackUp()
    {
        if (fallingLauncher.getmaxSkillCounted() == false)
        {
            if (shildLuncher.getSkillCountStack() >= 3 && drLuncher.getSkillCountStack() >= 3 && skill2ricochet.getSkillCountStack() >= 3) // falling 
            {
                bool first = fallingLauncher.getFirstSkill();
                if (first)
                {
                    fallingLauncher.setskillUpgradeAmount();
                }
                fallingLauncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }
        }
    }

    public void fallingAttackSpeedUp()
    {
        if (fallingLauncher.getFirstSkill() == true && fallingLauncher.getmaxCooldownCounted() == false)
        {
            fallingLauncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }



    public void spark_UpgradeStackUp()
    {
        if (sparkLauncher.getmaxSkillCounted() == false)
        {
            if (shildLuncher.getSkillCountStack() >= 3 && drLuncher.getSkillCountStack() >= 3 && skill2ricochet.getSkillCountStack() >= 3)
            {
                bool first = sparkLauncher.getFirstSkill();
                if (first)
                {
                    sparkLauncher.setskillUpgradeAmount();
                }
                sparkLauncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }

        }

    }

    public void sparkAttackSpeedUp()
    {
        if (sparkLauncher.getFirstSkill() == true && sparkLauncher.getmaxCooldownCounted() == false)
        {
            sparkLauncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }
    public void mrLuncher_UpgradeStackUp()
    {
        if (mrLuncher.getmaxSkillCounted() == false)
        {
            if (shildLuncher.getSkillCountStack() >= 3 && drLuncher.getSkillCountStack() >= 3 && skill2ricochet.getSkillCountStack() >= 3)
            {
                bool first = mrLuncher.getFirstSkill();
                if (first)
                {
                    mrLuncher.setskillUpgradeAmount();
                }
                mrLuncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }

        }

    }
    public void mrLuncherAttackSpeedUp()
    {
        if (mrLuncher.getFirstSkill() == true && mrLuncher.getmaxCooldownCounted() == false)
        {
            mrLuncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void backLuncher_UpgradeStackUp()
    {
        if (backLuncher.getmaxSkillCounted() == false)
        {

            if (fallingLauncher.getSkillCountStack() >= 3 && sparkLauncher.getSkillCountStack() >= 3 &&
         mrLuncher.getSkillCountStack() >= 3)
            {
                bool first = backLuncher.getFirstSkill();
                if (first)
                {
                    backLuncher.setskillUpgradeAmount();
                }
                backLuncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }

        }

    }
    public void backLuncherAttackSpeedUp()
    {
        if (backLuncher.getFirstSkill() == true && backLuncher.getmaxCooldownCounted() == false)
        {
            backLuncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void colMulAuto_UpgradeStackUp()
    {
        if (colMulAutoDiirLauncher.getmaxSkillCounted() == false)
        {
            if (fallingLauncher.getSkillCountStack() >= 3 && sparkLauncher.getSkillCountStack() >= 3 &&
         mrLuncher.getSkillCountStack() >= 3)
            {
                bool first = colMulAutoDiirLauncher.getFirstSkill();
                if (first)
                {
                    colMulAutoDiirLauncher.setskillUpgradeAmount();
                }
                colMulAutoDiirLauncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }
        }
    }

    public void colMulAutoAttackSpeedUp()
    {
        if (colMulAutoDiirLauncher.getFirstSkill() == true && colMulAutoDiirLauncher.getmaxCooldownCounted() == false)
        {
            colMulAutoDiirLauncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }
    public void colMultiDir_UpgradeStackUp()
    {
        if (colMultiDirLauncher.getmaxSkillCounted() == false)
        {
            if (fallingLauncher.getSkillCountStack() >= 3 && sparkLauncher.getSkillCountStack() >= 3 &&
         mrLuncher.getSkillCountStack() >= 3)
            {
                bool first = colMultiDirLauncher.getFirstSkill();
                if (first)
                {
                    colMultiDirLauncher.setskillUpgradeAmount();
                }
                colMultiDirLauncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }

        }

    }
    public void colMultiDirAttackSpeedUp()
    {
        if (colMultiDirLauncher.getFirstSkill() == true && colMultiDirLauncher.getmaxCooldownCounted() == false)
        {
            colMultiDirLauncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }

    public void orbLauncher_UpgradeStackUp()
    {
        if (orbLauncher.getmaxSkillCounted() == false)
        {
            if (colMulAutoDiirLauncher.getSkillCountStack() >= 5 && colMultiDirLauncher.getSkillCountStack() >= 5)
            {
                bool first = orbLauncher.getFirstSkill();
                if (first)
                {
                    orbLauncher.setskillUpgradeAmount();
                }
                orbLauncher.setFirstSkill(true);
                player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
                Time.timeScale = 1.0f;
                SkillUI.instance.backSkillUpgradepage();
            }

        }

    }
    public void orbLauncherAttackSpeedUp()
    {
        if (orbLauncher.getFirstSkill() == true && orbLauncher.getmaxCooldownCounted() == false)
        {
            orbLauncher.setcooldownAmount();
            player.GetComponent<PlayerStatus>().getSkillUiCanvas().SetActive(false); //하나가잡고있으면 그잡고있는걸로계속써야하나보다
            Time.timeScale = 1.0f;
            SkillUI.instance.backcooldownpage();
        }
    }



}
