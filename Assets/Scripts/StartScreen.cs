     using System.Collections;
using System.Collections.Generic;
using UnityEngine;
     using UnityEngine.SceneManagement;
     using UnityEngine.UI;

     public class StartScreen : MonoBehaviour
{
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private GameObject _optiontsUI;
    [SerializeField] private Button _mainUI_Focus;
    [SerializeField] private Slider _optionsUI_Focus;
    void Start()    
    {
     _mainUI.SetActive(true); 
     _optiontsUI.SetActive(false);
     _mainUI_Focus.Select();
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void ToggleOptionsUI()
    {
        _mainUI.SetActive(!_mainUI.activeSelf);
        _optiontsUI.SetActive(!_optiontsUI.activeSelf);
      
        if (_mainUI.activeSelf)
        {
            _mainUI_Focus.Select();
        }
        else
        {
            _optionsUI_Focus.Select();
        }
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    
}
