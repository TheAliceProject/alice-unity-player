using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    
    [PInteropType]
    public abstract class SGEntity : MonoBehaviour {
        
        public static T Create<T>(string inName = "SGEntity") where T : SGEntity {
            var go = new GameObject(inName);
            var entity = go.AddComponent<T>();
            return entity;
        }
        
        #region Interop Interfaces
        [PInteropField]
        public static string POSITION_PROPERTY_NAME = "Position";
        [PInteropField]
        public static string ORIENTATION_PROPERTY_NAME = "Rotation";

        [PInteropMethod]
        public void bindPositionProperty(string name, PositionProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindOrientationProperty(string name, OrientationProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindDecimalNumberProperty(string name, DecimalNumberProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindSizeProperty(string name, SizeProperty property) {
            BindProperty(name, property);
        }
        #endregion

        protected abstract void BindProperty<T>(string inName, PropertyBase<T> inProperty);
    }
}