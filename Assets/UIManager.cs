using UnityEngine;
using UnityEngine.EventSystems; // Required for Event Systems

public class UIManager : MonoBehaviour
{
    public GameObject yourDefaultButton; // Assign in the inspector

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(yourDefaultButton);
    }
}