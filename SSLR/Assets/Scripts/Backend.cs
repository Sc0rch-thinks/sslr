using Postgrest;
using Postgrest.Attributes;
using Postgrest.Models;
using UnityEngine;
using Supabase;

public class Backend : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private string anonKey;

    private async void Start()
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true
        };

        var client = new Supabase.Client(url, anonKey, options);
        await client.InitializeAsync();
        var test = new Test
        {
            Name = "John",
            Score = 100
        };
        await client.From<Test>().Insert(test, new QueryOptions { Returning = QueryOptions.ReturnType.Representation })
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var result = task.Result;
                    Debug.Log(result.Models[0].Name);
                }
                else
                {
                    Debug.LogError(task.Exception);
                }
                
            });
    }
}

[Table("test")]
public class Test : BaseModel
{
    [Column("name")] public string Name { get; set; }

    [Column("score")] public int Score { get; set; }
}