

namespace HAN.Lib.Test.Emulator 
{
    using WindowsInput;
    using System;
    using System.Collections.Generic;
    using WindowsInput.Native;

    /**
     * Windows keyboard emulator.
     * By calling methods the windows keyboard can generate key events and type text.
     */
    public class KeyboardEmulator
    {
        public static IKeyboardSimulator Instance { get { return MouseEmulator.Instance.Keyboard; } }

        public static IMouseSimulator Mouse => Instance.Mouse;

        public static IKeyboardSimulator KeyDown( VirtualKeyCode keyCode )
        {
            return Instance.KeyDown( keyCode );
        }

        public static IKeyboardSimulator KeyPress( VirtualKeyCode keyCode )
        {
            return Instance.KeyPress( keyCode );
        }

        public static IKeyboardSimulator KeyPress( params VirtualKeyCode[] keyCodes )
        {
            return Instance.KeyPress( keyCodes );
        }

        public static IKeyboardSimulator KeyUp( VirtualKeyCode keyCode )
        {
            return Instance.KeyUp( keyCode );
        }

        public static IKeyboardSimulator ModifiedKeyStroke( IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes )
        {
            return Instance.ModifiedKeyStroke( modifierKeyCodes, keyCodes );
        }

        public static IKeyboardSimulator ModifiedKeyStroke( IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode )
        {
            return Instance.ModifiedKeyStroke( modifierKeyCodes, keyCode );
        }

        public static IKeyboardSimulator ModifiedKeyStroke( VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes )
        {
            return Instance.ModifiedKeyStroke( modifierKey, keyCodes );
        }

        public static IKeyboardSimulator ModifiedKeyStroke( VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode )
        {
            return Instance.ModifiedKeyStroke( modifierKeyCode, keyCode );
        }

        public static IKeyboardSimulator Sleep( int millsecondsTimeout )
        {
            return Instance.Sleep( millsecondsTimeout );
        }

        public static IKeyboardSimulator Sleep( TimeSpan timeout )
        {
            return Instance.Sleep( timeout );
        }

        public static IKeyboardSimulator TextEntry( string text )
        {
            return Instance.TextEntry( text );
        }

        public static IKeyboardSimulator TextEntry( char character )
        {
            return Instance.TextEntry( character );
        }
    }
}