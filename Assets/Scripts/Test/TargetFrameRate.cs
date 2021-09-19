using UnityEngine;

class TargetFrameRate : MonoBehaviour
{
    [SerializeField] int fps = 30;

    void Start() => Application.targetFrameRate = fps;
}
