using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GhouldonTextManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text dialogueText;

    [Header("Global Manager Reference")]
    public GlobalManager globalManager;

    [Header("Scoring")]
    public int totalScore = 0;

    private int currentStage = 0;
    private bool finalReactionShown = false;
    private bool readyToChangeScene = false;

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
            // If ready to change scene after final text
            if (readyToChangeScene)
            {
                LoadEndScene();
                return;
            }

            AdvanceStage();
            UpdateReaction();
        }
    }

    private void AdvanceStage()
    {
        if (finalReactionShown)
            return;

        currentStage++;

        if (currentStage > 4)
        {
            currentStage = 5; // 0–4 are dishes, 5 is final result
            ShowFinalReaction();
        }
    }

    private void ShowFinalReaction()
    {
        finalReactionShown = true;

        string finalMessage;
        if (totalScore >= 10)
        {
            finalMessage = "You’ve done it all! Finally, a meal that doesn’t make me want to haunt the kitchen. Well done, you chaotic culinary genius!";
        }
        else
        {
            finalMessage = "You’ve done it all... wrong! I’ve seen zombies with better taste. Well, lets see now how YOU taste!";
        }

        dialogueText.text = "Ghouldon Ramsey: " + finalMessage;
        Debug.Log("Final Score: " + totalScore + " | " + finalMessage);

        // After showing final reaction, next click will trigger scene switch
        readyToChangeScene = true;
    }

    private void LoadEndScene()
    {
        if (totalScore >= 10)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            SceneManager.LoadScene("Lose");
        }
    }

    public void UpdateReaction()
    {
        if (dialogueText == null || globalManager == null)
        {
            Debug.LogWarning("GhouldonTextManager: Missing references.");
            return;
        }

        // Skip normal reactions if final reaction already shown
        if (finalReactionShown)
            return;

        string message = GetReactionMessage();
        dialogueText.text = "Ghouldon Ramsey: " + message;
    }

    private string GetReactionMessage()
    {
        switch (currentStage)
        {
            case 0:
                globalManager.completedMeatball = true;
                AddScore(GetMeatballScore(globalManager.numEyeBalls));
                return GetMeatballReaction(globalManager.numEyeBalls);

            case 1:
                globalManager.completedBowl = true;
                AddScore(GetBowlScore(globalManager.numFails));
                return GetBowlReaction(globalManager.numFails);

            case 2:
                globalManager.completedWorms = true;
                AddScore(GetWormScore(globalManager.numWorms, globalManager.numWormsCut));
                return GetWormReaction(globalManager.numWorms, globalManager.numWormsCut);

            case 3:
                globalManager.completedTomatos = true;
                AddScore(GetTomatoScore(globalManager.numTomatos, globalManager.numTomatosHit));
                return GetTomatoReaction(globalManager.numTomatos, globalManager.numTomatosHit);

            case 4:
                globalManager.completedMix = true;
                AddScore(GetMixScore(globalManager.mixScore));
                return GetMixReaction(globalManager.mixScore);

            default:
                return "";
        }
    }

    private void AddScore(int points)
    {
        totalScore += points;
        Debug.Log("Ghouldon Ramsey Score: +" + points + " (Total: " + totalScore + ")");
    }

    // ------------------------------
    // Reaction and scoring methods
    // ------------------------------

    private string GetMeatballReaction(int numEyeBalls)
    {
        if (numEyeBalls <= 3) return "(On the Eye-MeatBalls) What is this monstrosity? It's raw, there arent enough eye-meatballs! you blind mole!";
        if (numEyeBalls <= 5) return "(On the Eye-MeatBalls) Barely passable. Did you even look at the recipe? Have you even cooked with human flesh before?";
        if (numEyeBalls <= 7) return "(On the Eye-MeatBalls) Not bad, an alright amount of eye-meatballs. but it's still a bit of a horror show.";
        return "(On the Eye-MeatBalls) Finally! A dish that doesn’t make me lose faith in humanity An excellent amount of eye-meatballs! Delicous!";
    }

    private int GetMeatballScore(int numEyeBalls)
    {
        if (numEyeBalls <= 3) return 0;
        if (numEyeBalls <= 5) return 1;
        if (numEyeBalls <= 7) return 2;
        return 3;
    }

    private string GetBowlReaction(int numFails)
    {
        if (numFails == 0) return "(On the Bowl) You actually managed not to ruin it! You managed to pick out a bowl Miraculous!";
        if (numFails == 1) return "(On the Bowl) This bowl is blood stained, did you prick your finger on a mimic bowl? I've seen barbarians do better!";
        return "(On the Bowl) Disaster! This bowl is obviously a mimic! I can't eat from this, You’ve turned it into a crime scene!";
    }

    private int GetBowlScore(int numFails)
    {
        if (numFails == 0) return 3;
        if (numFails == 1) return 1;
        return 0;
    }

    private string GetWormReaction(int numWorms, int numWormsCut)
    {
        float cutPercent = (numWorms == 0) ? 0 : (float)numWormsCut / numWorms;
        if (cutPercent < 0.1f) return "(On the Worm-Noodles) Pathetic! The worms are laughing at you! You are supposed to cut them into noodles you donkey!";
        if (cutPercent < 0.2f) return "(On the Worm-Noodles) Come on, slice faster than that! It’s like watching paint dry! Cut more worm noodles!";
        if (cutPercent < 0.3f) return "(On the Worm-Noodles) Not bad, but those worms still have attitude. Be more accurate next time!";
        return "(On the Worm-Noodles) Excellent! You’ve shown those worms who’s the final boss!";
    }

    private int GetWormScore(int numWorms, int numWormsCut)
    {
        float cutPercent = (numWorms == 0) ? 0 : (float)numWormsCut / numWorms;
        if (cutPercent < 0.1f) return 0;
        if (cutPercent < 0.2f) return 1;
        if (cutPercent < 0.3f) return 2;
        return 3;
    }

    private string GetTomatoReaction(int numTomatos, int numTomatosHit)
    {
        if (numTomatos < 15)
            return "(On the Tomatolings-Sauce) My grandmother smashes tomatolings better than that! Absolute rubbish!";
        if (numTomatos < 30)
            return "(On the Tomatolings-Sauce) Mediocre aim, mediocre sauce! Pathetic!";
        return "(On the Tomatolings-Sauce) Finally! You’ve hit something! Maybe there’s hope for you yet.";
    }

    private int GetTomatoScore(int numTomatos, int numTomatosHit)
    {
        if (numTomatos < 15) return 0;
        if (numTomatos < 30) return 1;
        return 3;
    }

    private string GetMixReaction(float mixScore)
    {
        if (mixScore < 0.3f) return "(On mixing everthing together) That’s not a mix, that’s a chemical spill!";
        if (mixScore < 0.5f) return "(On mixing everthing together) You call that stirred? It’s barely alive!";
        if (mixScore < 0.7f) return "(On mixing everthing together) It’s decent... shockingly decent.";
        return "(On mixing everthing together) Beautiful mix! Almost as beautiful as my friend Frankenstein You’ve finally used your hands for something good.";
    }

    private int GetMixScore(float mixScore)
    {
        if (mixScore < 0.3f) return 0;
        if (mixScore < 0.5f) return 1;
        if (mixScore < 0.7f) return 2;
        return 3;
    }
}
