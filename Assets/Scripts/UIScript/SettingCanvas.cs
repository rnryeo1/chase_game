using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingCanvas : MonoBehaviour
{
    public static SettingCanvas instance;
    GameObject player;
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

        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void opensettingCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void backtoMainscene()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        player.GetComponent<PlayerStatus>().destroyResource(); //리소스정리 몬스터 
        SceneManager.LoadScene("MainScene");

    }
    public void LoadadRestart()
    {
        RewardAdsMgr.instance.LoadRewardedAd();
        // transform.GetChild(0).gameObject.SetActive(false);
        // transform.GetChild(1).gameObject.SetActive(false);
        // SceneManager.LoadScene("Stage");

    }

    public void RestartButton()
    {

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        SceneManager.LoadScene("Stage");

    }

    public void watchedRewardAds()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        player.GetComponent<PlayerStatus>().ReviveFromAds(); //리소스정리 몬스터

        int lifecnt3 = RewardAdsMgr.instance.getlifecount();
        lifecnt3--;
        RewardAdsMgr.instance.setlifecount(lifecnt3);
        //SceneManager.LoadScene("Stage");
    }

    public void resumegame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void openResultCanvas()
    {
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

}
