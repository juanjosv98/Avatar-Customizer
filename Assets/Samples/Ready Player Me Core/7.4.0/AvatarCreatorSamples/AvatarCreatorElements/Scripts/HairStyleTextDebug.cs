using UnityEngine;
using ReadyPlayerMe.Samples.AvatarCreatorElements;

public class HairStyleTextDebug : MonoBehaviour
{
    [SerializeField] private SimpleAvatarCreator creator;
    [SerializeField][TextArea] private string commandText = "short hair";

    void Update()
    {
        // Works when the Game view has focus
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (creator != null)
            {
                creator.SetHairStyleByText(commandText);
            }
        }
    }
}
