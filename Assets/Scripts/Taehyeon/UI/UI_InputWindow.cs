using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Logger = Utils.Logger;

// This prefab must be active state before start scene
public class UI_InputWindow : MonoBehaviour
{
    private static UI_InputWindow _instance;

    private Button _okBtn;
    private Button _cancelBtn;
    private TMP_Text _titleText;
    private TMP_InputField _inputField;

    private void Awake()
    {
        _instance = this;
        
        _okBtn = transform.Find("Ok Btn").GetComponent<Button>();
        _cancelBtn = transform.Find("Cancel Btn").GetComponent<Button>();
        _titleText = transform.Find("Title Text").GetComponent<TMP_Text>();
        _inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
        
        Hide();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            _okBtn.onClick?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cancelBtn.onClick?.Invoke();
        }
    }
    private void Show(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string> onOk) {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        _titleText.text = titleString;

        _inputField.characterLimit = characterLimit;
        _inputField.onValidateInput = (string text, int charIndex, char addedChar) => {
            return ValidateChar(validCharacters, addedChar);
        };

        _inputField.text = inputString;
        _inputField.Select();
        
        _okBtn.onClick.RemoveAllListeners();
        _okBtn.onClick.AddListener(() => {
            Hide();
            onOk(_inputField.text);
        });

        _cancelBtn.onClick.RemoveAllListeners();
        _cancelBtn.onClick.AddListener(() => {
            Hide();
            onCancel();
        });
    }
    
    private char ValidateChar(string validCharacters, char addedChar) {
        if (validCharacters.IndexOf(addedChar) != -1) {
            // Valid
            return addedChar;
        } else {
            // Invalid
            return '\0';
        }
    }

    public static void Show_Static(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string> onOk) {
        _instance.Show(titleString, inputString, validCharacters, characterLimit, onCancel, onOk);
    }

    public static void Show_Static(string titleString, int defaultInt, Action onCancel, Action<int> onOk) {
        _instance.Show(titleString, defaultInt.ToString(), "0123456789-", 20, onCancel, 
            (string inputText) => {
                // Try to Parse input string
                if (int.TryParse(inputText, out int _i)) {
                    onOk(_i);
                } else {
                    onOk(defaultInt);
                }
            }
        );
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}