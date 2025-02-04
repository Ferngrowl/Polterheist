using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Data/Scene Data", order = 1)]
public class SceneData : ScriptableObject
{
    public string previousScene; // Stores the name of the last scene
}
