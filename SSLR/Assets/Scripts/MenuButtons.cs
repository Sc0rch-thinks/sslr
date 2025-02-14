using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    /// <summary>
    /// Assigning Variables
    /// </summary>
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI daysPlayedText;
    public TextMeshProUGUI peopleHelpedText;
    public TextMeshProUGUI accuracyText;

    /// <summary>
    /// Calling backend to log player in
    /// </summary>
    public void Login()
    {
        Backend.instance.SignIn(loginEmailInput.text, loginPasswordInput.text);
    }
}
