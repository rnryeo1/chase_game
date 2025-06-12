using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 throwVector;
    Rigidbody2D _rb;
    LineRenderer _lr;
    GameObject enemy;

    private float throwPower = 0.13f;
    private bool m_bstartThrow = false;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        //_lr = this.GetComponent<LineRenderer>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");

    }
    void Update()
    {
        if (m_bstartThrow)
        {
            CalculateThrowVector();
            //SetArrow();
            Throw();
            m_bstartThrow = false;
        }

    }
    void setStartThrow(bool _bstartThrow)
    {
        m_bstartThrow = _bstartThrow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!m_bstartThrow)
            { m_bstartThrow = true; }
        }
    }

    void StartThrow()
    {
        //RemoveArrow();
        Throw();
    }
    void CalculateThrowVector()
    {

        Vector3 Pos = enemy.transform.position;
        //doing vector2 math to ignore the z values in our distance.
        Vector2 distance = Pos - this.transform.position;
        //dont normalize the ditance if you want the throw strength to vary
        throwVector = distance.normalized * 100;
    }
    // void SetArrow()
    // {
    //     _lr.positionCount = 2;
    //     _lr.SetPosition(0, this.transform.position);
    //     _lr.SetPosition(1, throwVector.normalized / 2);
    //     _lr.enabled = true;
    //     Debug.Log("SET ");
    // }
    void OnMouseUp()
    {
        //RemoveArrow();
        Throw();
    }
    // void RemoveArrow()
    // {
    //     _lr.enabled = false;
    // }
    public void Throw()
    {
        //_rb.AddForce(throwVector);
        _rb.linearVelocity = new Vector2(transform.localScale.x + throwVector.x, transform.localScale.y + throwVector.y) * throwPower;
    }
}
