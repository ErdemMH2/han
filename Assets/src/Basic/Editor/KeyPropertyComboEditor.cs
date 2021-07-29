//using HAN.Lib.Structure;
//using HAN.Lib.Structure.Editor;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//namespace HAN.Lib.Basic.Editor
//{
//    /**
//     * Shows a collection of Property editors, while the 
//     * Key editor will be shown as a selectable entry, 
//     * when provided via a_keySelection in the constructor
//     */
//    public class KeyPropertyComboEditor : PropertyComboEditor
//    {
//        protected Dictionary<Key, KeyListCombo> m_keyEditors = new Dictionary<Key, KeyListCombo>();

//        public KeyPropertyComboEditor( IEnumerable<IProperty> a_usableProperties
//                                     , Dictionary<Key, IEnumerable<Key>> a_keySelection )
//          : base( a_usableProperties )
//        {
//            foreach( var keyPair in a_keySelection )
//            {
//                m_keyEditors.Add( keyPair.Key, new KeyListCombo( new List<Key>( keyPair.Value ) ) );
//            }
//        }


//        public void PreselectKey( Key a_id, Key a_value )
//        {
//            if( m_keyEditors.TryGetValue( a_id, out var keyEditor ) )
//            {
//                keyEditor.Preselect( a_value );
//            }
//        }


//        public override void TryLoad( IEnumerable<IProperty> a_properties )
//        {
//            foreach( var property in a_properties )
//            {
//                if( property is Property<Key> keyProp )
//                {
//                    PreselectKey( keyProp.Id, keyProp.Value );
//                }
//            }

//            base.TryLoad( a_properties );
//        }


//        protected override void keyEditor( Property<Key> a_key )
//        {
//            if( m_keyEditors.TryGetValue( a_key.Id, out var keyEditor ) )
//            {
//                keyEditor.OnGUI();
//                a_key.SetValue( keyEditor.Selected );
//            }
//            else
//            {
//                a_key.SetValue( EditorGUILayout.TextField( a_key.Value.Name, GUILayout.Width( 300 ) ) );
//            }
//        }

//    }
//}