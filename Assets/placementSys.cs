using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class placementSys : MonoBehaviour
{
    public Sprite[] sprites;

    public Image[] images;

    public int currentImage = 0;
    public int nextScene = 18;

    public void place(int i)
    {
        images[currentImage].sprite = sprites[i];
        images[currentImage].color = new Color(1, 1, 1, 1);

        currentImage++;

        if(currentImage == 3)
        {
            FindObjectOfType<SceneController>().load(nextScene,3);
        }
    }

}
