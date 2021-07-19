using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using Siccity.GLTFUtility;

public class LoadModel : MonoBehaviour {

    [Tooltip("Can only load models from the Asset/Models folder per this script.")]
    [SerializeField] private string fileName = "ColaBottle.dae";
	[SerializeField] private Texture texOverride = null;

    private List<Mesh> GetMeshes( Transform rootTransform )
    {
        List<Mesh> meshes = new List<Mesh>();

        for (int i=0; i<rootTransform.childCount; i++)
        {
            Transform child = rootTransform.GetChild(i);
            SkinnedMeshRenderer skin = child.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skin != null)
            {
                meshes.Add(skin.sharedMesh);
            }
            else
            {
                meshes.AddRange(GetMeshes(child));
            }
        }
        return meshes;
    }

    private void CheckWeights(Mesh mesh)
    {
        Debug.Log(mesh.name + "START");
        Debug.Log("Bone Weights Check:");
        for (int i = 0; i < mesh.boneWeights.Length; i++)
        {
            BoneWeight weight = mesh.boneWeights[i];

            double weightTotal = weight.weight0 + weight.weight1 + weight.weight2 + weight.weight3;
            if (Math.Abs(1 - weightTotal) > .001)
            {
                Debug.Log("  "+i+" BAD WEIGHTS: "+weightTotal);
            }
        }
        Debug.Log(mesh.name + " DONE");
    }

    private void NormalizeWeightsInMesh(Mesh mesh)
    {
        BoneWeight[] newWeights = mesh.boneWeights;
        for (int i = 0; i < mesh.boneWeights.Length; i++)
        {
            BoneWeight weight = newWeights[i];
            double weightTotal = weight.weight0 + weight.weight1 + weight.weight2 + weight.weight3;
            weight.weight0 = (float)(weight.weight0 / weightTotal);
            weight.weight1 = (float)(weight.weight1 / weightTotal);
            weight.weight2 = (float)(weight.weight2 / weightTotal);
            weight.weight3 = (float)(weight.weight3 / weightTotal);
            newWeights[i] = weight;
        }
        mesh.boneWeights = newWeights;
    }

    private void NormalizeWeightsInModel(GameObject model)
    {
        List<Mesh> meshes = GetMeshes(model.transform);
        foreach(Mesh mesh in meshes)
        {
            NormalizeWeightsInMesh(mesh);
        }
    } 

    private void Start()
    {
        var assetLoaderOptions = new ImportSettings(); //Creates an Asset Loader Options object.
	    var filename = Path.Combine(Path.GetFullPath("./Assets/Models"), fileName); //Combines our current directory with our model filename "turtle1.b3d" and generates the full model path.
        Importer.ImportGLTFAsync(filename, assetLoaderOptions, OnFinishLoading);  //Loads our model.
    }

    private void OnFinishLoading(GameObject loadedModel) 
    {
        NormalizeWeightsInModel(loadedModel);
    }

    private void MaterialCreated(uint materialIndex, bool isOverriden, Material material)
	{
		material.SetTexture("_MainTex", texOverride);
	}
}