using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_switch : MonoBehaviour
{
    public GameObject Img;
    public void scene_changer(string scene_name) 
    {
        StartCoroutine(Fade(scene_name));
    }
    public IEnumerator Fade(string scene_name) 
    {
        for (int i = 0; i < 100; i++) {
            Color tmp = Img.GetComponent<Image>().color;
            tmp.a += 0.01f;
            Img.GetComponent<Image>().color = tmp;
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene(scene_name);
    }
}
