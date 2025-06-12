using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHealthBar : MonoBehaviour
{
    [SerializeField] private Slider silder;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    public void UpdateSkillBar(float currentValue, float maxValue)
    {
        silder.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = camera.transform.rotation;
        //transform.position = target.position + offset;
    }
}
