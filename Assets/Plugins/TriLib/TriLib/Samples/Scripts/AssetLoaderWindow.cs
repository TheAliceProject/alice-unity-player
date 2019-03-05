using UnityEngine;

namespace TriLib
{
    namespace Samples
    {
        /// <summary>
        /// Represents the asset loader UI component.
        /// </summary>
        public class AssetLoaderWindow : MonoBehaviour
        {
            /// <summary>
            /// Class singleton.
            /// </summary>
            public static AssetLoaderWindow Instance { get; private set; }
            /// <summary>
            /// "Load asset button" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Button _loadAssetButton = null;
            /// <summary>
            /// "Spinning text" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Text _spinningText = null;            
			/// <summary>
			/// "Cutout toggle" reference.
			/// </summary>
			[SerializeField]
			private UnityEngine.UI.Toggle _cutoutToggle = null;
			/// <summary>
			/// "Spin X toggle" reference.
			/// </summary>
			[SerializeField]
            private UnityEngine.UI.Toggle _spinXToggle = null;
            /// <summary>
            /// "Spin Y toggle" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Toggle _spinYToggle = null;
            /// <summary>
            /// "Reset rotation button" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Button _resetRotationButton = null;
            /// <summary>
            /// "Stop animation button" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Button _stopAnimationButton = null;
            /// <summary>
            /// "Animations text" reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.Text _animationsText = null;
            /// <summary>
            /// "Animations scroll rect "reference.
            /// </summary>
            [SerializeField]
            private UnityEngine.UI.ScrollRect _animationsScrollRect = null;
            /// <summary>
            /// "Animations scroll rect container" reference.
            /// </summary>
            [SerializeField]
            private Transform _containerTransform = null;
            /// <summary>
            /// <see cref="AnimationText"/> prefab reference.
            /// </summary>
            [SerializeField]
            private AnimationText _animationTextPrefab = null;
            /// <summary>
            /// "Background (gradient) canvas" reference.
            /// </summary>
            [SerializeField]
            private Canvas _backgroundCanvas = null;
            /// <summary>
            /// Loaded Game Object reference.
            /// </summary>
            private GameObject _rootGameObject;
            /// <summary>
            /// Handles events from <see cref="AnimationText"/>.
            /// </summary>
            /// <param name="animationName">Choosen animation name.</param>
            public void HandleEvent(string animationName)
            {
                _rootGameObject.GetComponent<Animation>().Play(animationName);
                _stopAnimationButton.interactable = true;
            }
            /// <summary>
            /// Destroys all objects in the container.
            /// </summary>
            public void DestroyItems()
            {
                foreach (Transform innerTransform in _containerTransform)
                {
                    Destroy(innerTransform.gameObject);
                }
            }
            /// <summary>
            /// Initializes variables.
            /// </summary>
            protected void Awake()
            {
                _loadAssetButton.onClick.AddListener(LoadAssetButtonClick);
                _stopAnimationButton.onClick.AddListener(StopAnimationButtonClick);
                _resetRotationButton.onClick.AddListener(ResetRotationButtonClick);
                HideControls();
                Instance = this;
            }
            /// <summary>
            /// Spins the loaded Game Object if options are enabled.
            /// </summary>
            protected void Update()
            {
                if (_rootGameObject != null)
                {
                    _rootGameObject.transform.Rotate(_spinXToggle.isOn ? 20f * Time.deltaTime : 0f,
                        _spinYToggle.isOn ? -20f * Time.deltaTime : 0f, 0f, Space.World);
                }
            }
            /// <summary>
            /// Hides user controls.
            /// </summary>
            private void HideControls()
            {
                _spinningText.gameObject.SetActive(false);
                _spinXToggle.gameObject.SetActive(false);
                _spinYToggle.gameObject.SetActive(false);
                _resetRotationButton.gameObject.SetActive(false);
                _stopAnimationButton.gameObject.SetActive(false);
                _animationsText.gameObject.SetActive(false);
                _animationsScrollRect.gameObject.SetActive(false);
            }
            /// <summary>
            /// Handles "Load asset button" click event and tries to load an asset at chosen path.
            /// </summary>
            private void LoadAssetButtonClick()
            {
                var fileOpenDialog = FileOpenDialog.Instance;
                fileOpenDialog.Title = "Please select a File";
                fileOpenDialog.Filter = AssetLoader.GetSupportedFileExtensions();
#if (UNITY_WINRT && !UNITY_EDITOR_WIN)
                fileOpenDialog.ShowFileOpenDialog(delegate(byte[] fileBytes, string filename)
#else
                fileOpenDialog.ShowFileOpenDialog(delegate (string filename)
#endif
                {
                    HideControls();
                    if (_rootGameObject != null)
                    {
                        Destroy(_rootGameObject);
                        _rootGameObject = null;
                    }
                    var assetLoaderOptions = AssetLoaderOptions.CreateInstance();
                    assetLoaderOptions.DontLoadCameras = false;
                    assetLoaderOptions.DontLoadLights = false;
					assetLoaderOptions.UseCutoutMaterials = _cutoutToggle.isOn;
                    using (var assimpLoader = new AssetLoader())
                    {
                        try
                        {
#if (UNITY_WINRT && !UNITY_EDITOR_WIN)
                            _rootGameObject = assimpLoader.LoadFromMemory(fileBytes, filename, assetLoaderOptions);
#else
                            _rootGameObject = assimpLoader.LoadFromFile(filename, assetLoaderOptions);
#endif
                        }
                        catch (System.Exception exception)
                        {
                            ErrorDialog.Instance.ShowDialog(exception.ToString());
                        }
                    }
                    if (_rootGameObject != null)
                    {
                        var mainCamera = Camera.main;
                        mainCamera.FitToBounds(_rootGameObject.transform, 3f);
                        _backgroundCanvas.planeDistance = mainCamera.farClipPlane * 0.99f;
                        _spinningText.gameObject.SetActive(true);
                        _spinXToggle.isOn = false;
                        _spinXToggle.gameObject.SetActive(true);
                        _spinYToggle.isOn = false;
                        _spinYToggle.gameObject.SetActive(true);
                        _resetRotationButton.gameObject.SetActive(true);
                        DestroyItems();
                        var rootAnimation = _rootGameObject.GetComponent<Animation>();
                        if (rootAnimation != null)
                        {
                            _animationsText.gameObject.SetActive(true);
                            _animationsScrollRect.gameObject.SetActive(true);
                            _stopAnimationButton.gameObject.SetActive(true);
                            _stopAnimationButton.interactable = false;
                            foreach (AnimationState animationState in rootAnimation)
                            {
                                CreateItem(animationState.name);
                            }
                        }
                    }
                }
            );
            }
            /// <summary>
            /// Creates a <see cref="AnimationText"/> item in the container.
            /// </summary>
            /// <param name="text">Text of the <see cref="AnimationText"/> item.</param>
            private void CreateItem(string text)
            {
                var instantiated = Instantiate(_animationTextPrefab, _containerTransform);
                instantiated.Text = text;
            }
            /// <summary>
            /// Handles the "Reset Rotation button" click event and stops the loaded Game Object spinning. 
            /// </summary>
            private void ResetRotationButtonClick()
            {
                _spinXToggle.isOn = false;
                _spinYToggle.isOn = false;
                _rootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            /// <summary>
            /// Handles the "Stop Animation button" click event and stops the loaded Game Object animation.
            /// </summary>
            private void StopAnimationButtonClick()
            {
                _rootGameObject.GetComponent<Animation>().Stop();
                _stopAnimationButton.interactable = false;
            }
        }
    }
}