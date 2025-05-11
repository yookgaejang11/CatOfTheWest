using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isTIme = false;
    public TextMeshProUGUI timer;
    public bool StageClear = false;                         //스테이지 클리어
    public bool StageOver = false;                          //스테이지 실패
    private static GameManager instance;
    public float overTime;                                  //오버타임
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        
        if(!StageClear && !StageOver && !isTIme)
        {
            isTIme = true;
            timer.text = "Time\n" + overTime;
            overTime -=1;

            if( overTime < 0)
            {
                overTime = 0;
                StageOver = true;
            }
            yield return new WaitForSeconds(1);
            isTIme=false;
        }
    }

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }


}
