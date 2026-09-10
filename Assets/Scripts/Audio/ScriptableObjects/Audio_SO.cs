using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Audio_SO", menuName = "ScriptableObjects/Audio_SO")]
public class Audio_SO : ScriptableObject
{
    public List<AudioData> audioDataList;
}
