using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    public static Timer instance;

    public TextMeshProUGUI timerResultText;
    public TextMeshProUGUI timerRecordText;
    int minutes;
    int seconds;

    int minutesres;
    int secondsres;
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

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerResultText.text = "result :  " + minutes + " : " + seconds + "";
        float records = PlayerPrefs.GetFloat("TimeRecord");
        minutesres = Mathf.FloorToInt(records / 60);
        secondsres = Mathf.FloorToInt(records % 60);
        timerRecordText.text = "result :  " + minutesres + " : " + secondsres + "";
    }
    public void resetTime()
    {
        minutes = 0;
        seconds = 0;
        elapsedTime = 0.0f;
    }
    public int getcurMinutes()
    {
        return minutes;
    }
    public int getseconds()
    {
        return seconds;
    }

    public void saveTimeRecord()
    {
        float records = PlayerPrefs.GetFloat("TimeRecord");

        if (elapsedTime > records)
        {
            PlayerPrefs.SetFloat("TimeRecord", elapsedTime);
        }
    }
    public float getTimeRecord()
    {
        return elapsedTime;
    }

}
