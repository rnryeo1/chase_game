using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteAnimation : MonoBehaviour
{


    public SpriteRenderer sr;
    public Sprite[] m_SpriteArray;
    private float m_Speed = 0.1f;

    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Func_PlayUIAnim();


    }
    public void Func_PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(Func_PlayAnimUI());

    }
    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
            Destroy(gameObject);
        }
        sr.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsDone == false)
        {
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
        }

    }
}
