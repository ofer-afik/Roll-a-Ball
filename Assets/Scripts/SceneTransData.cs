using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SceneTransData", menuName = "Scriptable Objects/SceneTransData")]
public class SceneTransData : ScriptableObject
{
    public StyleScale Scale = new Vector2(0f, 0f);
    public StyleScale AnimSpeed = new Vector2(0.1f, 0.1f);
}