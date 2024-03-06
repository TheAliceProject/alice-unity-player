using System.Collections;
using System.Collections.Generic;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Parse {
    public class LibraryCache {
        private Dictionary<ProjectIdentifier, TweedleSystem> m_LoadedSystems = new();
        private List<ProjectIdentifier> m_LoadingSystems = new();
        public int Count => m_LoadedSystems.Count;

        public IEnumerator Read(ProjectIdentifier identifier, string path, JsonParser.ExceptionHandler handler) {
            if (m_LoadedSystems.ContainsKey(identifier)) {
                // It has already been read and loaded
                yield break;
            }
            
            if (m_LoadingSystems.Contains(identifier)) {
                // Another process started loading. Wait for that to complete
                while (m_LoadingSystems.Contains(identifier)) {
                    yield return null;
                }
                yield break;
            }

            // First process to ask for it. Read it in.
            m_LoadingSystems.Add(identifier);
            var system = new TweedleSystem();
            yield return JsonParser.Parse(system, path, this, handler);
            m_LoadedSystems.Add(identifier, system);
            m_LoadingSystems.Remove(identifier);
        }

        public bool TryGetValue(ProjectIdentifier identifier, out TweedleSystem cachedLibrary) {
            return m_LoadedSystems.TryGetValue(identifier, out cachedLibrary);
        }

        public TAssembly GetRuntimeAssembly(ProjectIdentifier identifier) {
            return m_LoadedSystems.TryGetValue(identifier, out TweedleSystem library) ? library.GetRuntimeAssembly() : null;
        }
    }
}