using UnityEngine;
using TMPro;

public class Legacy_Physics_Button : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int mode = PlayerPrefs.GetInt("useLegacyPhysics", 1);
        buttonText.text = (mode == 1) ? "Fixed Physics" : "Legacy Physics";
    }

    // Update is called once per frame
    void Update()
    {
        int mode = PlayerPrefs.GetInt("useLegacyPhysics", 1);
        buttonText.text = (mode == 1) ? "Fixed Physics" : "Legacy Physics";
    }
}
