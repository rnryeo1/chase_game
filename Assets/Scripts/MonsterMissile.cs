using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMissile : MonoBehaviour
{
    Rigidbody2D m_rigid = null;

    float m_speed = 6.5f;


    public Vector2 size;
    public float angle;
    public Vector3 direction = new Vector3(1, 0);
    [SerializeField] int m_damageStack = 1;
    public GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        m_rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(LaunchDelay());
    }

    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitForSeconds(5f);//5초간있을경우 제자리에서 삭제 
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        float vx = direction.x * m_speed;
        float vy = direction.y * m_speed;
        m_rigid.linearVelocity = new Vector2(vx, vy);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            player.GetComponent<PlayerStatus>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
