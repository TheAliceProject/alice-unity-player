using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Unity {
    
    public abstract class SGModel : SGTransformableEntity {
        protected static void CreateModelObject(Mesh inMesh, Material inMaterial, Transform inParent, out Transform outTransform, out Renderer outRenderer, out MeshFilter outFilter) {
            var go = new GameObject("Model");
            outFilter = go.AddComponent<MeshFilter>();
            outFilter.mesh = inMesh;
            outRenderer = go.AddComponent<MeshRenderer>();
            outRenderer.sharedMaterial = inMaterial;
            outTransform = go.transform;

            outTransform.SetParent(inParent, false);
            outTransform.localPosition = UnityEngine.Vector3.zero;
            outTransform.localRotation = UnityEngine.Quaternion.identity;
        }

        protected static void GetPropertyBlock(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock) {
            GetPropertyBlock(inRenderer, ref ioPropertyBlock, 0, true);
        }

        protected static void GetPropertyBlock(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, int materialIndex, bool shared) {
            Debug.Assert(materialIndex == 0 || !shared);

            if (ioPropertyBlock == null) {
                ioPropertyBlock = new MaterialPropertyBlock();
            }

            if (inRenderer.HasPropertyBlock()) {
                if (shared) {
                    inRenderer.GetPropertyBlock(ioPropertyBlock);
                } else { 
                    inRenderer.GetPropertyBlock(ioPropertyBlock, materialIndex);
                }
            }
        }

        private static void SetPropertyBlock(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, int materialIndex, bool shared) {
            Debug.Assert(materialIndex == 0 || !shared);

            if (shared) {
                inRenderer.SetPropertyBlock(ioPropertyBlock);
            } else {
                inRenderer.SetPropertyBlock(ioPropertyBlock, materialIndex);
            }
        }

        public const string PAINT_PROPERTY_NAME = "Paint";
        public const string BACK_PAINT_PROPERTY_NAME = "BackPaint";
        public const string SIZE_PROPERTY_NAME = "Size";
        public const string OPACITY_PROPERTY_NAME = "Opacity";

        public const string MAIN_TEXTURE_SHADER_NAME = "_MainTex";
        public const string FILTER_TEXTURE_SHADER_NAME = "_FilterTex";
        public const string COLOR_SHADER_NAME = "_Color";

        protected Transform m_ModelTransform;
        protected Bounds m_CachedMeshBounds;
        
        private Paint m_CachedPaint = Primitives.Color.WHITE;
        protected float m_CachedOpacity = 1;

        protected Bounds MeshBounds { get {return m_CachedMeshBounds; } }

        protected virtual string PaintTextureName { get { return FILTER_TEXTURE_SHADER_NAME; } }
        protected virtual Material OpaqueMaterial { get { return SceneGraph.Current?.InternalResources?.OpaqueMaterial; } }
        protected virtual Material TransparentMaterial { get { return SceneGraph.Current?.InternalResources?.TransparentMultipassMaterial; } }

        protected override void Awake() {
            base.Awake();

            RegisterPropertyDelegate(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
            RegisterPropertyDelegate(OPACITY_PROPERTY_NAME, OnOpacityPropertyChanged);
        }

        public UnityEngine.Vector3 GetSize(bool inDynamic) {
            UnityEngine.Vector3 size = inDynamic ? GetMeshBounds().size : m_CachedMeshBounds.size;
            size.Scale(m_ModelTransform.localScale);
            return size;
        }

        public Bounds GetBounds(bool inDynamic) {

            Bounds bounds = inDynamic ? GetMeshBounds() : m_CachedMeshBounds;
            var scale = m_ModelTransform.localScale;

            var center = bounds.center;
            center.Scale(scale);
            var size = bounds.size;
            size.Scale(scale);

            bounds.center = center + m_ModelTransform.localPosition;
            bounds.size = size;

            return bounds;
        }

        public Bounds GetBoundsInWorldSpace(bool inDynamic) {
            var bounds = GetBounds(inDynamic);
            bounds.center += cachedTransform.position;
            return bounds;
        }

        protected void CacheMeshBounds() {
            m_CachedMeshBounds = GetMeshBounds();
        }

        protected abstract Bounds GetMeshBounds();

        private void OnSizePropertyChanged(TValue inValue) {
            SetSize(inValue.RawObject<Size>());
        }

        protected virtual void SetSize(UnityEngine.Vector3 inSize) {
            var meshSize = m_CachedMeshBounds.size;
            m_ModelTransform.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : inSize.x/meshSize.x,
                meshSize.y == 0 ? 1 : inSize.y/meshSize.y,
                meshSize.z == 0 ? 1 : inSize.z/meshSize.z
            );
        }

        public virtual Scale GetScale(){
            UnityEngine.Vector3 scale = transform.GetChild(0).localScale;
            return new Scale(scale.x, scale.y, scale.z);
        }

        private void OnPaintPropertyChanged(TValue inValue) {
            m_CachedPaint = inValue.RawObject<Paint>();  
            OnPaintChanged();
        }

        protected abstract void OnPaintChanged();
        
        protected void ApplyPaint(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, BaseMaterial baseMaterial = BaseMaterial.Opaque) {
        	ApplyPaint(inRenderer, ref ioPropertyBlock, 0, true, baseMaterial);
        }

        protected void ApplyPaint(Renderer inRenderer, ref MaterialPropertyBlock[] ioPropertyBlocks, BaseMaterial[] baseMaterial)
        {
            for (int i = 0; i < ioPropertyBlocks.Length; i++) {
                ApplyPaint(inRenderer, ref ioPropertyBlocks[i], i, false, baseMaterial[i]);
            }
        }

        private void ApplyPaint(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, int materialIndex, bool shared, BaseMaterial baseMaterial = BaseMaterial.Opaque) {
            GetPropertyBlock(inRenderer, ref ioPropertyBlock, materialIndex, shared);
            m_CachedPaint.Apply(ioPropertyBlock, m_CachedOpacity, PaintTextureName, baseMaterial);
            SetPropertyBlock(inRenderer, ref ioPropertyBlock, materialIndex, shared);
        }

        private void OnOpacityPropertyChanged(TValue inValue) {
            m_CachedOpacity = (float)inValue.RawObject<Portion>().Value;
            OnOpacityChanged();
        }

        protected abstract void OnOpacityChanged();
        
        protected void ApplyOpacity(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, BaseMaterial baseMaterial = BaseMaterial.Opaque) {
            ApplyCurrentPaintAndOpacity(inRenderer, ref ioPropertyBlock, baseMaterial);
        }
        
        protected void ApplyOpacity(Renderer inRenderer, ref MaterialPropertyBlock[] ioPropertyBlocks, BaseMaterial[] baseMaterial) {
            ApplyCurrentPaintAndOpacity(inRenderer, ref ioPropertyBlocks, baseMaterial);
        }

        private void ApplyCurrentPaintAndOpacity(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, BaseMaterial baseMaterial) {
            ApplyPaintAndCurrentOpacity(inRenderer, ref ioPropertyBlock, m_CachedPaint, 0,  true, baseMaterial);
        }


        protected void ApplyCurrentPaintAndOpacity(Renderer inRenderer, ref MaterialPropertyBlock[] ioPropertyBlocks, BaseMaterial[] baseMaterial) {
            ApplyPaintAndCurrentOpacity(inRenderer, ref ioPropertyBlocks, m_CachedPaint, baseMaterial);
        }

        private void ApplyPaintAndCurrentOpacity(Renderer inRenderer, ref MaterialPropertyBlock[] ioPropertyBlocks, Paint inPaint, BaseMaterial[] baseMaterial) {
            for (int i = 0; i < ioPropertyBlocks.Length; i++) {
                ApplyPaintAndCurrentOpacity(inRenderer, ref ioPropertyBlocks[i], inPaint, i, false, baseMaterial[i]);
            }
        }

        protected void ApplyPaintAndCurrentOpacity(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, Paint inPaint, BaseMaterial baseMaterial = BaseMaterial.Opaque) {
            ApplyPaintAndCurrentOpacity(inRenderer, ref ioPropertyBlock, inPaint, 0, true, baseMaterial);
        }

        private void ApplyPaintAndCurrentOpacity(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock, Paint inPaint, int materialIndex, bool shared, BaseMaterial baseMaterial) {
            if (m_CachedOpacity < 0.004f) {
                if (inRenderer.enabled) {
                    inRenderer.enabled = false;
                }
                return;
            }
            if (!inRenderer.enabled) {
                inRenderer.enabled = true;
            }

            GetPropertyBlock(inRenderer, ref ioPropertyBlock, materialIndex, shared);
            var appliedOpacity = m_CachedOpacity;

            Material appliedMaterial;
            if (baseMaterial == BaseMaterial.Glass) {
                appliedMaterial = SceneGraph.Current.InternalResources.GlassMaterial;
                appliedOpacity = 0;
            } else if (baseMaterial == BaseMaterial.Transparent || m_CachedOpacity < 0.996f) {
                appliedMaterial = TransparentMaterial;
            } else {
                appliedMaterial = OpaqueMaterial;
            }

            if (inRenderer.sharedMaterials[materialIndex] != appliedMaterial) {
                var oldMats = inRenderer.sharedMaterials;
                var newMats = new Material[oldMats.Length];
                for (var i = 0; i < oldMats.Length; i++) {
                    newMats[i] = i == materialIndex ? appliedMaterial : oldMats[i];
                }
                inRenderer.sharedMaterials = newMats;
            }

            inPaint.Apply(ioPropertyBlock, appliedOpacity, PaintTextureName, baseMaterial);
            SetPropertyBlock(inRenderer, ref ioPropertyBlock, materialIndex, shared);
        }

        protected override void CreateEntityCollider()
        {
            var meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers == null) return;
            
            foreach (var meshRenderer in meshRenderers)
            {
                if (meshRenderer.transform.GetComponent<MeshCollider>() != null) continue;
                var meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
                // Rigid body is required for collision detection
                var rigidBody = meshRenderer.gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
                meshRenderer.gameObject.AddComponent<CollisionBroadcaster>();
            }
        }

        protected override void CreateMouseCollider()
        {
            var meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers == null) return;
            
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.gameObject.AddComponent<MeshCollider>();
                meshRenderer.gameObject.AddComponent<CollisionBroadcaster>();
            }
        }

        public override void CleanUp() {}

    }
}