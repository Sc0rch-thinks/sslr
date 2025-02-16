using System.Collections;
using System.Collections.Generic;
using Supabase.Gotrue;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
    
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
    
    public Image profilePicture;
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

            if (Backend.instance.User != null)
            {
                UpdateProfileUI(Backend.instance.User);
            }
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
            var totalPeopleHelped = user.customersHelpedCorrectly + user.customersHelpedWrongly;
            var accuracy = totalPeopleHelped > 0? ((float)user.customersHelpedCorrectly/totalPeopleHelped)*100:0;
            
            Debug.Log($"Updating UI: {user.displayName}, Days Played: {user.daysPlayed}, Accuracy: {accuracy}%");
            
            usernameText.text = user.displayName;
            daysPlayedText.text = user.daysPlayed.ToString();
            peopleHelpedText.text = totalPeopleHelped.ToString();
            accuracyText.text = $"{accuracy:F2}%";
            
            GetProfilePicture();
        }
        else
        {
            Debug.LogError("User data is null. UI not updated");
        }
    }
    public void Logout()
    {
        Backend.instance.SignOut();
    }
    
    public void GetProfilePicture()
    {
        // profilePicture.sprite = Backend.instance.GetProfilePicture("https://fchobpauqasfebohuuam.supabase.co/storage/v1/object/public/Avatar//1739708594780-6156448458035283230_120.jpg");
        
        if(Backend.instance.User != null && !string.IsNullOrEmpty(Backend.instance.User.profilePictureUrl))
        Backend.instance.GetProfile(Backend.instance.User.profilePictureUrl, profilePicture);
    }

    public void LoadScene(string scene)
    {
        LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game");
    }
}
