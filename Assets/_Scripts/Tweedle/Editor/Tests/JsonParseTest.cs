using System;
using System.Collections;
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
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\":\"Program\",\"format\":\"tweedle\",\"file\": \"src/Program.twe\",\"type\":\"Class\"}," +
                    "{\"name\":\"TurnDirection\",\"format\":\"tweedle\",\"file\":\"src/TurnDirection.twe\",\"type\":\"Enum\"}," +
                    "{\"name\": \"Alien\", \"type\": \"model\", \"format\": \"json\", \"file\":  \"resources/alienCollada/model.json\" }," +
                    "{\"name\": \"beast_growl_02_echo.mp3\", \"type\": \"audio\", \"format\": \"mpeg\", \"file\": \"resources/beast_growl_02_echo.mp3\", \"uuid\": \"23d9dfb6-5cb0-4b55-bd05-1ec5bb133381\", \"duration\": 3.313}," +
                    "{\"name\": \"somePicture.png\", \"type\": \"image\", \"format\": \"png\", \"file\": \"resources/somePicture.png\", \"uuid\": \"9d7f2757-154a-4124-9d44-587025184679\", \"height\": 300, \"width\": 500}," +
                "]}";
         */

        [SetUp]
        public void Setup()
        {
            root = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\_Scripts\\Tweedle\\Parsing\\Editor\\JsonParse Files");
        }

        private TweedleSystem StoredSystem(string str)
        {
            var system = new TweedleSystem();
            var json = new JsonParser(system);
            TestHelpers.WaitOnEnumeratorTree(json.LoadStandAloneProject(str));
            return system;
        }

        // Tweedle System
        [Test]
        public void SystemShouldLoadFile()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{" +
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}}";
            Assert.IsTrue(StoredSystem(manifest).LoadedFiles.Contains(new ProjectIdentifier("Program", "1.0", "World")), "System should have loaded file");
        }

        [Test]
        public void LoadFileShouldHaveMetaData()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{" +
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}}";
            TweedleSystem system = StoredSystem(manifest);
            Assert.AreEqual("Program", system.LoadedFiles.First().name, "System's name should match.");
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
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}}";
            ProgramDescription program = StoredSystem(manifest).Programs["Program"];
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
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}}";
            ProgramDescription program = StoredSystem(manifest).Programs["Program"];
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
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}}";
            ProgramDescription program = StoredSystem(manifest).Programs["Program"];
            Assert.AreEqual("0.1", program.metadata.formatVersion, "File's format versions should match.");
            Assert.AreEqual("Program", program.metadata.identifier.name, "File's name should match.");
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
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\":\"Program\",\"format\":\"tweedle\",\"file\":\"src/Program.twe\",\"type\":\"Class\"}" +
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
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\": \"Alien\", \"type\": \"model\", \"format\": \"json\", \"file\": \"resources/alienCollada/model.json\"}" +
                "]}";
            Assert.NotNull(StoredSystem(manifest).Resources[new ResourceIdentifier("Alien", ContentType.Model, "json")], "Resources should store a model.");
        }

        [Test]
        public void SystemShouldHaveLibrary()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Scene Graph Library\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{" +
                    "\"identifier\":{\"name\":\"SceneGraphLibrary\",\"version\":\"1.0\",\"type\":\"Library\"}" +
                "}}";
            Assert.NotNull(StoredSystem(manifest).Libraries["SceneGraphLibrary"], "Library should be stored with identifier, not be null.");
        }

        [Test]
        public void SystemShouldHaveProgram()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{" +
                    "\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}" +
                "}}";
            Assert.NotNull(StoredSystem(manifest).Programs["Program"], "Program should be stored with identifier, not be null.");
        }

        [Test]
        public void SystemShouldHaveModel()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{" +
                    "\"identifier\":{\"name\":\"SomeModel\",\"version\":\"1.0\",\"type\":\"Model\"}" +
                "}}";
            Assert.NotNull(StoredSystem(manifest).Models["SomeModel"], "Model should be stored with identifier, not be null.");
        }

        [Test]
        public void UnrecognizedSystemShouldThrow()
        {
            var manifest = "{ " +
                           "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                           "\"provenance\":{\"aliceVersion\":\"3.6.1.0\",\"creationYear\":\"2022\",\"creator\":\"Anonymous\"}," +
                           "\"metadata\":{" +
                           "\"identifier\":{\"name\":\"NewThing\",\"version\":\"1.0\",\"type\":\"NewThing\"}" +
                           "}}";
            Assert.Throws<ArgumentException>(() => StoredSystem(manifest));
        }

        [Test]
        [Ignore("No zip to read from")]
        public void SystemShouldHaveType()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\":\"SomeType\",\"format\":\"tweedle\",\"file\": \"src/Program.twe\",\"type\":\"Class\"}" +
                "]}";
            Assert.NotNull(StoredSystem(manifest).TypeNamed("SomeType"), "Type should be stored by name, not be null.");
        }

        [Test]
        [Ignore("No zip to read from")]
        public void SystemShouldHaveClass()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\":\"SomeClass\",\"format\":\"tweedle\",\"file\":\"src/Program.twe\",\"type\":\"Class\"}" +
                "]}";
            Assert.NotNull(StoredSystem(manifest).TypeNamed("SomeClass"), "Class should be stored by name, not be null.");
        }

        [Test]
        [Ignore("Enums not implemented\nNo zip to read from")]
        public void SystemShouldHaveEnum()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\":\"TurnDirection\",\"format\":\"tweedle\",\"file\":\"src/TurnDirection.twe\",\"type\":\"Enum\"}" +
                "]}";
            Assert.NotNull(StoredSystem(manifest).TypeNamed("TurnDirection"), "Enum should be stored by name, not be null.");
        }

        [Test]
        public void SystemShouldHaveAudio()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\": \"beast_growl_02_echo.mp3\", \"type\": \"audio\", \"format\": \"mpeg\", \"file\": \"resources/beast_growl_02_echo.mp3\", \"uuid\": \"23d9dfb6-5cb0-4b55-bd05-1ec5bb133381\", \"duration\": 3.313}" +
                "]}";
            var resources = StoredSystem(manifest).Resources;
            using IEnumerator<ResourceIdentifier> keys = resources.Keys.GetEnumerator();
            keys.Reset();
            while (keys.MoveNext()) {
                resources.TryGetValue(keys.Current, out var val);
            }
            Assert.NotNull(StoredSystem(manifest).Resources[new ResourceIdentifier("resources/beast_growl_02_echo.mp3", ContentType.Audio, "mpeg")], "Audio should be stored, not be null.");
        }

        [Test]
        public void AudioShouldHaveInformation()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\": \"beast_growl_02_echo.mp3\", \"type\": \"audio\", \"format\": \"mpeg\", \"file\": \"resources/beast_growl_02_echo.mp3\", \"uuid\": \"23d9dfb6-5cb0-4b55-bd05-1ec5bb133381\", \"duration\": 3.313}" +
                "]}";
            AudioReference audio = (AudioReference)StoredSystem(manifest).Resources[new ResourceIdentifier("resources/beast_growl_02_echo.mp3", ContentType.Audio, "mpeg")];
            Assert.AreEqual("resources/beast_growl_02_echo.mp3", audio.file, "Audio should a file.");
            Assert.AreEqual("23d9dfb6-5cb0-4b55-bd05-1ec5bb133381", audio.uuid, "Audio uuid should match.");
            Assert.AreEqual(3.313f, audio.duration, "Audio duration should match.");
        }

        [Test]
        public void SystemShouldHaveImage()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\": \"somePicture.png\", \"type\": \"image\", \"format\": \"png\", \"file\": \"resources/somePicture.png\", \"uuid\": \"9d7f2757-154a-4124-9d44-587025184679\", \"height\": 300, \"width\": 500}" +
                "]}";
            Assert.NotNull(StoredSystem(manifest).Resources[new ResourceIdentifier("resources/somePicture.png", ContentType.Image, "png")], "Image should be stored, not be null.");
        }

        [Test]
        public void ImageShouldStoreInformation()
        {
            string manifest = "{ " +
                "\"description\":{ \"name\":\"Program\",\"icon\":\"thumbnail.png\",\"tags\":[],\"groupTags\":[],\"themeTags\":[]}," +
                "\"provenance\":{\"aliceVersion\":\"3.4.0.0-alpha\",\"creationYear\":\"2018\",\"creator\":\"Anonymous\"}," +
                "\"metadata\":{\"identifier\":{\"name\":\"Program\",\"version\":\"1.0\",\"type\":\"World\"}}," +
                "\"prerequisites\":[]," +
                "\"resources\":[" +
                    "{\"name\": \"somePicture.png\", \"type\": \"image\", \"format\": \"png\", \"file\": \"resources/somePicture.png\", \"uuid\": \"9d7f2757-154a-4124-9d44-587025184679\", \"height\": 300, \"width\": 500}" +
                "]}";
            ImageReference image = (ImageReference)StoredSystem(manifest).Resources[new ResourceIdentifier("resources/somePicture.png", ContentType.Image, "png")];
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
