using System;
using System.Windows.Forms;
using Microsoft.Win32;	// For RegKey

namespace TestMsgBoxCheck
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class TestMsgBoxCheckForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button _btnTest;
		private System.Windows.Forms.Label _label1;
		private System.Windows.Forms.Button _btnClear;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private readonly System.ComponentModel.Container _components = null;

	    private TestMsgBoxCheckForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			    _components?.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._btnTest = new System.Windows.Forms.Button();
			this._label1 = new System.Windows.Forms.Label();
			this._btnClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnTest
			// 
			this._btnTest.Location = new System.Drawing.Point(8, 110);
			this._btnTest.Name = "_btnTest";
			this._btnTest.Size = new System.Drawing.Size(128, 23);
			this._btnTest.TabIndex = 0;
			this._btnTest.Text = "Test MsgBoxCheck";
			this._btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// label1
			// 
			this._label1.Location = new System.Drawing.Point(8, 8);
			this._label1.Name = "_label1";
			this._label1.Size = new System.Drawing.Size(264, 100);
			this._label1.TabIndex = 1;
			this._label1.Text = "This the version from GUY.  \n\nPress the button below to test the MsgBoxCheck class.  If you check the \"Don\'t sh" +
				"ow me this again\" box pressing the button below will have no affect, unless you " +
				"use the other button to clear the registry variable.";
			// 
			// btnClear
			// 
			this._btnClear.Location = new System.Drawing.Point(144, 110);
			this._btnClear.Name = "_btnClear";
			this._btnClear.Size = new System.Drawing.Size(120, 23);
			this._btnClear.TabIndex = 2;
			this._btnClear.Text = "Clear Registry";
			this._btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// TestMsgBoxCheckForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 150);
			this.Controls.Add(this._btnClear);
			this.Controls.Add(this._label1);
			this.Controls.Add(this._btnTest);
			this.Name = "TestMsgBoxCheckForm";
			this.Text = "TestMsgBoxCheck";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new TestMsgBoxCheckForm());
		}

		private void btnTest_Click(object sender, System.EventArgs e)
		{
			MessageBox dlg = new MessageBox();
			DialogResult dr = dlg.Show(@"Software\PricklySoft\TestMsgBoxCheck","DontShowAgain",DialogResult.OK,
                "Don't ask me this again","Now is the time for all good men to check this message box", "Hello",
				MessageBoxButtons.OK, MessageBoxIcon.Information); 
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
		    RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"Software\PricklySoft\TestMsgBoxCheck");
		    regKey?.SetValue("DontShowAgain", false);
		}
	}
}
