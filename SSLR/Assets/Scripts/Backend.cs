/*
 * Author:Lin Hengrui Ryan, Livinia Poo
 * Date: 20/1/25
 * Description:
 * backend to talk to supabase and firebase
 */

using System.Threading.Tasks;
using UnityEngine;
using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class Backend : MonoBehaviour
{
    /// <summary>
    /// singleton instance
    /// </summary>
    public static Backend instance;

    /// <summary>
    /// url to supabase
    /// </summary>
    [SerializeField] private string url;

    /// <summary>
    /// supabase api anon key
    /// </summary>
    [SerializeField] private string anonKey;

    /// <summary>
    /// supabase client
    /// </summary>
    public Client Client;

    /// <summary>
    /// supabase auth session
    /// </summary>
    public Session Session;

    /// <summary>
    /// user data
    /// </summary>
    public Users User;

    /// <summary>
    /// menu buttons access
    /// </summary>
    public MenuButtons menuButtons;

    /// <summary>
    /// setting up supabase client
    /// </summary>
    private async void Start()
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true
        };


        Client = new Supabase.Client(url, anonKey, options);
        await Client.InitializeAsync().ContinueWith(task =>
        {
            // Check if the task is successful
            if (!task.IsCompletedSuccessfully)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                Debug.Log("Supabase Initialized");
            }
        });
    }

    /// <summary>
    /// signing out of supabase auth
    /// </summary>
    public async void SignOut()
    {
        User = null;
        Session = null;
        await Client.Auth.SignOut();
    }

    /// <summary>
    /// sending user data to supabase
    /// </summary>
    /// <param name="uid">the user id of the current player</param>
    /// <param name="profilePictureUrl">the url for the profile picture</param>
    /// <param name="score">total score</param>
    /// <param name="displayName">name of this player</param>
    /// <param name="daysPlayed">the number of days that this player has gone through</param>
    /// <param name="customersHelpedCorrectly"></param>
    /// <param name="customersHelpedWrongly"></param>
    public async void SendData(string uid, string profilePictureUrl, int score, string displayName, int daysPlayed,
        int customersHelpedCorrectly,
        int customersHelpedWrongly)
    {
        var user = new Users
        {
            uid = uid,
            profilePictureUrl = profilePictureUrl,
            score = score,
            displayName = displayName,
            daysPlayed = daysPlayed,
            customersHelpedCorrectly = customersHelpedCorrectly,
            customersHelpedWrongly = customersHelpedWrongly,
        };
        // Send the data to the database
        await Client.From<Users>().OnConflict(x => x.uid)
            .Upsert(user).ContinueWith(SendTask =>
            {
                if (!SendTask.IsCompletedSuccessfully)
                {
                    Debug.LogError(SendTask.Exception);
                }
                else
                {
                    Debug.Log("Data Sent Sucessfully");
                }
            });
    }

    /// <summary>
    /// sign in to supabase auth
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public async void SignIn(string email, string password)
    {
        Session = await Client.Auth.SignIn(email, password);
        Debug.Log(Session.User.Id);
        GetData(Session.User.Id);
        
        menuButtons.authPanel.SetActive(false);
        menuButtons.menuPanel.SetActive(true);
    }

    /// <summary>
    /// gettting user data from supabase
    /// </summary>
    /// <param name="uid">the uid for the player from auth</param>
    public async void GetData(string uid)
    {
        // Get the data from the database
        var result = await Client.From<Users>().Where(x => x.uid == uid).Get();
        User = result.Model;

        if (User != null)
        {
            Debug.Log($"User: " + User.displayName);

            MenuButtons profilePage = FindObjectOfType<MenuButtons>();
            if (profilePage != null)
            {
                Debug.Log("Updating UI...");
                profilePage.UpdateProfileUI(User);
            }
            else
            {
                Debug.Log("MenuButtons not found");
            }
        }
        else
        {
            Debug.Log("Data cannot retrieve");
        }
    }

    /// <summary>
    /// getting the npc data from firebase
    /// </summary>
    /// <param name="target">the npc script for desired npc</param>
    public void FirebaseGet(NpcMovementRework target)
    {
        NpcData data = new NpcData();
        FirebaseDatabase.DefaultInstance.RootReference.Child("scenarios")
            .Child(UnityEngine.Random.Range(1, 8).ToString())
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string json = snapshot.GetRawJsonValue();

                    if (!string.IsNullOrEmpty(json))
                    {
                        target.npcData = JsonUtility.FromJson<NpcData>(json);
                        target.correctService = target.npcData.correctDepartment;
                        Debug.Log($"NPC Loaded: {target.npcData.initialStatement}");

                        if (GameManager.instance.currentNPC == target.gameObject)
                        {
                            GameManager.instance.currentNPCCorrectDepartment = target.correctService;
                            target.LoadNPCDialogue();
                        }
                    }
                    else
                    {
                        Debug.LogError("Firebase returned empty");
                    }
                }

                ;
            });
    }

    /// <summary>
    /// getting the profile picture from the url
    /// </summary>
    /// <param name="url"></param>
    /// <param name="targetRenderer">the targeted ui image</param>
    public async void GetProfile(string url, Image targetRenderer)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("Profile picture URL is empty");
            return;
        }

        try
        {
            Texture2D texture = await GetTextureFromURL(url);

            if (texture != null)
            {
                if (targetRenderer != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    targetRenderer.sprite = sprite;
                    Debug.Log("Texture applied successfully.");
                }
                else
                {
                    Debug.LogError("Target Renderer is not assigned.");
                }
            }
            else
            {
                Debug.LogError("Failed to load texture.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error downloading texture: {e.Message}");
        }
    }

    /// <summary>
    /// getting the texture from the url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private async Task<Texture2D> GetTextureFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield(); // Yield until the operation is complete
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogError($"Error in UnityWebRequest: {request.error}");
                return null;
            }
        }
    }

    /// <summary>
    /// assigning the singleton instance
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}