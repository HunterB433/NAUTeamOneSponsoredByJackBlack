using UnityEngine;
using TMPro;

public class GhouldonTextManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text dialogueText;

    [Header("Global Manager Reference")]
    public GlobalManager globalManager;

    // Internal tracking: what stage we’re currently showing
    private int currentStage = 0;

    void Start()
    {
        if (globalManager == null)
            globalManager = FindFirstObjectByType<GlobalManager>();

        UpdateReaction();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceStage(); // move to next stage on each click
            UpdateReaction();
        }
    }

    private void AdvanceStage()
    {
        // Move to next uncompleted stage
        currentStage++;

        // Cap at 5 stages total
        if (currentStage > 5)
            currentStage = 5;
    }

    public void UpdateReaction()
    {
        if (dialogueText == null || globalManager == null)
        {
            Debug.LogWarning("GhouldonTextManager: Missing references.");
            return;
        }

        string message = GetReactionMessage();
        dialogueText.text = "Ghouldon Ramsey: " + message;
    }

    private string GetReactionMessage()
    {
        // Move through stages in fixed order
        switch (currentStage)
        {
            case 0:
                globalManager.completedMeatball = true;
                return GetMeatballReaction(globalManager.numEyeBalls);

            case 1:
                globalManager.completedBowl = true;
                return GetBowlReaction(globalManager.numFails);

            case 2:
                globalManager.completedWorms = true;
                return GetWormReaction(globalManager.numWorms, globalManager.numWormsCut);

            case 3:
                globalManager.completedTomatos = true;
                return GetTomatoReaction(globalManager.numTomatos, globalManager.numTomatosHit);

            case 4:
                globalManager.completedMix = true;
                return GetMixReaction(globalManager.mixScore);

            default:
                return "You’ve done it all! Finally, something that doesn’t taste like despair.";
        }
    }

    // ------------------------------
    // Reaction methods below (unchanged)
    // ------------------------------
    private string GetMeatballReaction(int numEyeBalls)
    {
        if (numEyeBalls <= 3) return "(On the Eye-MeatBalls) What is this monstrosity? It's raw, there arent enough eye-meatballs! you blind mole!";
        if (numEyeBalls <= 5) return "(On the Eye-MeatBalls) Barely passable. Did you even look at the recipe? Have you even cooked with human flesh before?";
        if (numEyeBalls <= 7) return "(On the Eye-MeatBalls) Not bad, an alright amount of eye-meatballs. but it's still a bit of a horror show.";
        return "(On the Eye-MeatBalls) Finally! A dish that doesn’t make me lose faith in humanity An excelent ammount of eye-meatballs! Delicous!";
    }

    private string GetBowlReaction(int numFails)
    {
        if (numFails == 0) return "(On the Bowl) You actually managed not to ruin it! You managed to pick out a bowl Miraculous!";
        if (numFails == 1) return " (On the Bowl) This bowl is blood stained, did you prick you finger on a mimic bowl? I've seen barbarians do better!";
        return "(On the Bowl) Disaster! This bowl is obviously a mimic! I can't eat from this, You’ve turned it into a crime scene!";
    }

    private string GetWormReaction(int numWorms, int numWormsCut)
    {
        float cutPercent = (numWorms == 0) ? 0 : (float)numWormsCut / numWorms;
        if (cutPercent < 0.3f) return "(On the Worm-Noodles) Pathetic! The worms are laughing at you! You are supposed to cut them into noodles you donkey!";
        if (cutPercent < 0.5f) return "(On the Worm-Noodles) Come on, slice faster than that! It’s like watching paint dry! Cut more worm noodles!";
        if (cutPercent < 0.7f) return "(On the Worm-Noodles) Not bad, but those worms still have attitude. Maybe be more accurate next time";
        return "(On the Worm-Noodles) Excellent! You’ve shown those worms who’s the final boss!";
    }

    private string GetTomatoReaction(int numTomatos, int numTomatosHit)
    {
        float hitPercent = (numTomatos == 0) ? 0 : (float)numTomatosHit / numTomatos;
        if (numTomatos < 50 || hitPercent < 0.3f)
            return "(On the Tomatolings-Sauce) My grandmother smashes tomatolings better than that! Absolute rubbish!";
        if (numTomatos < 70 || hitPercent < 0.5f)
            return "(On the Tomatolings-Sauce) Mediocre aim, mediocre sauce! Pathetic!";
        return "(On the Tomatolings-Sauce) Finally! You’ve hit something! Maybe there’s hope for you yet.";
    }

    private string GetMixReaction(float mixScore)
    {
        if (mixScore < 0.3f) return "(On mixing everthing together) That’s not a mix, that’s a chemical spill!";
        if (mixScore < 0.5f) return "(On mixing everthing together) You call that stirred? It’s barely alive!";
        if (mixScore < 0.7f) return "(On mixing everthing together) It’s decent... shockingly decent.";
        return "(On mixing everthing together) Beautiful mix! Almost as beautiful as my friend Frankenstein You’ve finally used your hands for something good.";
    }
}
