using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private EventReference _musicEventReference;
    private EventInstance _musicEventInstance;

    [Header("interaction")] [SerializeField]
    private GameObject _interactUI;

    [Header("dialogue")] [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Animator _dialogueAnimation;
    [SerializeField] private TextMeshProUGUI _speakertext;
    [SerializeField] private TextMeshProUGUI _dialoguetext;
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _buttonGroup;
    public bool inDialogue = false;
    private Dialogue _currentDialogue;
    private int _currentLineIndex;

    [Header("Quest")] [SerializeField] private TextMeshProUGUI _questTitle;
    [SerializeField] private TextMeshProUGUI _questText;
    public List<string> items;

    [Header("Pause")] [SerializeField] private GameObject _pauseUI;
    private bool _isPaused = false;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ShowInteractUI(false);
        _pauseUI.SetActive(false);

        _musicEventInstance = RuntimeManager.CreateInstance(_musicEventReference);
        _musicEventInstance.start();
        _musicEventInstance.setParameterByName("MusicStage", value: 2);
    }

    private void OnEnable()
    {
        _playerInput.actions.FindActionMap("Player").FindAction("Pause").performed += PauseAction;
        _playerInput.actions.FindActionMap("UI").FindAction("Pause").performed += PauseAction;
    }

    private void OnDisable()
    {
        if (_playerInput  == null){return;}
        
        _playerInput.actions.FindActionMap("Player").FindAction("Pause").performed -= PauseAction;
        _playerInput.actions.FindActionMap("UI").FindAction("Pause").performed -= PauseAction;
    }

    private void PauseAction(InputAction.CallbackContext obj)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _pauseUI.SetActive(_isPaused);
        
        if (_isPaused)
        {
            _playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }
    }
    

    public void ShowInteractUI(bool show)
    {
        _interactUI.SetActive(show);
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        _musicEventInstance.setParameterByName("MusicStage", value: 1);
        inDialogue = true;
        _currentDialogue = dialogue;
        _currentLineIndex = 0;
        _playerInput.SwitchCurrentActionMap("UI");
        ClearDialogueButtons();
        _speakertext.SetText("");
        _dialoguetext.SetText("");

        ShowInteractUI(false);
        ShowCurrentLine();
        Cursor.lockState = CursorLockMode.Confined;
        _dialogueAnimation.Play("DialogueShow");

    }

    public void HideDialogue()
    {
        _dialogueAnimation.Play("DialogueHide");
        Cursor.lockState = CursorLockMode.Locked;
        inDialogue = false;
        _currentDialogue.onEnd.Invoke();
        _playerInput.SwitchCurrentActionMap("Player");
        _musicEventInstance.setParameterByName("MusicStage", value: 2);
    }

    private void ShowCurrentLine()
    {
        Dialogueline dialogueline = _currentDialogue.dialoguelines[_currentLineIndex];

        if (dialogueline == null)
        {
            return;
        }

        ClearDialogueButtons();

        foreach (DialogueLineButton button in dialogueline.buttons)
        {
            GameObject buttonInstance = Instantiate(_buttonPrefab, _buttonGroup.transform);
            buttonInstance.GetComponent<DialogueButton>().Setup(button.text, button.buttonEvent);
        }

        _speakertext.SetText(dialogueline.speaker);
        _dialoguetext.SetText(dialogueline.text);
        dialogueline.lineevent.Invoke();

        StartCoroutine(FocusButton(dialogueline));
    }

    IEnumerator FocusButton(Dialogueline dialogueLine)
    {
        yield return new WaitForEndOfFrame();
        if (dialogueLine.buttons.Count > 0)
        {
            GameObject firstButton = _buttonGroup.transform.GetChild(0).gameObject;
            firstButton.GetComponent<Button>().Select();

            _continueButton.gameObject.SetActive(false);
        }
        else
        {
            _continueButton.gameObject.SetActive(true);
            _continueButton.Select();
        }
    }


    public void Nextline()
    {
        if (!inDialogue)
        {
            return;
        }

        _currentLineIndex++;
        if (_currentDialogue.dialoguelines.Count <= _currentLineIndex)
        {
            HideDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void ClearDialogueButtons()
    {
        foreach (Transform t in _buttonGroup.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void ShowNewDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
        _currentLineIndex = 0;

        ShowCurrentLine();
    }

    public void SetQuestTitle(string title)
    {
        _questTitle.SetText(title);
    }

    public void SetQuestText(string text)
    {
        _questText.SetText(text);
    }

    public void ClearQuest()
    {
        SetQuestTitle("");
        SetQuestText("");
    }

    public void AddItem(string item)
    {
        items.Add(item);
    }
    public void RemoveItem(string item)
    {
        items.Remove(item);
    }





}
    
    
