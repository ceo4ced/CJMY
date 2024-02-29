using UnityEngine;
using UnityEngine.UI; // Required for interacting with UI elements


public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject[] characters; // Assign in the Unity Editor
    private int selectedIndex = 0; // Default to the first character

    void Start()
    {
        HighlightCharacter(selectedIndex); // Highlight the default selected character at the start
    }

    // Method to highlight the selected character
    public void HighlightCharacter(int index)
    {
        float selectedScale = 1.2f; // Scale for the selected character
        float normalScale = 1f; // Scale for non-selected characters

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].transform.localScale = (i == index) ? new Vector3(selectedScale, selectedScale, selectedScale) : new Vector3(normalScale, normalScale, normalScale);
        }
    }

    // Example method to select a character (could be called by UI buttons)
    public void SelectCharacter(int characterIndex)
    {
        selectedIndex = characterIndex;
        HighlightCharacter(selectedIndex);
    }

    // Public method to confirm the selection
    public void ConfirmSelection()
    {
        // Implement what happens after confirmation. For example:
        Debug.Log("Character " + selectedIndex + " selected.");

        // If you're storing the selected character index for later use (e.g., in a game manager), do it here
        // GameManager.Instance.SetSelectedCharacter(selectedIndex);

        // Optionally, load the next scene or close the character selection menu
    }
}
