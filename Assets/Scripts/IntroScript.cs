using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text dialogueText;

    [Header("Ghouldon References (set inactive at start)")]
    public GameObject ghouldon1;
    public GameObject ghouldon2;
    private bool ghouldonActivated = false;

    [Header("Scene Lines")]
    [TextArea(2, 5)]
    public string[] dialogueLines =
    {
        "You are an aspiring witch researcher working on your PhD in potion studies.",
        "After months of research, you finally submitted your paper to your dream conference, the Witch Coven L'association, or WCL.",
        "To your delight, you were accepted. You eagerly packed your favorite potion ingredients: human eyeballs, tomatolings, and worms, and set off for Transylvania.",
        "After a long trip through dark forests and foggy roads, you followed signs to the WCL. You eventually arrived at a haunted mansion. It had to be the right place. Right?",
        "You step inside, and suddenly…",
        "Ghouldon Ramsey: YOU! New chef! You are late! You are on in five!",
        "You: What? Ghouldon Ramsey? Why are you at the WCL?",
        "Ghouldon Ramsey: Because I run the show, you donkey!",
        "You: Show? This isn’t the Witch Coven L'association?",
        "Ghouldon Ramsey: No, you fool. This is Wicked Cooking Live. The WCL.",
        "You: Nooooo!",
        "Ghouldon Ramsey: Yessss. Tonight’s challenge is spaghetti and meatballs. Do it well, or you will be on the menu.",
        "You: But I only have potion ingredients.",
        "Ghouldon Ramsey: Then make do. Cut the worms into noodles, smash the tomatolings into sauce, use the eyeballs for meatballs, and grab a bowl from the pantry. Watch out for mimics. Got it?",
        "You: No?",
        "Ghouldon Ramsey: Too bad. Start cooking!"
    };

    private int currentLine = 0;

    void Start()
    {
        if (dialogueText == null)
        {
            Debug.LogWarning("IntroScript: Missing TMP_Text reference.");
            return;
        }

        // make sure Ghouldon starts hidden
        if (ghouldon1 != null) ghouldon1.SetActive(false);
        if (ghouldon2 != null) ghouldon2.SetActive(false);

        dialogueText.text = dialogueLines[currentLine];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceLine();
        }
    }

    private void AdvanceLine()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndIntro();
            return;
        }

        dialogueText.text = dialogueLines[currentLine];

        // When Ghouldon first appears (line 5), activate both objects
        if (currentLine == 5 && !ghouldonActivated)
        {
            ActivateGhouldon();
        }
    }

    private void ActivateGhouldon()
    {
        ghouldonActivated = true;

        if (ghouldon1 != null)
            ghouldon1.SetActive(true);

        if (ghouldon2 != null)
            ghouldon2.SetActive(true);

        Debug.Log("Ghouldon has entered the scene!");
    }

    private void EndIntro()
    {
        Debug.Log("Intro finished! Moving to next scene or enabling gameplay.");
        SceneManager.LoadScene("KitchenScene");
    }
}
