using UnityEngine;
using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class Backend : MonoBehaviour
{
    public static Backend instance;

    [SerializeField] private string url;
    [SerializeField] private string anonKey;
    public Client Client;
  
    public Session Session;
    public Users User;

    
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

    public async void SendData(string uid, int score, string displayName, int daysPlayed, int customersHelpedCorrectly,
        int customersHelpedWrongly)
    {
        var user = new Users
        {
            uid = uid,
            score = score,
            displayName = displayName,
            daysPlayed = daysPlayed,
            customersHelped = customersHelpedCorrectly,
            customersHelpedWrongly = customersHelpedWrongly,
        };
        await Client.From<Users>().Insert(user).ContinueWith(SendTask =>
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

    public async void SignUp(string email, string password, string displayName)
    {
        Session = await Client.Auth.SignUp(email, password);
        Debug.Log(Session.User.Id);
        SendData(Session.User.Id, 0, displayName, 0, 0, 0);
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
        FirebaseDatabase.DefaultInstance.RootReference.Child("scenarios").Child("1").GetValueAsync()
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
                    data = JsonUtility.FromJson<NpcData>(json);
                    target.npcData = data;
                }

                ;
            });
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