using UnityEngine;
using TriLib;
using System.IO;
using System;

public class LoadModel : MonoBehaviour {

    [SerializeField] private readonly bool useTriLib = true;
    [Tooltip("Can only load models from the Asset/Models folder per this script.")]
    [SerializeField] private string fileName = "alice.dae";
	[SerializeField] private Texture texOverride;

    private void Start()
    {
        //Debug.Log(Path.GetFullPath("./Models"));
        if (useTriLib)
        {
            using (var assetLoader = new AssetLoader())
            { //Initializes our Asset Loader.
                var assetLoaderOptions = ScriptableObject.CreateInstance<AssetLoaderOptions>(); //Creates an Asset Loader Options object.
                assetLoaderOptions.AutoPlayAnimations = true; //Indicates that our model will automatically play its first animation, if any.
				if (texOverride)
				{
					assetLoader.OnMaterialCreated += MaterialCreated;
				} else
				{
					assetLoaderOptions.TexturesPathOverride = Path.GetFullPath("./Assets/Models/Textures");
				}

				var filename = Path.Combine(Path.GetFullPath("./Assets/Models"), fileName); //Combines our current directory with our model filename "turtle1.b3d" and generates the full model path.
				assetLoader.LoadFromFile(filename, assetLoaderOptions); //Loads our model.
			}
        }
    }

	private void MaterialCreated(uint materialIndex, bool isOverriden, Material material)
	{
		material.SetTexture("_MainTex", texOverride);
	}
}