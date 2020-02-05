using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using BeauRoutine;

namespace Alice.Player.Unity {
    public class ObjectKeyboardMover{

        private List<SGTransformableEntity> m_movingObjects = new List<SGTransformableEntity>();

        private const float UP_DOWN_SCALE_FACTOR = 0.055f;
        private const float LEFT_RIGHT_SCALE_FACTOR = 1.612f;

        public void AddObject(SGTransformableEntity entityToMove){
            m_movingObjects.Add(entityToMove);
        }

        public bool HasObjects(){
            return m_movingObjects.Count > 0;
        }

        public void NotifyEvent(Key theKey){
            if(!KeyMap.ArrowKeys.Contains(theKey))
                return;

            foreach(var movedObject in m_movingObjects) {
                move(movedObject, theKey);
            }
        }

        private void move(SGTransformableEntity movedObject, Key theKey) {
            var movingTrans = movedObject.cachedTransform;
            if (KeyMap.UpKeys.Contains(theKey)) {
                var p = movingTrans.position - movingTrans.forward * UP_DOWN_SCALE_FACTOR;
                VantagePoint vp = VantagePoint.FromUnity(p, movingTrans.rotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
            if (KeyMap.DownKeys.Contains(theKey)) {
                var p = movingTrans.position + movingTrans.forward * UP_DOWN_SCALE_FACTOR;
                VantagePoint vp = VantagePoint.FromUnity(p, movingTrans.rotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
            if (KeyMap.LeftKeys.Contains(theKey)) {
                movingTrans.SetRotation(movingTrans.localRotation.eulerAngles.y - LEFT_RIGHT_SCALE_FACTOR, Axis.Y, Space.Self);
                VantagePoint vp = VantagePoint.FromUnity(movingTrans.position, movingTrans.rotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
            if (KeyMap.RightKeys.Contains(theKey)) {
                movingTrans.SetRotation(movingTrans.localRotation.eulerAngles.y + LEFT_RIGHT_SCALE_FACTOR, Axis.Y, Space.Self);
                VantagePoint vp = VantagePoint.FromUnity(movingTrans.position, movingTrans.rotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
        }
    }
}
