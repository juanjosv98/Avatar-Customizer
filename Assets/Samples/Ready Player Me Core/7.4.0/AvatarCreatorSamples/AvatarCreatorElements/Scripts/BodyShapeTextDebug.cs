using UnityEngine;
using ReadyPlayerMe.Samples.AvatarCreatorElements;

public class BodyShapeTextDebug : MonoBehaviour
{
    [SerializeField] private SimpleAvatarCreator creator;

    [SerializeField] [TextArea]
    private string commandText = "slim";

    void Update()
    {
        // Press Enter/Return to apply the current text
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (creator != null)
            {
                creator.SetBodyShapeByText(commandText);
            }
        }
    }
}
