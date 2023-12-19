using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsPanel; // Assign in the inspector

    // Function to load the next scene
    public void GoToNextScene()
    {
        // Assuming you want to go to the next scene in the build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Function to show the instruction panel
    public void ShowInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
    }

    // Function to hide the instruction panel
    public void HideInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }
}