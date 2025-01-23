using Postgrest.Attributes;
using Postgrest.Models;

[Table("test")]
public class Test : BaseModel
{
    [Column()] public string name { get; set; }
    [Column()] public int score { get; set; }
}

[Table("users")]
public class Users : BaseModel
{
    

    [Column()] public string uid { get; set; }
    [Column()] public int score { get; set; }
    [Column()] public string displayName { get; set; }
    [Column()] public int daysPlayed { get; set; }
    [Column()] public int customersHelped { get; set; }
    [Column()] public int customersHelpedWrongly { get; set; }
}