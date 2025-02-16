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
    public static Backend instance;

    [SerializeField] private string url;
    [SerializeField] private string anonKey;
    public Client Client;

    public Session Session;
    public Users User;

    public Image profilePicture;
    private async void Start()
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true
        };


        Client = new Supabase.Client(url, anonKey, options);
        await Client.InitializeAsync().ContinueWith(task =>
        {
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

    public async void SignOut()
    {
        User = null;
        Session = null;
        await Client.Auth.SignOut();
    }

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


    public async void SignIn(string email, string password)
    {
        Session = await Client.Auth.SignIn(email, password);
        Debug.Log(Session.User.Id);
        GetData(Session.User.Id);
    }

    public async void GetData(string uid)
    {
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

    public void FirebaseGet(NpcMovementRework target)
    {
        NpcData data = new NpcData();
        FirebaseDatabase.DefaultInstance.RootReference.Child("scenarios").Child(UnityEngine.Random.Range(1, 8).ToString())
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