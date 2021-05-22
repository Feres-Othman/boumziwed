using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethodes
{
    public static string RemoveQuotes(this string s)
    {
        s = s.Remove(0, 1);
        s = s.Remove(s.Length - 1, 1);

        return s;
    }

    /// <summary>
    /// the start is ^ ----
    /// the end is @ ---
    /// for the " use '
    /// </summary>
    public static string ToJSON(this string s)
    {
        s = s.Replace("^", "{");
        s = s.Replace("@", "}");
        s = s.Replace("'", "\"");

        return s;
    }

    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }


    public static void fit(this RawImage ri ,int width , int height )
    {
        //Debug.Log(ri.texture.width +"/"+ ri.texture.height);
        ri.GetComponent<RectTransform>().sizeDelta = ri.texture.width > ri.texture.height ?
            new Vector2((int)((float)ri.texture.width / ri.texture.height * height), height) :
            new Vector2(width , (int)((float)ri.texture.height / ri.texture.width * width));

        //Debug.Log(ri.GetComponent<RectTransform>().sizeDelta);

    }

    public static void fitIn(this RawImage ri, int width, int height)
    {
        //Debug.Log(ri.texture.width +"/"+ ri.texture.height);
        ri.GetComponent<RectTransform>().sizeDelta = ri.texture.width < ri.texture.height ?
            new Vector2((int)((float)ri.texture.width / ri.texture.height * height), height) :
            new Vector2(width, (int)((float)ri.texture.height / ri.texture.width * width));

        //Debug.Log(ri.GetComponent<RectTransform>().sizeDelta);
    }

}
