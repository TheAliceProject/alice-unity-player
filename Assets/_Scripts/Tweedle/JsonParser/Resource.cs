using System.Collections.Generic;
using UnityEngine;

namespace Alice.Json
{
	public class Resource : MonoBehaviour
	{

		private string jsonStr =
			"{\"aliceVersion\": \"3.4.1.0\",\"projectType\": \"Model\",\"name\": \"Chicken Little\",\"id\": \"ChickenLittle\",\"projectVersion\": \"0.3\", \"prerequisites\": [],\"resources\": [{\"contentType\": \"model.collada\",\"entryName\": \"resources/importedChicken\",\"name\": \"importedChicken\",\"originalFileName\": \"somePicture.png\",\"uuid\": \"b54e2757-154a-4124-9d44-587025184679\",\"structureFile\": \"importedChicken.dae\",\"textureFiles\": [\"importedChicken.png\"]},{\"contentType\": \"model.collada\",\"entryName\": \"resources/funkyChicken\",\"name\": \"funkyChicken\",\"originalFileName\": \"somePicture.png\",\"uuid\": \"b3333757-154a-4124-9d44-587025184679\",\"structureFile\": \"importedChicken.dae\",\"textureFiles\": [\"funkyChicken.png\"]}]}";

		private string aliceVersion;
		//private Alice.Tweedle.ProjectIdentifier id;
		//private List<Alice.Tweedle.ProjectIdentifier> prerequisites;
		private List<Alice.Tweedle.TweedleResource> resources;

		private void Start()
		{
			JSONObject json = new JSONObject(jsonStr);
		}

		public void Parse()
		{
			string var = "aliceVersion";
			JsonParser.SetValue(this, var, "1.1.1");
			Debug.Log(this.GetType().GetField(var, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this));
		}
	}
}