using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour {
    [SerializeField] private SettingsPopup settingsPopup;
    private bool isOpen = false; 
    // Use this for initialization
    void Start () {
        settingsPopup.Close();
	}

    public void OnOpenSettings()
    {
        settingsPopup.Open();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
            {
                settingsPopup.Open();
                
            }
            else
            {
                settingsPopup.Close();
            }
            isOpen = !isOpen;
        }
    }
}
