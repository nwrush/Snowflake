using System;

using MOIS;

using Snowflake.GuiComponents;

namespace Snowflake.Modules
{
	
	///<summary> 
	///MOIS manager                                                          
	///</summary>
	public class MoisManager
	{
		//////////////////////////////////////////////////////////////////////////
		private InputManager mInputMgr;
		private Keyboard mKeyboard;
		private Mouse mMouse;
		private bool[] mKeyDown;
		private bool[] mKeyPressed;
		private bool[] mMouseDown;
		private bool[] mMousePressed;
		private bool[] mMouseReleased;
		private Vector3 mMouseMove;
		private Vector3 mMousePos;
		private Vector3 mMousePressedPos;
		private Vector3 mMouseReleasedPos;

		// get last relative mouse movement (and wheel movement on Z axis) ///////
		public int MouseMoveX { get { return (int)mMouseMove.x; } }
		public int MouseMoveY { get { return (int)mMouseMove.y; } }
		public int MouseMoveZ { get { return (int)mMouseMove.z; } }

		// get absolute mouse position within window bounds //////////////////////
		public int MousePosX { get { return (int)mMousePos.x; } }
		public int MousePosY { get { return (int)mMousePos.y; } }

		// get last absolute mouse position when a mouse button was pressed //////
		public int MousePressedPosX { get { return (int)mMousePressedPos.x; } }
		public int MousePressedPosY { get { return (int)mMousePressedPos.y; } }

		// get last absolute mouse position when a mouse button was released /////
		public int MouseReleasedPosX { get { return (int)mMouseReleasedPos.x; } }
		public int MouseReleasedPosY { get { return (int)mMouseReleasedPos.y; } }

