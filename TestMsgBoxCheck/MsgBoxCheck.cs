using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using MsdnMag;
// For LocalCbtHook

// For RegKey

namespace TestMsgBoxCheck
{
	public class MessageBox
	{
	    private readonly LocalCbtHook _mCbt;
	    private IntPtr _mHwnd = IntPtr.Zero;
	    private IntPtr _mHwndBtn = IntPtr.Zero;
	    private bool _mBInit = false;
	    private bool _mBCheck = false;
	    private string _mStrCheck;

		public MessageBox()
		{
			_mCbt = new LocalCbtHook();
			_mCbt.WindowCreated += new LocalCbtHook.CbtEventHandler(WndCreated);
			_mCbt.WindowDestroyed += new LocalCbtHook.CbtEventHandler(WndDestroyed);
			_mCbt.WindowActivated += new LocalCbtHook.CbtEventHandler(WndActivated);
		}

		public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText, string strTitle, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			RegistryKey regKey = Registry.CurrentUser.CreateSubKey(strKey);
			try
			{
				if(Convert.ToBoolean(regKey.GetValue(strValue,false)))
					return dr;
			}
			catch 
			{
				// No processing needed...the convert might throw an exception,
				// but if so we proceed as if the value was false.
			}

			_mStrCheck = strCheck;
			_mCbt.Install();
			dr = System.Windows.Forms.MessageBox.Show(strText, strTitle, buttons, icon);
			_mCbt.Uninstall();

			regKey.SetValue(strValue,_mBCheck);
			return dr;
		}

		public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText, string strTitle, MessageBoxButtons buttons)
		{
			return Show(strKey, strValue, dr, strCheck, strText, strTitle, buttons, MessageBoxIcon.None);
		}
		
		public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText, string strTitle)
		{
			return Show(strKey, strValue, dr, strCheck, strText, strTitle, MessageBoxButtons.OK, MessageBoxIcon.None);
		}
		
		public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText)
		{
			return Show(strKey, strValue, dr, strCheck, strText, "", MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		private void WndCreated(object sender, CbtEventArgs e)
		{
			if (e.IsDialogWindow)
			{
				_mBInit = false;
				_mHwnd = e.Handle;
			}
		}
	
		private void WndDestroyed(object sender, CbtEventArgs e)
		{
			if (e.Handle == _mHwnd)
			{
				_mBInit = false;
				_mHwnd = IntPtr.Zero;
				if(BstChecked == (int)SendMessage(_mHwndBtn,BmGetcheck,IntPtr.Zero,IntPtr.Zero))
					_mBCheck = true;
			}
		}

		private void WndActivated(object sender, CbtEventArgs e)
		{
			if (_mHwnd != e.Handle)
				return;

			// Not the first time
			if (_mBInit)
				return;
			else
				_mBInit = true;

			// Get the current font, either from the static text window
			// or the message box itself
			IntPtr hFont;
			IntPtr hwndText = GetDlgItem(_mHwnd, 0xFFFF);
			if(hwndText != IntPtr.Zero)
				hFont = SendMessage(hwndText, WmGetfont, IntPtr.Zero, IntPtr.Zero);			
			else
				hFont = SendMessage(_mHwnd, WmGetfont, IntPtr.Zero, IntPtr.Zero);			
			Font fCur = Font.FromHfont(hFont);
	
			// Get the x coordinate for the check box.  Align it with the icon if possible,
			// or one character height in
			int x = 0;
			IntPtr hwndIcon = GetDlgItem(_mHwnd, 0x0014);
			if(hwndIcon != IntPtr.Zero)
			{
				Rect rcIcon = new Rect();
				GetWindowRect(hwndIcon, rcIcon);
				Point pt = new Point();
				pt.x = rcIcon.left;
				pt.y = rcIcon.top;
				ScreenToClient(_mHwnd, pt);
				x = pt.x;
			}
			else
				x = (int)fCur.GetHeight();

			// Get the y coordinate for the check box, which is the bottom of the
			// current message box client area
			Rect rc = new Rect();
			GetClientRect(_mHwnd, rc);
			int y = rc.bottom - rc.top;

			// Resize the message box with room for the check box
			GetWindowRect(_mHwnd, rc);
			MoveWindow(_mHwnd,rc.left,rc.top,rc.right-rc.left,rc.bottom-rc.top + (int)fCur.GetHeight()*2,true);

			_mHwndBtn = CreateWindowEx(0, "button", _mStrCheck, BsAutocheckbox|WsChild|WsVisible|WsTabstop, 
				x, y , rc.right-rc.left-x, (int)fCur.GetHeight(),
				_mHwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

			SendMessage(_mHwndBtn, WmSetfont, hFont, new IntPtr(1));			
		}

		#region Win32 Imports
		private const int WsVisible		= 0x10000000;
		private const int WsChild			= 0x40000000;
		private const int WsTabstop        = 0x00010000;
		private const int WmSetfont		= 0x00000030;
		private const int WmGetfont		= 0x00000031;
		private const int BsAutocheckbox	= 0x00000003; 
		private const int BmGetcheck       = 0x00F0;
		private const int BstChecked       = 0x0001;

		[DllImport("user32.dll")]
		protected static extern void DestroyWindow(IntPtr hwnd);
	
		[DllImport("user32.dll")]
		private static extern IntPtr GetDlgItem(IntPtr hwnd, int id);
		
		[DllImport("user32.dll")]
		protected static extern int GetWindowRect(IntPtr hwnd, Rect rc);
		
		[DllImport("user32.dll")]
		protected static extern int GetClientRect(IntPtr hwnd, Rect rc);
		
		[DllImport("user32.dll")]
		private static extern void MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
		
		[DllImport("user32.dll")]
		protected static extern int ScreenToClient(IntPtr hwnd, Point pt);
		
		[DllImport("user32.dll", EntryPoint="MessageBox")]
		protected static extern int _MessageBox(IntPtr hwnd, string text, string caption,
			int options);
	
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hwnd, 
			int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern IntPtr CreateWindowEx(
			int dwExStyle,			// extended window style
			string lpClassName,		// registered class name
			string lpWindowName,	// window name
			int dwStyle,			// window style
			int x,					// horizontal position of window
			int y,					// vertical position of window
			int nWidth,				// window width
			int nHeight,			// window height
			IntPtr hWndParent,      // handle to parent or owner window
			IntPtr hMenu,			// menu handle or child identifier
			IntPtr hInstance,		// handle to application instance
			IntPtr lpParam			// window-creation data
			);
	
		[StructLayout(LayoutKind.Sequential)]
		protected class Point
		{ 
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		protected class Rect
		{ 
			public int left; 
			public int top; 
			public int right; 
			public int bottom; 
		}

		#endregion
	}
}
