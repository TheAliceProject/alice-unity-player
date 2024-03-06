using System;
using System.Collections.Generic;
using System.IO;
using Alice.Tweedle.File;
using Alice.Tweedle.VM;
using NUnit.Framework;
using UnityEngine;

namespace Alice.Tweedle.Parse {
    [TestFixture]
    public class TweedleSystemTest {
        private static readonly string TestWorld = $"{Path.Combine(Application.streamingAssetsPath, "Default")}/MinimalTestWorld.a3w";

        private TweedleSystem _system;
        private readonly LibraryCache _libraryCache = new();

        private TestVirtualMachine _vm;
        
        [SetUp]
        public void Setup() {
            _system = new TweedleSystem();
            TestHelpers.WaitOnEnumeratorTree(JsonParser.Parse(_system, TestWorld, _libraryCache, LogExceptionAsError));
            _system.Link();
        }

        private void Unload() {
            _system.Unload();
        }

        private void Run() {
            _vm = new TestVirtualMachine(_system);
            _vm.QueueProgramMain(_system);
        }

        private static void LogExceptionAsError(Exception e) {
            Debug.LogError($"Exception {e}"); 
        }

        [Test]
        public void SystemShouldLoadDefaultWorldProgram() {
        Assert.AreEqual(1, _system.Programs.Count, "System should have loaded one program");
        }

        [Test]
        public void DefaultWorldShouldLoadFile() {
            Assert.AreEqual(1, _system.LoadedFiles.Count, "System should have loaded from one file");
        }

        [Test]
        public void WorldLoadShouldCacheLibrary() {
            Assert.AreEqual(1, _libraryCache.Count, "System should have loaded one library");
        }

        [Test]
        public void WorldReloadShouldNotDuplicateCachedLibrary() {
            Unload();
            Setup();
            Assert.AreEqual(1, _libraryCache.Count, "System should have loaded one library");
        }

        [Test]
        public void WorldShouldRun() {
            Run();
            Assert.AreEqual(1, _libraryCache.Count, "System should have loaded one library");
        }

        [Test]
        public void WorldShouldRunAfterReload() {
            Run();
            Unload();
            Setup();
            Run();
            Assert.AreEqual(1, _libraryCache.Count, "System should have loaded one library");
        }

        /*[Test]
        public void SystemShouldLoadFile2() {
            
            Assert.IsTrue(StoredSystem(manifest).LoadedFiles.Contains(new ProjectIdentifier("Program", "1.0", "World")), "System should have loaded file");
        }*/
    }
}