		//get Keyboard and Mouse instances
		public Keyboard Keyboard { get { return mKeyboard; } }
		public Mouse Mouse { get { return mMouse; } }

		
		///<summary> 
		///  constructor                                                           
		///</summary>
		internal MoisManager()
		{
			mInputMgr = null;
			mKeyboard = null;
			mMouse = null;
			mKeyDown = new bool[256];
			mKeyPressed = new bool[256];
			mMouseDown = new bool[8];
			mMousePressed = new bool[8];
			mMouseReleased = new bool[8];
			mMouseMove = new Vector3();
			mMousePos = new Vector3();
			mMousePressedPos = new Vector3();
			mMouseReleasedPos = new Vector3();
		}

		
		///<summary> 
		///  start up mois manager                                                 
		///</summary>
		internal bool Startup(IntPtr _windowHandle, int _width, int _height)
		{
			// check if already initialized
			if (mInputMgr != null)
				return false;

			// initialize input manager
			ParamList pl = new ParamList();
			pl.Insert("WINDOW", _windowHandle.ToString());
            try
            {
                mInputMgr = InputManager.CreateInputSystem(pl);
            }
            catch (AccessViolationException e)
            {

            }
			if (mInputMgr == null)
				return false;

			// initialize keyboard
			mKeyboard = (Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
			if (mKeyboard == null)
				return false;

			// set up keyboard event handlers
			mKeyboard.KeyPressed += OnKeyPressed;
			mKeyboard.KeyReleased += OnKeyReleased;

			// initialize mouse
			mMouse = (Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, true);
			if (mMouse == null)
				return false;

			// set up area for absolute mouse positions
			MouseState_NativePtr state = mMouse.MouseState;
			state.width = _width;
			state.height = _height;

			// set up mouse event handlers
			mMouse.MouseMoved += OnMouseMoved;
			mMouse.MousePressed += OnMousePressed;
			mMouse.MouseReleased += OnMouseReleased;

			Clear();

			// OK
			return true;
		}

		
		///<summary> 
		///  shut down mois manager                                                
		///</summary>
		internal void Shutdown()
		{
			// shutdown mouse
			if (mMouse != null)
			{
				mInputMgr.DestroyInputObject(mMouse);
				mMouse = null;
			}

			// shutdown keyboard
			if (mKeyboard != null)
			{
				mInputMgr.DestroyInputObject(mKeyboard);
				mKeyboard = null;
			}

			// shutdown input manager
			if (mInputMgr != null)
			{
				InputManager.DestroyInputSystem(mInputMgr);
				mInputMgr = null;
			}
		}

		
		///<summary> 
		///  update mois manager                                                   
		///</summary>
		
		internal void Update()
		{
			ClearKeyPressed();
			ClearMousePressed();
			ClearMouseMove();
			ClearMouseReleased();

			mKeyboard.Capture();
			mMouse.Capture();
		}

		
		///<summary> 
		///Clear all keyboard and mouse state except for absolute mouse pos      
		///</summary>
		public void Clear()
		{
			ClearKeyPressed();
			ClearKeyDown();
			ClearMousePressed();
			ClearMouseDown();
			ClearMouseMove();
			ClearMouseReleased();
		}

		
		///<summary> 
		///check if a key is currently pressed on the keyboard                   
		///</summary>
		public bool IsKeyDown(KeyCode _key)
		{
			return mKeyDown[(int)_key];
		}

		
		///<summary> 
		///  check if a key was pressed on the keyboard this frame                 
		///</summary>
		
		public bool WasKeyPressed(KeyCode _key)
		{
			return mKeyPressed[(int)_key];
		}

		
		///<summary> 
		///  check if a mouse button is currently pressed                          
		///</summary>
		
		public bool IsMouseButtonDown(MouseButtonID _button)
		{
			return mMouseDown[(int)_button];
		}

		
		///<summary> 
		///  check if a mouse button was pressed this frame                        
		///</summary>
		
		public bool WasMouseButtonPressed(MouseButtonID _button)
		{
			return mMousePressed[(int)_button];
		}

		public bool WasMouseButtonReleased(MouseButtonID _button) {
			return mMouseReleased[(int)_button];
		}

		
		///<summary> 
		///  check if the mouse was moved this frame (or wheel position changed)   
		///</summary>
		
		public bool WasMouseMoved()
		{
			return mMouseMove.x != 0 || mMouseMove.y != 0 || mMouseMove.z != 0;
		}

		//////////////////////////////////////////////////////////////////////////
		// internal functions ////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////

		
		///<summary> 
		///  clear the buffer for keys that were pressed                           
		///</summary>
		
		private void ClearKeyPressed()
		{
			for (int i = 0; i < mKeyPressed.Length; ++i)
				mKeyPressed[i] = false;
		}

		
		///<summary> 
		///  clear the buffer for keys that are pressed                            
		///</summary>
		
		private void ClearKeyDown()
		{
			for (int i = 0; i < mKeyDown.Length; ++i)
				mKeyDown[i] = false;
		}

		
		///<summary> 
		///  clear the buffer for mouse buttons that were pressed                  
		///</summary>
		
		private void ClearMousePressed()
		{
			for (int i = 0; i < mMousePressed.Length; ++i)
				mMousePressed[i] = false;
		}

		
		///<summary> 
		///  clear the buffer for mouse buttons that are pressed                   
		///</summary>
		
		private void ClearMouseDown()
		{
			for (int i = 0; i < mMouseDown.Length; ++i)
				mMouseDown[i] = false;
		}

		private void ClearMouseReleased() 
		{
			for (int i = 0; i < mMouseReleased.Length; ++i)
				mMouseReleased[i] = false;          
		}

		
		///<summary> 
		///  clear the relative mouse movement for this frame                      
		///</summary>
		
		private void ClearMouseMove()
		{
			mMouseMove.x = 0;
			mMouseMove.y = 0;
			mMouseMove.z = 0;
		}

		
		///<summary> 
		///  event handler for key presses                                         
		///</summary>
		
		private bool OnKeyPressed(KeyEvent arg)
		{
			mKeyDown[(int)arg.key] = true;
			mKeyPressed[(int)arg.key] = true;
			return true;
		}

		
		///<summary> 
		///  event handler for key releases                                        
		///</summary>
		
		private bool OnKeyReleased(KeyEvent arg)
		{
			mKeyDown[(int)arg.key] = false;
			return true;
		}

		
		///<summary> 
		///  event handler for mouse movement                                      
		///</summary>
		
		private bool OnMouseMoved(MouseEvent arg)
		{
			mMouseMove.x = arg.state.X.rel;
			mMouseMove.y = arg.state.Y.rel;
			mMouseMove.z = arg.state.Z.rel;
			mMousePos.x = arg.state.X.abs;
			mMousePos.y = arg.state.Y.abs;
			return true;
		}

		
		///<summary> 
		///  event handler for mouse button presses                                
		///</summary>
		
		private bool OnMousePressed(MouseEvent arg, MouseButtonID id)
		{
			mMouseDown[(int)id] = true;
			mMousePressed[(int)id] = true;
			mMousePos.x = arg.state.X.abs;
			mMousePos.y = arg.state.Y.abs;
			mMousePressedPos.x = arg.state.X.abs;
			mMousePressedPos.y = arg.state.Y.abs;
			return true;
		}

		
		///<summary> 
		///  event handler for mouse button releases                               
		///</summary>
		
		private bool OnMouseReleased(MouseEvent arg, MouseButtonID id)
		{
			mMouseDown[(int)id] = false;
			mMouseReleased[(int)id] = true;
			mMousePos.x = arg.state.X.abs;
			mMousePos.y = arg.state.Y.abs;
			mMouseReleasedPos.x = arg.state.X.abs;
			mMouseReleasedPos.y = arg.state.Y.abs;
			return true;
		}

	} // class

} // namespace
