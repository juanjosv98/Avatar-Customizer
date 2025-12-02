using UnityEngine;
using ReadyPlayerMe.Samples.AvatarCreatorElements;

public class BodyShapeDebugHotkeys : MonoBehaviour
{
    [SerializeField] private SimpleAvatarCreator creator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            creator.SetBodyShapeByIndex(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            creator.SetBodyShapeByIndex(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            creator.SetBodyShapeByIndex(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            creator.SetBodyShapeByIndex(3);
    }
}
