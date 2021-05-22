using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Common : Singleton<Common>
{

    public void load(int index)
    {
        StartCoroutine(LoadYourAsyncScene(index));
    }

    IEnumerator LoadYourAsyncScene(int index)
    {
        GetComponent<Animator>().Play("out");

        yield return new WaitForSeconds(.5f);

        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        //Slider sd = loading.GetComponentInChildren<Slider>(true);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //sd.value = asyncLoad.progress;

            yield return null;
        }
    }

}
