using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Events;
using TaskExtensions = ReadyPlayerMe.AvatarCreator.TaskExtensions;

#pragma warning disable CS4014
#pragma warning disable CS1998

namespace ReadyPlayerMe.Samples.AvatarCreatorElements
{

    /// <summary>
    ///     A class responsible for creating and customizing avatars using asset and color selections.
    /// </summary>
    [RequireComponent(typeof(SessionHandler))]
    public class SimpleAvatarCreator : MonoBehaviour
    {
        public UnityEvent<AvatarProperties> OnAvatarLoaded;
        public UnityEvent OnAvatarSelected;

        [SerializeField] private List<AssetSelectionElement> assetSelectionElements;
        [SerializeField] private List<ColorSelectionElement> colorSelectionElements;
        [SerializeField] private BodyShapeSelectionElement bodyShapeSelectionElement;
        [SerializeField] private GameObject loading;

        [SerializeField] private GameObject createRPMAccount;
        [SerializeField] private AssetSelectionElement hairStyleSelectionElement;


        private OutfitGender gender = OutfitGender.Masculine;


        [SerializeField] private AvatarHandler avatarHandler;

        public async void OnLogout()
        {
            avatarHandler.CreateNewAvatarFromTemplate();
        }

        public async void OnLogin()
        {
            avatarHandler.CreateNewAvatarFromTemplate();
        }


        public async void SelectAvatar(string avatarId)
        {
            await TaskExtensions.HandleCancellation(avatarHandler.LoadAvatar(avatarId), () =>
            {
                OnAvatarSelected?.Invoke();
            });
        }

        public async void LoadAvatar(string avatarId)
        {
            await TaskExtensions.HandleCancellation(avatarHandler.LoadAvatar(avatarId));
        }

        public async void OnAvatarDeleted(string avatarId)
        {
            if (AuthManager.UserSession.LastModifiedAvatarId == avatarId)
            {
                AuthManager.StoreLastModifiedAvatar(null);
            }

            if (avatarHandler.ActiveAvatarProperties.Id != avatarId)
            {
                return;
            }

            await TaskExtensions.HandleCancellation(avatarHandler.CreateNewAvatarFromTemplate());
        }


        public void SignupAndSaveAvatar()
        {
            if (!AuthManager.IsSignedIn)
            {
                createRPMAccount.SetActive(true);
                return;
            }
            SaveAvatar();
        }

        public async void SaveAvatar()
        {
            loading.SetActive(true);
            await TaskExtensions.HandleCancellation(avatarHandler.SaveActiveAvatar(), () =>
            {
                OnAvatarSelected?.Invoke();
                loading.SetActive(false);
            });
        }

        public async void LoadAvatarFromTemplate(IAssetData template)
        {
            await TaskExtensions.HandleCancellation(avatarHandler.LoadAvatarFromTemplate(template.Id));
        }


        /// <summary>
        ///     LoadUser is used to initialize the avatar creator and loads initial avatar assets.
        /// </summary>
        public async void LoadUserAssets(UserSession session)
        {
            UpdateButtons();
        }

        private void OnEnable()
        {
            avatarHandler.OnAvatarLoading.AddListener(OnAvatarLoading);
            avatarHandler.OnAvatarLoaded.AddListener(OnAvatarLoadingFinished);
            bodyShapeSelectionElement.OnAssetSelected.AddListener(OnAssetSelection);

            // Subscribes to asset selection events when this component is enabled.
            foreach (var element in assetSelectionElements)
            {
                element.OnAssetSelected.AddListener(OnAssetSelection);
            }

            foreach (var element in colorSelectionElements)
            {
                element.OnAssetSelected.AddListener(OnAssetSelection);
            }
        }

        private void OnDisable()
        {
            avatarHandler.OnAvatarLoading.RemoveListener(OnAvatarLoading);
            avatarHandler.OnAvatarLoaded.RemoveListener(OnAvatarLoadingFinished);

            bodyShapeSelectionElement.OnAssetSelected.RemoveListener(OnAssetSelection);
            // Unsubscribes from asset selection events when this component is disabled.
            foreach (var element in assetSelectionElements)
            {
                element.OnAssetSelected.RemoveListener(OnAssetSelection);
            }

            foreach (var element in colorSelectionElements)
            {
                element.OnAssetSelected.RemoveListener(OnAssetSelection);
            }
        }

        private void OnAvatarLoadingFinished(AvatarProperties properties)
        {
            GetColors(properties);
            LoadAssets(properties.Gender);

            // 🔍 debug: log all hair styles once after loading
            if (hairStyleSelectionElement != null)
            {
                hairStyleSelectionElement.LogAssets("HAIR");
            }

            OnAvatarLoaded?.Invoke(properties);
            loading.SetActive(false);
        }

