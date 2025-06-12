using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager instance;

    public TextMeshProUGUI textCash;
    public TextMeshProUGUI textCashShop;
    private int cashValue = 0;

    private string loadcashdata;

    private bool isConnect;
    string log;

    GameObject IAPStore;
    GameObject m_Info;
    private int movespeed = 0;
    private int skillup = 0;
    private int increaseHp = 0;

    public TextMeshProUGUI text_movespeed;
    public TextMeshProUGUI text_skillup;
    public TextMeshProUGUI text_increaseHp;
    private void Awake()
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
        //GPGSBinder.Inst.Login((success, localUser) =>
        //   log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");
    }

    void Start()
    {
        IAPStore = GameObject.Find("IAPStore");
        m_Info = GameObject.Find("InfoScroll");
        //textCashShop = GameObject.Find("MainSceneManager/Canvas/IAPStore/Scroll/Panel/Content1/CashText").GetComponent<TextMeshProUGUI>();

        IAPStore.SetActive(false);
        m_Info.SetActive(false);
        cashValue = PlayerPrefs.GetInt("cashValue");
        textCash.text = cashValue.ToString();
        textCashShop.text = cashValue.ToString();
        movespeed = PlayerPrefs.GetInt("movespeed");
        skillup = PlayerPrefs.GetInt("skillup");
        increaseHp = PlayerPrefs.GetInt("increaseHp");

    }

    void Update()
    {
        textCash.text = cashValue.ToString();
        textCashShop.text = cashValue.ToString();
        text_movespeed.text = "Movement Speed Up : " + movespeed.ToString();
        text_skillup.text = "Skill Up Increase : " + skillup.ToString();
        text_increaseHp.text = "Increase Hp : " + increaseHp.ToString();
        //cashValue.text
    }

    public int getMoveSpeed() { return movespeed; }
    public void MoveSpeedUp() { if (movespeed < 10 && cashValue > 0) { movespeed++; cashValue--; PlayerPrefs.SetInt("movespeed", movespeed); PlayerPrefs.SetInt("cashValue", cashValue); } }
    public void MoveSpeedDown() { if (movespeed >= 1) { movespeed--; cashValue++; PlayerPrefs.SetInt("movespeed", movespeed); PlayerPrefs.SetInt("cashValue", cashValue); } }
    public int getskillup() { return skillup; }
    public void skillupUp() { if (skillup < 10 && cashValue > 0) { skillup++; cashValue--; PlayerPrefs.SetInt("skillup", skillup); PlayerPrefs.SetInt("cashValue", cashValue); } }
    public void skillupDown() { if (skillup >= 1) { skillup--; cashValue++; PlayerPrefs.SetInt("skillup", skillup); PlayerPrefs.SetInt("cashValue", cashValue); } }
    public int getincreaseHp() { return increaseHp; }
    public void increaseHpUp() { if (increaseHp < 10 && cashValue > 0) { increaseHp++; cashValue--; PlayerPrefs.SetInt("increaseHp", increaseHp); PlayerPrefs.SetInt("cashValue", cashValue); } }
    public void increaseHpDown() { if (increaseHp >= 1) { increaseHp--; cashValue++; PlayerPrefs.SetInt("increaseHp", increaseHp); PlayerPrefs.SetInt("cashValue", cashValue); } }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //CheckConnectInternet();
        if (scene.name == "MainScene")
        {
            transform.GetChild(0).gameObject.SetActive(true);

            if (isConnect) //인터넷안되도 클라우드 폰에서 접근 가능한가 확인. 그러면 인터넷접속여부 확인할 필요없음.
            {
                //textCash = GameObject.Find("MainSceneManager/Canvas/Cash/CashText").GetComponent<TextMeshProUGUI>();
                //GPGSBinder.Inst.LoadCloud("cashdata", (success, loadcashdata) => log = $"{success}, {loadcashdata}");
                //cashValue = int.Parse(loadcashdata);
                textCash.text = cashValue.ToString();
            }
            else
            {
                textCash.text = cashValue.ToString();
            }

        }
        else if (scene.name == "Stage")
        {

        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("확인 컬백인지");
        //GPGSBinder.Inst.SaveCloud("cashdata", cashValue.ToString(), success => log = $"{success}"); //want data 가 저장된다. 
        //아니면 구매시 , 소모시 바로 세이브 주기적으로 , 아니면 버튼만들어서 세이브 2가지방법이있다. 
    }


    public bool CheckConnectInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 인터넷 연결이 안되었을때
            isConnect = false;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            // 데이터로 인터넷 연결이 되었을때
            isConnect = true;
        }
        else
        {
            // 와이파이로 연결이 되었을때
            isConnect = true;
        }
        return isConnect;
    }

    // Start is called before the first frame update


    // Update is called once per frame

    public void gotoStage()
    {
        Time.timeScale = 1.0f;
        transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadScene("Stage");
    }

    public void setCashval(int _count)
    {
        cashValue = _count;
        PlayerPrefs.SetInt("cashValue", cashValue);
    }
    public int getCashval()
    {
        return cashValue;
    }

    public void openiapstore()
    {
        IAPStore.SetActive(true);

    }
    public void closeiapstore()
    {
        IAPStore.SetActive(false);
    }


    public void openInfo()
    {
        m_Info.SetActive(true);
    }
    public void closeInfo()
    {
        m_Info.SetActive(false);
    }
    public void closeGame()
    {
        Application.Quit();
    }
    void OnGUI()
    {
        // GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 3);


        // if (GUILayout.Button("ClearLog"))
        //     log = "";

        // if (GUILayout.Button("Login"))
        //     GPGSBinder.Inst.Login((success, localUser) =>
        //     log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");

        // if (GUILayout.Button("Logout"))
        //     GPGSBinder.Inst.Logout();

        // if (GUILayout.Button("SaveCloud"))
        //     GPGSBinder.Inst.SaveCloud("cashdata", cashValue.ToString(), success => log = $"{success}"); //want data 가 저장된다. 

        // if (GUILayout.Button("LoadCloud"))
        //     GPGSBinder.Inst.LoadCloud("cashdata", (success, data) => log = $"{success}, {data}"); //want data 가 로드된다. 

        // if (GUILayout.Button("DeleteCloud"))
        //     GPGSBinder.Inst.DeleteCloud("mysave", success => log = $"{success}");

        // if (GUILayout.Button("ShowAchievementUI"))
        //     GPGSBinder.Inst.ShowAchievementUI();

        // if (GUILayout.Button("UnlockAchievement_one"))
        //     //  GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_one, success => log = $"{success}");

        //     if (GUILayout.Button("UnlockAchievement_two"))
        //         //   GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_two, success => log = $"{success}");

        //         if (GUILayout.Button("IncrementAchievement_three"))
        //             // GPGSBinder.Inst.IncrementAchievement(GPGSIds.achievement_three, 1, success => log = $"{success}");

        //             if (GUILayout.Button("ShowAllLeaderboardUI"))
        //                 GPGSBinder.Inst.ShowAllLeaderboardUI();

        // if (GUILayout.Button("ShowTargetLeaderboardUI_num"))
        //     //  GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_num);

        //     if (GUILayout.Button("ReportLeaderboard_num"))
        //         // GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_num, 1000, success => log = $"{success}");

        //         if (GUILayout.Button("LoadAllLeaderboardArray_num"))
        //             // GPGSBinder.Inst.LoadAllLeaderboardArray(GPGSIds.leaderboard_num, scores =>
        //             // {
        //             //     log = "";
        //             //     for (int i = 0; i < scores.Length; i++)
        //             //         log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
        //             // });

        //             if (GUILayout.Button("LoadCustomLeaderboardArray_num"))
        //                 // GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard_num, 10,
        //                 //     GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, GooglePlayGames.BasicApi.LeaderboardTimeSpan.Daily, (success, scoreData) =>
        //                 //     {
        //                 //         log = $"{success}\n";
        //                 //         var scores = scoreData.Scores;
        //                 //         for (int i = 0; i < scores.Length; i++)
        //                 //             log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
        //                 //     });

        //                 if (GUILayout.Button("IncrementEvent_event"))
        //                     //GPGSBinder.Inst.IncrementEvent(GPGSIds.event_event, 1);

        //                     if (GUILayout.Button("LoadEvent_event"))
        //                         // GPGSBinder.Inst.LoadEvent(GPGSIds.event_event, (success, iEvent) =>
        //                         // {
        //                         //     log = $"{success}, {iEvent.Name}, {iEvent.CurrentCount}";
        //                         // });

        //                         if (GUILayout.Button("LoadAllEvent"))
        //                             GPGSBinder.Inst.LoadAllEvent((success, iEvents) =>
        //                             {
        //                                 log = $"{success}\n";
        //                                 foreach (var iEvent in iEvents)
        //                                     log += $"{iEvent.Name}, {iEvent.CurrentCount}\n";
        //                             });

        // GUILayout.Label(log);
    }
}
