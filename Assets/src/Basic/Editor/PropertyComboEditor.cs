using HAN.Lib.Structure;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HAN.Lib.Basic.Editor
{
    /**
     * Shows a collection of Property editors, which can be selected
     * exclusivly from a combo box
     */
    public class PropertyComboEditor : PropertyAccessEditor
    {
        private Dictionary<Key, IProperty> m_template;
        private IEnumerable<IProperty> m_usableProperties;

        public PropertyComboEditor( IEnumerable<IProperty> a_usableProperties )
        {
            m_usableProperties = a_usableProperties;
            m_template = a_usableProperties.ToDictionary( prop => prop.Id, prop => prop );
        }


        public void Preselect( Key a_id )
        {
            if( m_template.TryGetValue( a_id, out var property ) )
            {
                Add( property );
                m_template.Remove( a_id ); // we dont want to select it anymore
            }
        }


        public override void TryLoad( IEnumerable<IProperty> a_properties )
        {
            foreach( var property in a_properties )
            {
                Preselect( property.Id );
            }
            base.TryLoad( a_properties );
        }

        public override void Clear()
        {
            base.Clear();

            m_template = m_usableProperties.ToDictionary( prop => prop.Id, prop => prop );
        }

        public override void OnGUI()
        {
            if( m_template.Count > 0 )
            { 
                if( GUILayout.Button( "Add" ) )
                {
                    GenericMenu menu = new GenericMenu();

                    foreach( Key labelKey in m_template.Keys )
                    {
                        menu.AddItem( new GUIContent( labelKey.Name ),
                                      false,
                                      onSelected,
                                      labelKey.Name );
                    }

                    // display the menu
                    menu.ShowAsContext();
                }
            }

            base.OnGUI();
        }


        private void onSelected( object a_entry )
        {
            Key id = KeyFactory.Create( a_entry.ToString() );

            // deselect
            if( m_properties.Remove( id ) )
            {
                return;
            }

            if( m_template.ContainsKey( id ) )
            { 
                // else select
                Add( m_template[id] );
            }
        }
    }
}
