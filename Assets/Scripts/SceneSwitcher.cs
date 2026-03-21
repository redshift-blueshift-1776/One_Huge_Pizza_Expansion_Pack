using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void switchScenes(int i) {
        SceneManager.LoadScene(i);
    }

    public void toggleLegacyPhysics()
    {
        int mode = PlayerPrefs.GetInt("useLegacyPhysics", 1);
        // Debug.Log("useLegacyPhysics was: " + mode);
        PlayerPrefs.SetInt("useLegacyPhysics", 1 - mode);
    }
}
