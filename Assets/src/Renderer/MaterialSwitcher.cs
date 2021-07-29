using HAN.Lib.Structure;
using System.Collections.Generic;
using UnityEngine;


namespace HAN.Lib.Basic.Renderer
{
    /**
     * Changes the Material of given meshes to the Material with a_id.
     * The Materials has to be added before.  
     */
    public class MaterialSwitcher : MonoBehaviour
    {
        private Dictionary<MeshRenderer,Material> m_defaultMeshMaterials = new Dictionary<MeshRenderer, Material>();

        private Dictionary<Key, Material> m_materials = new Dictionary<Key, Material>();


        /**
         * Inits with all children meshes
         */
        public void Init()
        {
            var meshes = GetComponentsInChildren<MeshRenderer>();
            Init( meshes );
        }


        /**
         * Inits with given meshes
         */
        public void Init( MeshRenderer[] a_meshRenderer )
        {
            m_defaultMeshMaterials.Clear();

            foreach( var meshRenderer in a_meshRenderer )
            {
                m_defaultMeshMaterials.Add( meshRenderer, meshRenderer.material );
            }
        }
        

        public void AddMaterial( Key a_id, Material a_mat )
        {
            m_materials.Add( a_id, a_mat );
        }


        public Material GetMaterial( Key a_id )
        {
            if( m_materials.ContainsKey( a_id ) )
            {
                return m_materials[a_id];
            }

            return null;
        }


        public void SetMaterial( Key a_id )
        {
            if( m_materials.TryGetValue( a_id, out Material material ) )
            { 
                foreach( var meshMatPair in m_defaultMeshMaterials )
                {
                    meshMatPair.Key.material = material;
                }
            }
        }


        public void SetDefaultMaterial()
        {
            foreach( var meshMatPair in m_defaultMeshMaterials )
            {
                meshMatPair.Key.material = meshMatPair.Value;
            }
        }
    }
}
