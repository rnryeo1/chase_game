using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;

    int curHp, maxHealth = 10;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        curHp = maxHealth;
        StartCoroutine(destbyTime());

    }
    IEnumerator destbyTime()
    {
        yield return new WaitForSeconds(60f);//60초간있을경우 제자리에서 삭제 
        Destroy(gameObject);
    }
    void Update()
    {
        healthBar.UpdateHealthBar(curHp, maxHealth);
    }
    public void TakeDamage(int damageAmount)
    {
        curHp -= damageAmount;
        healthBar.UpdateHealthBar(curHp, maxHealth);
    }

    public void setHp(int _curHp)
    {
        curHp = _curHp;
        if (curHp <= 0)
        {
            ItemSpawner.instance.dropskillItemandStealHealth(transform);
            Destroy(gameObject);
        }
    }
    public void setMaxHp(int _curHp)
    {
        maxHealth = _curHp;
    }
    public int getHp()
    {
        return curHp;
    }
}