        private void OnAvatarLoading()
        {
            loading.SetActive(true);
        }

        /// <summary>
        ///     Handles the selection of an asset and updates the avatar accordingly.
        /// </summary>
        /// <param name="assetData">The selected asset data.</param>
        private async void OnAssetSelection(IAssetData assetData)
        {
            await TaskExtensions.HandleCancellation(avatarHandler.SelectAsset(assetData));
        }

        /// <summary>
        ///     Loads and initializes asset selection elements for avatar customization.
        /// </summary>
        private void LoadAssets(OutfitGender newGender)
        {
            if (gender == newGender)
            {
                return;
            }
            gender = newGender;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bodyShapeSelectionElement.LoadAndCreateButtons();
            foreach (var element in assetSelectionElements)
            {
                TaskExtensions.HandleCancellation(element.LoadAndCreateButtons(gender));
            }
        }

        /// <summary>
        ///     Loads and initializes color selection elements for choosing avatar colors.
        /// </summary>
        /// <param name="avatarProperties">The properties of the avatar.</param>
        private void GetColors(AvatarProperties avatarProperties)
        {
            foreach (var element in colorSelectionElements)
            {
                element.LoadAndCreateButtons(avatarProperties);
            }
        }

        // Changes from ChatGPT 
        public void SetBodyShapeByIndex(int index)
        {
            if (bodyShapeSelectionElement == null)
            {
                Debug.LogWarning("SimpleAvatarCreator: bodyShapeSelectionElement is not assigned.");
                return;
            }

            bodyShapeSelectionElement.SelectBodyShapeByIndex(index);
        }

        public void SetHairStyleByIndex(int index)
        {
            if (hairStyleSelectionElement == null)
            {
                Debug.LogWarning("SimpleAvatarCreator: hairStyleSelectionElement is not assigned.");
                return;
            }

            hairStyleSelectionElement.SelectAssetByIndex(index);
        }

        public void SetBodyShapeByText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            text = text.ToLowerInvariant();

            // Keywords for each body type – you can extend these lists as you like
            string[] slimWords = { "slim", "thin", "skinny", "narrow", "lean" };
            string[] averageWords = { "average", "normal", "standard", "regular" };
            string[] muscularWords = { "muscular", "strong", "athletic", "ripped", "fit" };
            string[] plusWords = { "plus", "big", "curvy", "thicker", "larger" };

            int index = -1;

            bool ContainsAny(string[] words)
            {
                foreach (var w in words)
                {
                    if (text.Contains(w))
                        return true;
                }
                return false;
            }

            if (ContainsAny(slimWords))
                index = 0;
            else if (ContainsAny(averageWords))
                index = 1;
            else if (ContainsAny(muscularWords))
                index = 2;
            else if (ContainsAny(plusWords))
                index = 3;
            else
            {
                // Fallback: support "body 1", "type 2", etc.
                if (text.Contains("1"))
                    index = 0;
                else if (text.Contains("2"))
                    index = 1;
                else if (text.Contains("3"))
                    index = 2;
                else if (text.Contains("4"))
                    index = 3;
            }

            if (index == -1)
            {
                Debug.LogWarning($"SetBodyShapeByText: could not understand body type from '{text}'");
                return;
            }

            Debug.Log($"SetBodyShapeByText: '{text}' ➜ index {index}");
            SetBodyShapeByIndex(index);
        }

        public void SetHairStyleByText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            text = text.ToLowerInvariant();

            // You can tweak these lists after you see which index is which style
            string[] shortWords = { "short", "shorter", "buzz", "buzzcut" };
            string[] mediumWords = { "medium", "mid", "in between" };
            string[] longWords = { "long", "longer" };
            string[] baldWords = { "bald", "no hair", "shaved" };

            int index = -1;

            bool ContainsAny(string[] words)
            {
                foreach (var w in words)
                {
                    if (text.Contains(w))
                        return true;
                }
                return false;
            }

            if (ContainsAny(shortWords))
                index = 0;
            else if (ContainsAny(mediumWords))
                index = 1;
            else if (ContainsAny(longWords))
                index = 2;
            else if (ContainsAny(baldWords))
                index = 3;
            else
            {
                // Fallback: hairstyle 1/2/3/4
                if (text.Contains("1")) index = 0;
                else if (text.Contains("2")) index = 1;
                else if (text.Contains("3")) index = 2;
                else if (text.Contains("4")) index = 3;
            }

            if (index == -1)
            {
                Debug.LogWarning($"SetHairStyleByText: could not understand hair style from '{text}'");
                return;
            }

            Debug.Log($"SetHairStyleByText: '{text}' ➜ hair index {index}");
            SetHairStyleByIndex(index);
        }

    }
}

