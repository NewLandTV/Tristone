using System;
using UnityEngine;

[CreateAssetMenu(fileName = "My Talk Data", menuName = "Data/Talk")]
public class TalkData : ScriptableObject
{
    [Serializable]
    public class Group
    {
        public string name;
        public string message;
    }

    public Group[] groups;
}
