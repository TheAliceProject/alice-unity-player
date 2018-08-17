using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class JsonParseTest
	{
		private string root;

		/*
		   string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\":\"Program\",\"format\":\"tweedle\",\"files\":[\"src/Program.twe\"],\"type\":\"Class\"}," +
					"{\"id\":\"TurnDirection\",\"format\":\"tweedle\",\"files\":[\"src/TurnDirection.twe\"],\"type\":\"Enum\"}," +
				    "{\"id\": \"Alien\", \"type\": \"model\", \"format\": \"json\", \"files\": [ \"resources/alienCollada/model.json\" ]}," +
					"{\"id\": \"beast_growl_02_echo.mp3\", \"type\": \"audio\", \"format\": \"mpeg\", \"files\": [\"resources/beast_growl_02_echo.mp3\" ], \"uuid\": \"23d9dfb6-5cb0-4b55-bd05-1ec5bb133381\", \"duration\": 3.313}," +
					"{\"id\": \"somePicture.png\", \"type\": \"image\", \"format\": \"png\", \"files\": [ \"resources/somePicture.png\" ], \"uuid\": \"9d7f2757-154a-4124-9d44-587025184679\", \"height\": 300, \"width\": 500}," +
				"]}";
		 */

		[SetUp]
		public void Setup()
		{
			root = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\_Scripts\\Tweedle\\Parsing\\Editor\\JsonParse Files");
		}

		private TweedleSystem StoredSystem(string str)
		{
			JsonParser json = new JsonParser(new TweedleSystem(), null);
			json.ParseJson(str);
			return json.StoredSystem;
		}

		// Tweedle System
		[Test]
		public void SystemShouldLoadFile()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}}";
			Assert.IsTrue(StoredSystem(manifest).LoadedFiles.Contains(new ProjectIdentifier("970a310f-90a7-4d29-8380-5b2a742e3ee4", "1.0", "World")), "System should have loaded file");
		}

		[Test]
		public void LoadFileShouldHaveMetaData()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}}";
			TweedleSystem system = StoredSystem(manifest);
			Assert.AreEqual("970a310f-90a7-4d29-8380-5b2a742e3ee4", system.LoadedFiles.First().id, "System's id should match.");
			Assert.AreEqual("1.0", system.LoadedFiles.First().version, "System's version should match.");
			Assert.AreEqual("World", system.LoadedFiles.First().type, "System's type should match.");
		}

		[Test]
		public void FileShouldHaveDescription()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[\"tags\"],\"groupTags\":[\"group\"],\"themeTags\":[\"theme\"]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}}";
			ProgramDescription program = StoredSystem(manifest).Programs["970a310f-90a7-4d29-8380-5b2a742e3ee4"];
			Assert.AreEqual("Program", program.description.name, "File's name should match");
			Assert.AreEqual("thumbnail.png", program.description.icon, "File's icon should match");
			Assert.AreEqual(new List<string>() { "tags" }, program.description.tags, "File's tags should match.");
			Assert.AreEqual(new List<string>() { "group" }, program.description.groupTags, "File's group tags should match.");
			Assert.AreEqual(new List<string>() { "theme" }, program.description.themeTags, "File's theme tags should match.");
		}

		[Test]
		public void FileShouldHaveProvenance()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[\"tags\"],\"groupTags\":[\"group\"],\"themeTags\":[\"theme\"]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}}";
			ProgramDescription program = StoredSystem(manifest).Programs["970a310f-90a7-4d29-8380-5b2a742e3ee4"];
			Assert.AreEqual("3.4.0.0-alpha", program.provenance.aliceVersion, "File's alice version should match.");
			Assert.AreEqual("2018", program.provenance.creationYear, "File's creation year should match.");
			Assert.AreEqual("Anonymous", program.provenance.creator, "File's creator should match.");
		}

		[Test]
		public void FileShouldHaveMetaData()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[\"tags\"],\"groupTags\":[\"group\"],\"themeTags\":[\"theme\"]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"formatVersion\": \"0.1\"," +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}}";
			ProgramDescription program = StoredSystem(manifest).Programs["970a310f-90a7-4d29-8380-5b2a742e3ee4"];
			Assert.AreEqual("0.1", program.metadata.formatVersion, "File's id should match.");
			Assert.AreEqual("970a310f-90a7-4d29-8380-5b2a742e3ee4", program.metadata.identifier.id, "File's id should match.");
			Assert.AreEqual("1.0", program.metadata.identifier.version, "File's version should match.");
			Assert.AreEqual("World", program.metadata.identifier.type, "File's type should match.");
		}

		[Test]
		[Ignore("No zip to read from")]
		public void SystemShouldLoadResource()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\":\"Program\",\"format\":\"tweedle\",\"files\":[\"src/Program.twe\"],\"type\":\"Class\"}" +
				"]}";
			Assert.IsTrue(StoredSystem(manifest).Resources.ContainsKey(new ResourceIdentifier("Program", ContentType.Class, "tweedle")), "System should have loaded resource.");
		}

		[Test]
		[Ignore("No zip to read from")]
		public void SystemShouldHaveResource()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\": \"Alien\", \"type\": \"model\", \"format\": \"json\", \"files\": [ \"resources/alienCollada/model.json\" ]}" +
				"]}";
			Assert.NotNull(StoredSystem(manifest).Resources[new ResourceIdentifier("Alien", ContentType.Model, "json")], "Resources should store a model.");
		}

		[Test]
		public void SystemShouldHaveLibrary()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"Library\"}" +
				"}}";
			Assert.NotNull(StoredSystem(manifest).Libraries["970a310f-90a7-4d29-8380-5b2a742e3ee4"], "Library should be stored with identifier, not be null.");
		}

		[Test]
		public void SystemShouldHaveProgram()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}" +
				"}}";
			Assert.NotNull(StoredSystem(manifest).Programs["970a310f-90a7-4d29-8380-5b2a742e3ee4"], "Program should be stored with identifier, not be null.");
		}

		[Test]
		public void SystemShouldHaveModel()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{" +
					"\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"Model\"}" +
				"}}";
			Assert.NotNull(StoredSystem(manifest).Models["970a310f-90a7-4d29-8380-5b2a742e3ee4"], "Program should be stored with identifier, not be null.");
		}

		[Test]
		[Ignore("No zip to read from")]
		public void SystemShouldHaveType()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\":\"Program\",\"format\":\"tweedle\",\"files\":[\"src/Program.twe\"],\"type\":\"Class\"}" +
				"]}";
			Assert.NotNull(StoredSystem(manifest).Types["Program"], "Type should be stored by name, not be null.");
		}

		[Test]
		[Ignore("No zip to read from")]
		public void SystemShouldHaveClass()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\":\"Program\",\"format\":\"tweedle\",\"files\":[\"src/Program.twe\"],\"type\":\"Class\"}" +
				"]}";
			Assert.NotNull(StoredSystem(manifest).Classes["Program"], "Class should be stored by name, not be null.");
		}

		[Test]
		[Ignore("No zip to read from")]
		public void SystemShouldHaveEnum()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\":\"TurnDirection\",\"format\":\"tweedle\",\"files\":[\"src/TurnDirection.twe\"],\"type\":\"Enum\"}" +
				"]}";
			Assert.NotNull(StoredSystem(manifest).Enums["TurnDirection"], "Enum should be stored by name, not be null.");
		}

		[Test]
		public void SystemShouldHaveAudio()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\": \"beast_growl_02_echo.mp3\", \"type\": \"audio\", \"format\": \"mpeg\", \"files\": [\"resources/beast_growl_02_echo.mp3\" ], \"uuid\": \"23d9dfb6-5cb0-4b55-bd05-1ec5bb133381\", \"duration\": 3.313}" +
				"]}";
			Assert.NotNull(StoredSystem(manifest).Resources[new ResourceIdentifier("beast_growl_02_echo.mp3", ContentType.Audio, "mpeg")], "Audio should be stored, not be null.");
		}

		[Test]
		public void AudioShouldHaveInformation()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\": \"beast_growl_02_echo.mp3\", \"type\": \"audio\", \"format\": \"mpeg\", \"files\": [\"resources/beast_growl_02_echo.mp3\" ], \"uuid\": \"23d9dfb6-5cb0-4b55-bd05-1ec5bb133381\", \"duration\": 3.313}" +
				"]}";
			AudioReference audio = (AudioReference)StoredSystem(manifest).Resources[new ResourceIdentifier("beast_growl_02_echo.mp3", ContentType.Audio, "mpeg")];
			Assert.AreEqual(new List<string>() { "resources/beast_growl_02_echo.mp3" }, audio.files, "Audio should have list of files.");
			Assert.AreEqual("23d9dfb6-5cb0-4b55-bd05-1ec5bb133381", audio.uuid, "Audio uuid should match.");
			Assert.AreEqual(3.313f, audio.duration, "Audio duration should match.");
		}

		[Test]
		public void SystemShouldHaveImage()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\": \"somePicture.png\", \"type\": \"image\", \"format\": \"png\", \"files\": [ \"resources/somePicture.png\" ], \"uuid\": \"9d7f2757-154a-4124-9d44-587025184679\", \"height\": 300, \"width\": 500}" +
				"]}";
			Assert.NotNull(StoredSystem(manifest).Resources[new ResourceIdentifier("somePicture.png", ContentType.Image, "png")], "Image should be stored, not be null.");
		}

		[Test]
		public void ImageShouldStoreInformation()
		{
			string manifest = "{ " +
				"\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
				"\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
				"\"metadata\":{\"identifier\":{\"id\":\"970a310f-90a7-4d29-8380-5b2a742e3ee4\",\"version\":\"1.0\",\"type\":\"World\"}}," +
				"\"prerequisites\":[]," +
				"\"resources\":[" +
					"{\"id\": \"somePicture.png\", \"type\": \"image\", \"format\": \"png\", \"files\": [ \"resources/somePicture.png\" ], \"uuid\": \"9d7f2757-154a-4124-9d44-587025184679\", \"height\": 300, \"width\": 500}" +
				"]}";
			ImageReference image = (ImageReference)StoredSystem(manifest).Resources[new ResourceIdentifier("somePicture.png", ContentType.Image, "png")];
			Assert.AreEqual("9d7f2757-154a-4124-9d44-587025184679", image.uuid, "Image uuid should match.");
			Assert.AreEqual(300f, image.height, "Image height should match.");
			Assert.AreEqual(500f, image.width, "Image width should match.");
		}

		[Test]
		[Ignore("TODO")]
		public void SystemShouldHaveSkeletonMesh()
		{
			// TODO
			Assert.Fail();
		}

		[Test]
		[Ignore("TODO")]
		public void SystemShouldHaveTexture()
		{
			// TODO
			Assert.Fail();
		}

		// Model Manifest
		// Can be moved to another file
		[Test]
		[Ignore("TODO")]
		public void ModelShouldHaveRootJoints()
		{
			// TODO
			Assert.Fail();
		}

		[Test]
		[Ignore("TODO")]
		public void ModelShouldHaveAdditionalJoints()
		{
			// TODO
			Assert.Fail();
		}

		[Test]
		[Ignore("TODO")]
		public void ModelShouldHavePoses()
		{
			// TODO
			Assert.Fail();
		}

		[Test]
		[Ignore("TODO")]
		public void ModelShouldHaveABoundingBox()
		{
			// TODO
			Assert.Fail();
		}

		[Test]
		[Ignore("TODO")]
		public void ModelShouldHaveTextureSets()
		{
			// TODO
		}

		[Test]
		[Ignore("TODO")]
		public void ModelShouldHaveStructures()
		{
			// TODO
			Assert.Fail();
		}

		[Test]
		[Ignore("TODO")]
		public void ModelShouldHaveModels()
		{
			// TODO
			Assert.Fail();
		}
	}
}
