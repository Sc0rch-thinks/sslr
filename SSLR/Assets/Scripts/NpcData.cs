/*
 * Author: Lin Hengrui Ryan
 * Date: 12/2/25
 * Description:
 * Npc Data
 */

using System;
using UnityEngine.Serialization;

[Serializable]
public class NpcData
{
    public string name;
    public string initialStatement;
    public string question1;
    public string question2;
    public string question3;
    public string answer1;
    public string answer2;
    public string answer3;
    public string response1;
    public string response2;
    public string response3;
    public int points;
    public bool hasPaper;
    public string correctDepartment;
}