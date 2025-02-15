using System.Collections;
using System.Collections.Generic;
using Supabase.Gotrue;
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

    /// <summary>
    /// Loads user profile from Supabase
    /// </summary>
    public void LoadProfileData()
    {
        if (Backend.instance.Session != null)
        {
            Backend.instance.GetData(Backend.instance.Session.User.Id);
        }
        else
        {
            Debug.Log("Session null, no user logged in");
        }
    }

    public void UpdateProfileUI(Users user)
    {
        if (user != null)
        {
            var totalPeopleHelped = user.customersHelped + user.customersHelpedWrongly;
            var accuracy = (user.customersHelped/totalPeopleHelped)*100;
            
            usernameText.text = user.displayName;
            daysPlayedText.text = user.daysPlayed.ToString();
            peopleHelpedText.text = totalPeopleHelped.ToString();
            accuracyText.text = accuracy.ToString();
        }
    }

}
