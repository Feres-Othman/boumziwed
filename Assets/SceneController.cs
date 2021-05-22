using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool isAutoTransition = false;
    public TextMeshProUGUI tt;
    public int time;
    public int sceneI;

    private void Start()
    {
        if (isAutoTransition)
        {
            this.Invoke(() => { load(sceneI); }, time);
            StartCoroutine(startTimer(time));
            
        }
    }

    IEnumerator startTimer(int time)
    {
        while (time > 0)
        {
            time--;
            tt.text = $"{(time/60).ToString("d2")}:{(time%60).ToString("d2")}";
            yield return new WaitForSeconds(1);
        }
    }

    public void load(int sceneIndex)
    {
        Common.instance.load(sceneIndex);
    }

    public void load(int sceneIndex, int offset)
    {
        this.Invoke(() => { load(sceneIndex); }, offset);
    }
}
