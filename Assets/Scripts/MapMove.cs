using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MapMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private RawImage _img;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (GameManager.instance.getisMainScene() == false)
        {

            if (player.GetComponent<PlayerMove>().ismvoing())
            {

                _img.uvRect = new Rect(_img.uvRect.position +
                new Vector2(player.GetComponent<PlayerMove>().joystick.Direction.x, player.GetComponent<PlayerMove>().joystick.Direction.y)
                 * Time.deltaTime * speed, _img.uvRect.size);

            }

        }
    }
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {

        }
        else if (scene.name == "Stage")
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }


}
