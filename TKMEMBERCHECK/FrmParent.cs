﻿using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TKMEMBERCHECK
{
    public partial class FrmParent : Form
    {
        SqlConnection conn;
        MenuStrip MnuStrip;
        ToolStripMenuItem MnuStripItem;

        public FrmParent()
        {
            InitializeComponent();
        }

        //private void InitializeComponent()
        //{
        //    throw new NotImplementedException();
        //}

        private void FrmParent_Load(object sender, EventArgs e)
        {
            // To make this Form the Parent Form
            this.IsMdiContainer = true;

            //Creating object of MenuStrip class
            MnuStrip = new MenuStrip();

            //Placing the control to the Form
            this.Controls.Add(MnuStrip);

            String connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(connectionString);
            String Sequel = "SELECT MAINMNU,MENUPARVAL,STATUS FROM MNU_PARENT";
            SqlDataAdapter da = new SqlDataAdapter(Sequel, conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                MnuStripItem = new ToolStripMenuItem(dr["MAINMNU"].ToString());
                SubMenu(MnuStripItem, dr["MENUPARVAL"].ToString());
                MnuStrip.Items.Add(MnuStripItem);
            }
            // The Form.MainMenuStrip property determines the merge target.
            this.MainMenuStrip = MnuStrip;
        }


        public void SubMenu(ToolStripMenuItem mnu, string submenu)
        {
            String Seqchild = "SELECT FRM_NAME FROM MNU_SUBMENU WHERE MENUPARVAL='" + submenu + "'";
            SqlDataAdapter dachildmnu = new SqlDataAdapter(Seqchild, conn);
            DataTable dtchild = new DataTable();
            dachildmnu.Fill(dtchild);

            foreach (DataRow dr in dtchild.Rows)
            {
                ToolStripMenuItem SSMenu = new ToolStripMenuItem(dr["FRM_NAME"].ToString(), null, new EventHandler(ChildClick));
                mnu.DropDownItems.Add(SSMenu);
            }
        }

        private void ChildClick(object sender, EventArgs e)
        {
            // MessageBox.Show(string.Concat("You have Clicked ", sender.ToString(), " Menu"), "Menu Items Event",MessageBoxButtons.OK, MessageBoxIcon.Information);

            String Seqtx = "SELECT FRM_CODE FROM MNU_SUBMENU WHERE FRM_NAME='" + sender.ToString() + "'";
            SqlDataAdapter datransaction = new SqlDataAdapter(Seqtx, conn);
            DataTable dtransaction = new DataTable();
            datransaction.Fill(dtransaction);

            Assembly frmAssembly = Assembly.LoadFile(Application.ExecutablePath);
            foreach (Type type in frmAssembly.GetTypes())
            {
                //MessageBox.Show(type.Name);
                if (type.BaseType == typeof(Form))
                {
                    if (type.Name == dtransaction.Rows[0][0].ToString())
                    {
                        Form frmShow = (Form)frmAssembly.CreateInstance(type.ToString());
                        // then when you want to close all of them simple call the below code

                        foreach (Form form in this.MdiChildren)
                        {
                            form.Close();
                        }

                        frmShow.MdiParent = this;
                        frmShow.WindowState = FormWindowState.Maximized;
                        //frmShow.ControlBox = false;
                        frmShow.Show();
                    }
                }
            }
        }
    }
}