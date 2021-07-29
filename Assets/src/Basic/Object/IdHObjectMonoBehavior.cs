using HAN.Lib.Structure;
using UnityEngine;

namespace HAN.Lib.Basic.Unity
{
    public class IdHObjectMonoBehavior : DefaultHObjectMonoBehavior
    {
        public virtual Key Id
        {
            get
            {
                if( m_id == null )
                {
                    KeyHolder keyholder = null;

                    if( m_keyHolder != null )
                    {
                        keyholder = m_keyHolder;
                    }
                    else
                    {
                        keyholder = GetComponent<KeyHolder>();
                    }

                    if( keyholder != null )
                    {
                        m_id = keyholder.Key;
                    }
                    else
                    {
                        m_id = Key.Null;
                    }
                }

                return m_id;
            }

            protected set
            {
                m_id = value;
            }
        }

        [SerializeField] private KeyHolder m_keyHolder;
        private Key m_id;
    }
}