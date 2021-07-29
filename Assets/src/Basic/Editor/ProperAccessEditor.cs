using HAN.Lib.Structure;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HAN.Lib.Basic.Editor
{
    /**
     * Shows a collection of Property-Editors 
     */
    public class PropertyAccessEditor : EditorWindow
    {
        public IReadOnlyDictionary<Key, IProperty> Properties { get { return m_properties; } }

        protected Dictionary<Key, IProperty> m_properties = new Dictionary<Key, IProperty>();

        public virtual void OnGUI()
        {
            if( m_properties == null )
                return;

            foreach( var property in m_properties.Values )
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label( property.Id.Name, GUILayout.Width( 150 ) );

                if( property is Property<int> propInt ) {
                    intEditor( propInt );
                }
                else if( property is Property<bool> propBool ) {
                    boolEditor( propBool );
                }
                else if( property is Property<string> propString ) {
                    stringEditor( propString );
                }
                else if( property is Property<Key> propKey ) {
                    keyEditor( propKey );
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void intEditor( Property<int> a_int )
        {
            a_int.SetValue( EditorGUILayout.IntField( a_int.Value, GUILayout.Width( 300 ) ) );
        }
         
        private void boolEditor( Property<bool> a_bool )
        {
            a_bool.SetValue( EditorGUILayout.Toggle( a_bool.Value, GUILayout.Width( 300 ) ) );
        }

        private void stringEditor( Property<string> a_string )
        {
            a_string.SetValue( EditorGUILayout.TextField( a_string.Value, GUILayout.Width( 300 ) ) );
        }

        protected virtual void keyEditor( Property<Key> a_key )
        {
            a_key.SetValue( EditorGUILayout.TextField( a_key.Value.Name, GUILayout.Width( 300 ) ) );
        }


        public void Add( IProperty a_property )
        {
            m_properties[a_property.Id] = a_property;
        }


        public void Remove( Key a_id )
        {
            m_properties.Remove( a_id );
        }


        public T Value<T>( Key a_id )
        {
            if( m_properties.TryGetValue( a_id, out var property ) 
             && property is Property<T> propertyT )
            {
                return propertyT.Value;
            }
            else
            {
                HAN.Debug.Logger.Error( "PropertyAccessEditor"
                                      , string.Format( " Cannot find or cast property {0}", a_id ) );
            }

            return default;
        }


        public virtual void TryLoad( IEnumerable<IProperty> a_properties )
        {
            foreach( var property in a_properties )
            {
                if( m_properties.TryGetValue( property.Id, out var savedProperty ) )
                {
                    savedProperty.TrySetValue( property.ValueObject );
                }
            }
        }


        public virtual void Clear()
        {
            m_properties.Clear();
        }
    }
}
