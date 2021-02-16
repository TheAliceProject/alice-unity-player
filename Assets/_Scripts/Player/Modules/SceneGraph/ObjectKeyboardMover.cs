using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using BeauRoutine;

namespace Alice.Player.Unity {
    public class ObjectKeyboardMover{

        private List<SGTransformableEntity> m_movingObjects = new List<SGTransformableEntity>();

        private const float UP_DOWN_SCALE_FACTOR = 2.5f;
        private const float LEFT_RIGHT_SCALE_FACTOR = 70;

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
            if (movingTrans == null) {
                return;
            }
            if (KeyMap.UpKeys.Contains(theKey)) {
                var vp = VantagePoint.FromUnity(movingTrans.localPosition - BackwardMovement(movingTrans), movingTrans.localRotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
            if (KeyMap.DownKeys.Contains(theKey)) {
                var vp = VantagePoint.FromUnity(movingTrans.localPosition + BackwardMovement(movingTrans), movingTrans.localRotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
            if (KeyMap.LeftKeys.Contains(theKey)) {
                movingTrans.SetRotation(movingTrans.localRotation.eulerAngles.y - LEFT_RIGHT_SCALE_FACTOR * Time.deltaTime, Axis.Y, Space.Self);
                var vp = VantagePoint.FromUnity(movingTrans.localPosition, movingTrans.localRotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
            if (KeyMap.RightKeys.Contains(theKey)) {
                movingTrans.SetRotation(movingTrans.localRotation.eulerAngles.y + LEFT_RIGHT_SCALE_FACTOR * Time.deltaTime, Axis.Y, Space.Self);
                var vp = VantagePoint.FromUnity(movingTrans.localPosition, movingTrans.localRotation);
                movedObject.UpdateVantagePointProperty(vp);
            }
        }

        private static UnityEngine.Vector3 BackwardMovement(Transform movingTrans) {
            UnityEngine.Vector3 forward = movingTrans.localRotation * UnityEngine.Vector3.forward;
            UnityEngine.Vector3 forwardMotion = new UnityEngine.Vector3(forward.x, 0, forward.z);
            forwardMotion.Normalize();
            return forwardMotion * (UP_DOWN_SCALE_FACTOR * Time.deltaTime);
        }
    }
}
