using System.Collections.Generic;
using UnityEngine;
using ReadyPlayerMe.Samples.AvatarCreatorElements;
using ReadyPlayerMe.AvatarCreator;

public class HairStyleClassifier : MonoBehaviour
{
    [SerializeField] private SimpleAvatarCreator creator;
    [SerializeField] private AssetSelectionElement hairStyleSelectionElement;

    private int currentIndex = 0;
    private int maxIndex = 0;

    private readonly List<int> shortIndices  = new List<int>();
    private readonly List<int> mediumIndices = new List<int>();
    private readonly List<int> longIndices   = new List<int>();
    private readonly List<int> baldIndices   = new List<int>();

    // 🔹 Wait until hairstyles are actually loaded
    private System.Collections.IEnumerator Start()
    {
        if (hairStyleSelectionElement == null)
        {
            Debug.LogError("HairStyleClassifier: hairStyleSelectionElement not assigned.");
            yield break;
        }

        // Wait until the Avatar Creator has loaded hair assets
        var maxFrames = 600; // ~10 seconds at 60 FPS
        while (hairStyleSelectionElement.GetAssetCount() == 0 && maxFrames-- > 0)
        {
            yield return null;
        }

        maxIndex = hairStyleSelectionElement.GetAssetCount();
        if (maxIndex <= 0)
        {
            Debug.LogError("HairStyleClassifier: no assets in hairStyleSelectionElement after waiting.");
            yield break;
        }

        currentIndex = 0;
        ShowCurrent();
    }

    private void ShowCurrent()
    {
        hairStyleSelectionElement.SelectAssetByIndex(currentIndex);
        Debug.Log($"[HAIR-CLASSIFIER] Showing index {currentIndex}/{maxIndex - 1}");
    }

    private void Update()
    {
        // 👉 Make sure the Game view has focus when you press keys

        // navigate hairstyles
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % maxIndex;
            ShowCurrent();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + maxIndex) % maxIndex;
            ShowCurrent();
        }

        // label current hairstyle
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shortIndices.Add(currentIndex);
            Debug.Log($"[HAIR-CLASSIFIER] Marked index {currentIndex} as SHORT");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mediumIndices.Add(currentIndex);
            Debug.Log($"[HAIR-CLASSIFIER] Marked index {currentIndex} as MEDIUM");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            longIndices.Add(currentIndex);
            Debug.Log($"[HAIR-CLASSIFIER] Marked index {currentIndex} as LONG");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            baldIndices.Add(currentIndex);
            Debug.Log($"[HAIR-CLASSIFIER] Marked index {currentIndex} as BALD");
        }

        // print summary
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintSummary();
        }
    }

    private void PrintSummary()
    {
        Debug.Log($"[HAIR-CLASSIFIER] SHORT:  {string.Join(", ", shortIndices)}");
        Debug.Log($"[HAIR-CLASSIFIER] MEDIUM: {string.Join(", ", mediumIndices)}");
        Debug.Log($"[HAIR-CLASSIFIER] LONG:   {string.Join(", ", longIndices)}");
        Debug.Log($"[HAIR-CLASSIFIER] BALD:   {string.Join(", ", baldIndices)}");
    }
}
