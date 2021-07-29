using HAN.Lib.Basic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using HAN.Lib.Structure;

namespace HAN.Lib.Event.Unity
{
    /**
     * GamepadInputManagerHBehavior is a Singleton. It catchs the unity event input and emit
     * signals to the subscriber.
     * */
    public class GamepadInputManagerHBehavior : SingletonHObjectMonobehavior<GamepadInputManagerHBehavior>
    {
        public override IHANObject HObject { get { return m_gamepadInputManager; } }

        private static GamepadInputManager m_gamepadInputManager;


        private bool m_horizontalAxisPressed = false;
        private bool m_verticalAxisPressed = false;


        public GamepadInputManagerHBehavior()
        {
            m_gamepadInputManager = new GamepadInputManager();
        }


        void Update()
        {
            if( m_horizontalAxisPressed )
            {
                if( Input.GetAxis( "Horizontal" ) == 0 )
                {
                    m_horizontalAxisPressed = false;
                }
            }

            if( m_verticalAxisPressed )
            {
                if( Input.GetAxis( "Vertical" ) == 0 )
                {
                    m_verticalAxisPressed = false;
                }
            }

            if( Input.GetKeyDown( KeyCode.JoystickButton1 ) ) // X
            {
                m_gamepadInputManager.SignalX.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.XButton, GamePadKeys.Signal.Clicked ) );
            }
            else if( Input.GetKeyDown( KeyCode.JoystickButton2 ) ) // O
            {
                m_gamepadInputManager.SignalO.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.CircleButton, GamePadKeys.Signal.Clicked ) );
            }
            else if( Input.GetKeyDown( KeyCode.JoystickButton4 ) ) // L1
            {
                m_gamepadInputManager.SignalO.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.L1Button, GamePadKeys.Signal.Clicked ) );
            }
            else if( Input.GetKeyDown( KeyCode.JoystickButton5 ) ) // R1
            {
                m_gamepadInputManager.SignalO.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.R1Button, GamePadKeys.Signal.Clicked ) );
            }
            else if( Input.GetAxis( "Horizontal" ) > 0 ) // Right Arrow
            {
                if( !m_horizontalAxisPressed )
                {
                    m_gamepadInputManager.SignalRightArrow.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.RightArrow, GamePadKeys.Signal.Clicked ) );
                    m_horizontalAxisPressed = true;
                }
            }
            else if( Input.GetAxis( "Horizontal" ) < 0 ) // Left Arrow
            {
                if( !m_horizontalAxisPressed )
                {
                    m_gamepadInputManager.SignalLeftArrow.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.LeftArrow, GamePadKeys.Signal.Clicked ) );
                    m_horizontalAxisPressed = true;
                }
            }
            else if( Input.GetAxis( "Vertical" ) < 0 ) // Down Arrow
            {
                if( !m_verticalAxisPressed )
                {
                    m_gamepadInputManager.SignalDownArrow.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.DownArrow, GamePadKeys.Signal.Clicked ) );
                    m_verticalAxisPressed = true;
                }
            }
            else if( Input.GetAxis( "Vertical" ) > 0 ) // Up Arrow
            {
                if( !m_verticalAxisPressed )
                {
                    m_gamepadInputManager.SignalUpArrow.Emit( new GamePadSignalParameter( this, GamePadKeys.Input.UpArrow, GamePadKeys.Signal.Clicked ) );
                    m_verticalAxisPressed = true;
                }
            }

        }

        private class GamepadInputManager : HObject
        {
            public Signal SignalX { get { return m_signal_x; } }
            public Signal SignalO { get { return m_signal_o; } }
            public Signal SignalR1 { get { return m_signal_r1; } }
            public Signal SignalL1 { get { return m_signal_l1; } }
            public Signal SignalRightArrow { get { return m_signal_right_Arrow; } }
            public Signal SignalLeftArrow { get { return m_signal_left_Arrow; } }
            public Signal SignalUpArrow { get { return m_signal_up_Arrow; } }
            public Signal SignalDownArrow { get { return m_signal_down_Arrow; } }

            private Signal m_signal_x;
            private Signal m_signal_o;
            private Signal m_signal_r1;
            private Signal m_signal_l1;
            private Signal m_signal_right_Arrow;
            private Signal m_signal_left_Arrow;
            private Signal m_signal_up_Arrow;
            private Signal m_signal_down_Arrow;


            public GamepadInputManager()
            {
                m_signal_x = new Signal( this );
                m_signal_o = new Signal( this );
                m_signal_r1 = new Signal( this );
                m_signal_l1 = new Signal( this );
                m_signal_right_Arrow = new Signal( this );
                m_signal_left_Arrow = new Signal( this );
                m_signal_up_Arrow = new Signal( this );
                m_signal_down_Arrow = new Signal( this );

                addSignal( GamePadKeys.Input.XButton, m_signal_x );
                addSignal( GamePadKeys.Input.CircleButton, m_signal_o );
                addSignal( GamePadKeys.Input.R1Button, m_signal_r1 );
                addSignal( GamePadKeys.Input.L1Button, m_signal_l1 );
                addSignal( GamePadKeys.Input.RightArrow, m_signal_right_Arrow );
                addSignal( GamePadKeys.Input.LeftArrow, m_signal_left_Arrow );
                addSignal( GamePadKeys.Input.UpArrow, m_signal_up_Arrow );
                addSignal( GamePadKeys.Input.DownArrow, m_signal_down_Arrow );
            }


            public override MetaType Type()
            {
                if( m_type == null )
                {
                    m_type = new MetaType( KeyFactory.Create( "GamepadInputManager" ) );
                }

                return m_type;
            }
        }
    }
}
