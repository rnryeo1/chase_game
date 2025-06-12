using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


public class IAPManager : MonoBehaviour
{
    public static IAPManager instance;
    string log;
    [SerializeField] private string cash_01;




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
    }
    public void OnPurchaseComplete(Product product)
    {

        if (product.definition.id == cash_01)
        {
            Debug.Log("You just bought cash_01");
            int valcur = MainSceneManager.instance.getCashval();
            valcur += 1;
            MainSceneManager.instance.setCashval(valcur);
            // GPGSBinder.Inst.SaveCloud("cashdata", valcur.ToString(), success => log = $"{success}"); //cashdata 가 저장된다. 
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log(product.definition.id + "failed as a result of " + failureDescription);
    }
    public void OnTransactionsRestored(bool success, string error)
    {
        Debug.Log("OnTransactionsRestored " + success);
        if (success)
        {
            PlayerPrefs.SetInt("Adsonoff", 1);
        }
        Debug.Log("OnTransactionsRestored " + error);

    }
}
