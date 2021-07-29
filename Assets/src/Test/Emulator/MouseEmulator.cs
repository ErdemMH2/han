

namespace HAN.Lib.Test.Emulator
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using WindowsInput;

    /**
     * Windows mouse emulator.
     * By calling methods the windows mouse cursor can be moved and used.
     * This class will inject events into the event list of windows to emulate mouse behavior.
     */
    public static class MouseEmulator
    {
        /**
         * Native access to windows mouse states.
         * Used for reading the position.
         */
        private static class MouseNativeFunctions
        {
            [StructLayout( LayoutKind.Sequential )]
            public struct MousePoint
            {
                public int x;
                public int y;

                public MousePoint( int x, int y )
                {
                    this.x = x;
                    this.y = y;
                }
            }

            [Flags]
            public enum MouseEventFlags
            {
                LeftDown = 0x00000002,
                LeftUp = 0x00000004,
                MiddleDown = 0x00000020,
                MiddleUp = 0x00000040,
                Move = 0x00000001,
                Absolute = 0x00008000,
                RightDown = 0x00000008,
                RightUp = 0x00000010
            }

            [DllImport( "user32.dll", EntryPoint = "SetCursorPos" )]
            [return: MarshalAs( UnmanagedType.Bool )]
            private static extern bool SetCursorPos( int x, int y );

            [DllImport( "user32.dll" )]
            [return: MarshalAs( UnmanagedType.Bool )]
            private static extern bool GetCursorPos( out MousePoint lpMousePoint );

            [DllImport( "user32.dll" )]
            private static extern void mouse_event( int dwFlags, int dx, int dy, int dwData, int dwExtraInfo );


            public static void SetCursorPosition( int x, int y )
            {
                SetCursorPos( x, y );
            }


            public static void SetCursorPosition( MousePoint point )
            {
                SetCursorPos( point.x, point.y );
            }


            public static MousePoint GetCursorPosition()
            {
                MousePoint currentMousePoint;
                var gotPoint = GetCursorPos( out currentMousePoint );
                if( !gotPoint )
                {
                    currentMousePoint = new MousePoint( 0, 0 );
                }

                return currentMousePoint;
            }


            public static void Event( MouseEventFlags value )
            {
                MousePoint position = GetCursorPosition();

                mouse_event( (int) value,
                             position.x, position.y,
                             0, 0 );
            }
        }


        /// The position of the mouse pointer in unity coordinates.
        public static Vector3 PositionUnity { get { return Input.mousePosition; }  }

        /// The position in windows mouse coordinates
        public static Vector3 PositionReal { get { MouseNativeFunctions.MousePoint p = MouseNativeFunctions.GetCursorPosition(); return new Vector3( p.x, p.y ); }  }

        /// scale of real resolution to windows scaling dpi 
        public static Vector2 Scale { get; private set; }

        /// delta value betwenn window top left position and unity game window
        private static float m_delta { get; set; }

        /// IInputSimulator instance used for mouse events
        private static IInputSimulator m_mouseInputSimluator = new InputSimulator();

        /// Instance of IInputSimulator IMouseSimulator
        public static IMouseSimulator Instance { get { return m_mouseInputSimluator.Mouse; } }
        

        /**
         * Will calibrate the window and game window coordinates
         * to calculate the windows screen pixel position of the screen
         */
        public static IEnumerator Calibrate() 
        {
            // find virtual resolution by moving the cursor to most left  bottom screen
            MouseEmulator.MouseNativeFunctions.SetCursorPosition( Screen.currentResolution.width, Screen.currentResolution.height );
            yield return new WaitForEndOfFrame();
            var displayEnd = MouseEmulator.MouseNativeFunctions.GetCursorPosition();

            // windows will change resolution of screen and use dpi scaling to scale things up. 
            // calculate scale factor (windows scaling)
            Scale = new Vector2( (float) Screen.currentResolution.width   / (float) ( displayEnd.x + 1 ),
                                   (float) Screen.currentResolution.height  / (float) ( displayEnd.y + 1 ) ); 

            MouseEmulator.MouseNativeFunctions.SetCursorPosition( 0, 0 ); // move to zeto position to calculate delta between unity and windows mouse pos
            yield return new WaitForEndOfFrame();

            var realPos = MouseEmulator.MouseNativeFunctions.GetCursorPosition(); // get zero position (some tollerance)

            // this will calculate the delta value betwenn window top left position and unity game window top left position in pixel size
            // delta pixels of uniy window to top screen = unity mouse position y - windows cursor pos y - unity window screen height - 1 [index <> lenght] * windows scale factor
            m_delta = ( Input.mousePosition.y - realPos.y - (Screen.height - 1) ) * Scale.y;

            yield return new WaitForEndOfFrame();
        }
        
        /**
         * Converts y value from unity positions to windows screen positions
         */
        public static double RealY( double a_unitYPos )
        {
            // turns the bottom right coordinates to top left coordinates for unity position 
            // [index <> lenght] height - unity pos y [game window screen coordinates] * windows scale factor + delta pixels of uniy window
            return ( (Screen.height - 1) - a_unitYPos ) * Scale.y + m_delta;
        }


        /// Maximum pixel error when moving with mouse
        public static float PixelError()
        {
            return 10f;
        }


        public static IMouseSimulator HorizontalScroll( int scrollAmountInClicks )
        {
            return Instance.HorizontalScroll( scrollAmountInClicks );
        }


        public static IMouseSimulator LeftButtonClick()
        {
            return Instance.LeftButtonClick();
        }

        public static IEnumerator WaitForLeftButtonClick()
        {
            Instance.LeftButtonClick();
            yield return new WaitForFixedUpdate();
        }


        public static IMouseSimulator LeftButtonDoubleClick()
        {
            return Instance.LeftButtonDoubleClick();
        }


        public static IMouseSimulator LeftButtonDown()
        {
            return Instance.LeftButtonDown();
        }


        public static IEnumerator WaitForLeftButtonDown()
        {
            Instance.LeftButtonDown();
            yield return new WaitForFixedUpdate();
        }
         

        public static IMouseSimulator LeftButtonUp()
        {
            return Instance.LeftButtonUp();
        }


        public static IEnumerator WaitForLeftButtonUp()
        {
            Instance.LeftButtonUp();
            yield return new WaitForFixedUpdate();
        }


        /// Moves the cursor by a defined amount of unity (window scaled) pixels
        public static IMouseSimulator MoveBy( int pixelDeltaX, int pixelDeltaY )
        { 
            return MoveTo( PositionUnity.x + pixelDeltaX, PositionUnity.y + pixelDeltaY );
        }


        /// Moves the cursor by a defined amount of unity (window scaled) pixels
        public static IMouseSimulator MoveBy( Vector3 a_pixelDelta )
        {
            return MoveTo( PositionUnity + a_pixelDelta );
        }


        /// Coroutine: Waits for ending of moving the cursor by a defined amount of unity (window scaled) pixels
        public static IEnumerator WaitForMoveBy( int pixelDeltaX, int pixelDeltaY )
        {
            MoveTo( PositionUnity.x + pixelDeltaX, PositionUnity.y + pixelDeltaY );
            yield return new WaitForFixedUpdate();
        }

        /// move on whole displays coordinates (multi screen)
        public static IMouseSimulator MoveToPositionOnVirtualDesktop( double absoluteX, double absoluteY )
        {
            return Instance.MoveMouseToPositionOnVirtualDesktop( ( absoluteX * Scale.x ), RealY( absoluteY ) );
        }


        /// Moves the cursor on a absolute screen position
        public static IMouseSimulator MoveTo( double absoluteX, double absoluteY )
        {
            return Instance.MoveMouseTo( ( (int) ( absoluteX * Scale.x ) * 65536 ) / Screen.currentResolution.width, ( (int) RealY( absoluteY ) * 65536 ) / Screen.currentResolution.height );
        }

        /// Moves the cursor on a absolute screen position
        public static IMouseSimulator MoveTo( Vector3 a_absolute )
        {
            return MoveTo( a_absolute.x, a_absolute.y );
        }

        /// Coroutine: Waits for ending of moving the cursor on a absolute screen position
        public static IEnumerator WaitForMoveTo( Vector3 a_absolute )
        {
            MoveTo( a_absolute.x, a_absolute.y );
            yield return new WaitForFixedUpdate();
        }


        /// Moves the cursor to the game object
        public static IMouseSimulator MoveToObject( GameObject a_object )
        {
            return MoveTo( a_object.transform.position );
        }


        /// Coroutine: Waits for ending of moving the cursor to the game object
        public static IEnumerator WaitForMoveToObject( GameObject a_object )
        {
            MoveTo( a_object.transform.position );
            yield return new WaitForFixedUpdate();
        }


        public static IMouseSimulator RightButtonClick()
        {
            return Instance.RightButtonClick();
        }


        public static IMouseSimulator RightButtonDoubleClick()
        {
            return Instance.RightButtonDoubleClick();
        }


        public static IMouseSimulator RightButtonDown()
        {
            return Instance.RightButtonDown();
        }


        public static IMouseSimulator RightButtonUp()
        {
            return Instance.RightButtonUp();
        }


        public static IMouseSimulator Sleep( int millsecondsTimeout )
        {
            return Instance.Sleep( millsecondsTimeout );
        }


        public static IMouseSimulator Sleep( TimeSpan timeout )
        {
            return Instance.Sleep( timeout );
        }


        public static IMouseSimulator VerticalScroll( int scrollAmountInClicks )
        {
            return Instance.VerticalScroll( scrollAmountInClicks );
        }


        public static IMouseSimulator XButtonClick( int buttonId )
        {
            return Instance.XButtonClick( buttonId );
        }


        public static IMouseSimulator XButtonDoubleClick( int buttonId )
        {
            return Instance.XButtonDoubleClick( buttonId );
        }


        public static IMouseSimulator XButtonDown( int buttonId )
        {
            return Instance.XButtonDown( buttonId );
        }


        public static IMouseSimulator XButtonUp( int buttonId )
        {
            return Instance.XButtonUp( buttonId );
        }
    }
}