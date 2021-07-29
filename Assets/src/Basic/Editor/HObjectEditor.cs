using HAN.Lib.Basic;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HAN.Editor
{
    /**
     * Custom editor for HObjectMonoBehavior. The wrapped HObjects properties will be exposed. 
     * In further versions this will provide the editing features for properties.
     */
    [CustomEditor( typeof( HObjectMonoBehavior ), true )]
    public class HObjectMonoBehaviorEditor : UnityEditor.Editor
    {
        private HObjectMonoBehavior m_instance;

        private List<IProperty> m_properties = new List<IProperty>();

        private GenericConnectionBookkeeper m_subscribers;


        private void OnEnable()
        {
            m_instance = ( (HObjectMonoBehavior) target );
            
            System.Reflection.FieldInfo[] propertyFields = m_instance.HObject.GetType().GetFields( System.Reflection.BindingFlags.NonPublic
                                                                                                 | System.Reflection.BindingFlags.Instance );
            for( int i = 0; i < propertyFields.Length; i++ )
            {
                var value = propertyFields[i].GetValue( m_instance.HObject );
                // Determine whether or not each field is a special name. 
                if( value is IProperty )
                {
                    m_properties.Add( (IProperty) value );
                }

                if( value is GenericConnectionBookkeeper )
                {
                    m_subscribers = (GenericConnectionBookkeeper) value;
                }
            }
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // show all properties
            showProperties();

            showSignals();
        }


        /**
         * Shows the list of all properties with values in a non editable list.
         */
        private void showProperties()
        {
            if( m_properties.Count > 0 )
            {
                GUILayout.Space( 10f );
                GUILayout.Label( "Properties:", EditorStyles.boldLabel );

                foreach( IProperty abstractProperty in m_properties )
                {
                    if( abstractProperty != null )
                    {
                        GUILayout.BeginHorizontal();

                        GUILayout.Label( abstractProperty.Id.Name, GUILayout.Width( 200 ) );
                        if( abstractProperty.ValueObject != null ) { 
                            GUILayout.Label( abstractProperty.ValueObject.ToString() );
                        }
                        else {
                            GUILayout.Label( "null" );
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }


        private void showSignals()
        {
            updateSubscribers();

            if( m_subscribers != null 
             && m_subscribers.Connections.Count > 0 )
            {
                GUILayout.Space( 10f );
                GUILayout.BeginHorizontal();
                GUILayout.Label( "Connections:", EditorStyles.boldLabel, GUILayout.Width( 400 ) );
                GUILayout.Label( "Signals:", EditorStyles.boldLabel, GUILayout.Width( 250 ) );
                GUILayout.EndHorizontal();

                foreach( var conn in m_subscribers.Connections )
                {
                    GUILayout.BeginHorizontal();
                    if( conn.Sender == m_instance.HObject ) {
                        GUILayout.Label( conn.Receiver.ToString(), GUILayout.Width( 400 ) );
                    }
                    else {
                        GUILayout.Label( conn.Sender.ToString(), GUILayout.Width( 400 ) );
                    }

                    GUILayout.Label( conn.Signal.ToString(), GUILayout.Width( 250 ) );
                    GUILayout.EndHorizontal();
                }
            }
        }


        private void updateSubscribers()
        {
            System.Reflection.FieldInfo field = m_instance.HObject.GetType().GetField( "m_connectionBookkeeper", System.Reflection.BindingFlags.NonPublic
                                                                                     | System.Reflection.BindingFlags.Instance );
            var value = field.GetValue( m_instance.HObject );

            if( value is GenericConnectionBookkeeper )
            {
                m_subscribers = (GenericConnectionBookkeeper) value;
            }
        }
    }
}
