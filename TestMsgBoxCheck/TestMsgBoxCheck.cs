using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;	// For RegKey

namespace TestMsgBoxCheck
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class TestMsgBoxCheckForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnClear;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestMsgBoxCheckForm()
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
				if (components != null) 
				{
					components.Dispose();
				}
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
			this.btnTest = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(8, 96);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(128, 23);
			this.btnTest.TabIndex = 0;
			this.btnTest.Text = "Test MsgBoxCheck";
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 80);
			this.label1.TabIndex = 1;
			this.label1.Text = "Press the button below to test the MsgBoxCheck class.  If you check the \"Don\'t sh" +
				"ow me this again\" box pressing the button below will have no affect, unless you " +
				"use the other button to clear the registry variable.";
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(144, 96);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(120, 23);
			this.btnClear.TabIndex = 2;
			this.btnClear.Text = "Clear Registry";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// TestMsgBoxCheckForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 126);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnTest);
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
			MsgBoxCheck.MessageBox dlg = new MsgBoxCheck.MessageBox();
			DialogResult dr = dlg.Show(@"Software\PricklySoft\TestMsgBoxCheck","DontShowAgain",DialogResult.OK,"Don't ask me this again","Now is the time for all good men to check this message box", "Hello",
				MessageBoxButtons.OK, MessageBoxIcon.Information); 
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"Software\PricklySoft\TestMsgBoxCheck");
			regKey.SetValue("DontShowAgain",false);
		}
	}
}
