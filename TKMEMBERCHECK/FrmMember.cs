﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TKMEMBERCHECK
{
    public partial class FrmMember : Form
    {

        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComm = new SqlCommand();
        string connectionString;
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbSqlQuery = new StringBuilder();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder();
        SqlTransaction tran;
        SqlCommand cmd = new SqlCommand();
        DataSet dsMember = new DataSet();
        DataSet dsNewCardID = new DataSet();
        int result;
        string ADDORUPDATE=null;
        DataGridViewRow drMEMBER = new DataGridViewRow();

        public FrmMember()
        {
            InitializeComponent();
        }

        public class Member
        {
            public string ID { set; get; }
            public string Cname { set; get; }
            public string Mobile1 { set; get; }
            public string Telphone { set; get; }
            public string Email { set; get; }
            public string Address { set; get; }
            public string Sex { set; get; }
            public string Birthday { set; get; }
            public string NewCardID { set; get; }
            public string NewLevel { set; get; }
            public string IsUpdate { set; get; }
        }

        List<Member> list_Member = new List<Member>();



        #region FUNTION
        public void Search()
        {
            try
            {

                if (!string.IsNullOrEmpty(textBox1.Text.ToString()) || !string.IsNullOrEmpty(textBox5.Text.ToString()))
                {
                    connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                    sqlConn = new SqlConnection(connectionString);

                    sbSql.Clear();
                    sbSqlQuery.Clear();

                    if (!string.IsNullOrEmpty(textBox1.Text.ToString()))
                    {
                        if (string.IsNullOrEmpty(sbSqlQuery.ToString()))
                        {
                            sbSqlQuery.AppendFormat(" OldCardID LIKE '%{0}%' ", textBox1.Text.ToString());
                        }
                        else
                        {
                            sbSqlQuery.AppendFormat(" AND OldCardID LIKE '%{0}%' ", textBox1.Text.ToString());
                        }

                    }
                    if (!string.IsNullOrEmpty(textBox5.Text.ToString()))
                    {
                        if (string.IsNullOrEmpty(sbSqlQuery.ToString()))
                        {
                            sbSqlQuery.AppendFormat("  Cname LIKE '%{0}%' ", textBox5.Text.ToString());
                        }
                        else
                        {
                            sbSqlQuery.AppendFormat(" AND  Cname LIKE '%{0}%' ", textBox5.Text.ToString());
                        }

                    }

                    sbSql.AppendFormat("SELECT TOP 10000 [ID],[Cname] AS '姓名',[Mobile1]  AS '手機',[Telphone]  AS '電話',[Email],[Address]  AS '住址',[Sex]  AS '性別',[Birthday]  AS '生日',[OldCardID]  AS '舊卡號',[OldLevel]  AS '舊會員等級' ,[NewCardID]  AS '新卡號' ,[NewLevel]  AS '新會員等級' FROM [TKMEMBER].[dbo].[Member] WHERE {0} ", sbSqlQuery.ToString());

                    adapter = new SqlDataAdapter(@"" + sbSql, sqlConn);
                    sqlCmdBuilder = new SqlCommandBuilder(adapter);

                    sqlConn.Open();
                    dsMember.Clear();
                    adapter.Fill(dsMember, "TEMPdsMember");
                    sqlConn.Close();
                    label1.Text = dsMember.Tables["TEMPdsMember"].Rows.ToString();

                    if (dsMember.Tables["TEMPdsMember"].Rows.Count == 0)
                    {
                        label1.Text = "0 find nothing";
                    }
                    else
                    {
                        label1.Text = dsMember.Tables["TEMPdsMember"].Rows.Count.ToString();

                        dataGridView1.DataSource = dsMember.Tables["TEMPdsMember"];
                    }
                }
                else
                {

                }



            }
            catch
            {

            }
            finally
            {

            }
        }

        public void MemberUpdate()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                sqlConn.Close();
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();

                sbSql.Clear();
                //sbSql.Append("UPDATE Member SET Cname='009999',Mobile1='009999',Telphone='',Email='',Address='',Sex='',Birthday='' WHERE ID='009999'");

                sbSql.AppendFormat("  UPDATE Member SET Cname='{1}',Mobile1='{2}',Telphone='{3}',Email='{4}',Address='{5}',Sex='{6}',Birthday='{7}',IsUpdate='{8}' WHERE ID='{0}' ", list_Member[0].ID.ToString(), list_Member[0].Cname.ToString(), list_Member[0].Mobile1.ToString(), list_Member[0].Telphone.ToString(), list_Member[0].Email.ToString(), list_Member[0].Address.ToString(), list_Member[0].Sex.ToString(), list_Member[0].Birthday.ToString(), list_Member[0].IsUpdate.ToString());
                //sbSql.AppendFormat("  UPDATE Member SET Cname='{1}',Mobile1='{2}' WHERE ID='{0}' ", list_Member[0].ID.ToString(), list_Member[0].Cname.ToString(), list_Member[0].Mobile1.ToString());

                cmd.Connection = sqlConn;
                cmd.CommandTimeout = 60;
                cmd.CommandText = sbSql.ToString();
                cmd.Transaction = tran;
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    tran.Rollback();    //交易取消
                }
                else
                {
                    tran.Commit();      //執行交易
                    Search();
                }
                sqlConn.Close();
            }
            catch
            {

            }

            finally
            {
                sqlConn.Close();
            }

        }

        public void MemberAdd()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                sqlConn.Close();
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();

                sbSql.Clear();
                //sbSql.Append("UPDATE Member SET Cname='009999',Mobile1='009999',Telphone='',Email='',Address='',Sex='',Birthday='' WHERE ID='009999'");

                sbSql.AppendFormat("  INSERT  INTO Member (ID, Cname, Mobile1, Telphone, Email, Address, Sex, Birthday, IsUpdate,OldCardID,OldLevel) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') ", list_Member[0].ID.ToString(), list_Member[0].Cname.ToString(), list_Member[0].Mobile1.ToString(), list_Member[0].Telphone.ToString(), list_Member[0].Email.ToString(), list_Member[0].Address.ToString(), list_Member[0].Sex.ToString(), list_Member[0].Birthday.ToString(), list_Member[0].IsUpdate.ToString(), list_Member[0].ID.ToString(),"銀卡");
                //sbSql.AppendFormat("  UPDATE Member SET Cname='{1}',Mobile1='{2}' WHERE ID='{0}' ", list_Member[0].ID.ToString(), list_Member[0].Cname.ToString(), list_Member[0].Mobile1.ToString());

                cmd.Connection = sqlConn;
                cmd.CommandTimeout = 60;
                cmd.CommandText = sbSql.ToString();
                cmd.Transaction = tran;
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    tran.Rollback();    //交易取消
                }
                else
                {
                    tran.Commit();      //執行交易
                    Search();
                }

                sqlConn.Close();

                connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                //顯示新增的會員
                sbSql.Clear();
                sbSqlQuery.Clear();
                sbSql.AppendFormat("SELECT [ID],[Cname] AS '姓名',[Mobile1]  AS '手機',[Telphone]  AS '電話',[Email],[Address]  AS '住址',[Sex]  AS '性別',[Birthday]  AS '生日',[OldCardID]  AS '舊卡號',[OldLevel]  AS '舊會員等級' ,[NewCardID]  AS '新卡號' ,[NewLevel]  AS '新會員等級' FROM [TKMEMBER].[dbo].[Member] WHERE ID='{0}' ", list_Member[0].ID.ToString());

                adapter = new SqlDataAdapter(@"" + sbSql, sqlConn);
                sqlCmdBuilder = new SqlCommandBuilder(adapter);

                sqlConn.Open();
                dsMember.Clear();
                adapter.Fill(dsMember, "TEMPdsMember");
                sqlConn.Close();
                label1.Text = dsMember.Tables["TEMPdsMember"].Rows.ToString();

                if (dsMember.Tables["TEMPdsMember"].Rows.Count == 0)
                {
                    label1.Text = "0 find nothing";
                }
                else
                {
                    label1.Text = dsMember.Tables["TEMPdsMember"].Rows.Count.ToString();

                    dataGridView1.DataSource = dsMember.Tables["TEMPdsMember"];
                }
            }
            catch
            {

            }

            finally
            {
                sqlConn.Close();
            }
        }

        #endregion

        #region GRIDVIEW

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dsMember.Tables["TEMPdsMember"].Rows.Count >= 1)
            {
                drMEMBER = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex];
                textBox2.Text = drMEMBER.Cells["姓名"].Value.ToString();
                textBox3.Text = drMEMBER.Cells["手機"].Value.ToString();
                textBox4.Text = drMEMBER.Cells["電話"].Value.ToString();
                textBox9.Text = drMEMBER.Cells["Email"].Value.ToString();
                textBox7.Text = drMEMBER.Cells["住址"].Value.ToString();
                textBox8.Text = drMEMBER.Cells["舊卡號"].Value.ToString();
                textBox19.Text = drMEMBER.Cells["舊會員等級"].Value.ToString();
                textBox17.Text = drMEMBER.Cells["新卡號"].Value.ToString();
                textBox20.Text = drMEMBER.Cells["新會員等級"].Value.ToString();

                if (!string.IsNullOrEmpty(drMEMBER.Cells["性別"].Value.ToString()) && drMEMBER.Cells["性別"].Value.ToString().Equals("女"))
                {
                    comboBox1.Text = "女";
                }
                else if (!string.IsNullOrEmpty(drMEMBER.Cells["性別"].Value.ToString()) && drMEMBER.Cells["性別"].Value.ToString().Equals("男"))
                {
                    comboBox1.Text = "男";
                }
                if (!string.IsNullOrEmpty(drMEMBER.Cells["生日"].Value.ToString()))
                {
                    dateTimePicker1.Value = Convert.ToDateTime(drMEMBER.Cells["生日"].Value.ToString());
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Now;
                }


            }
            else
            {
                textBox2.Text = "";
            }

        }
        public void SETADD()
        {
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox9.Text = null;
            textBox7.Text = null;
            textBox8.Text = null;
            textBox19.Text = null;
            textBox17.Text = null;
            textBox20.Text = null;
            dateTimePicker1.Value = DateTime.Now;
        }
        #endregion

        #region BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ADDORUPDATE = "UPDATE";
            button4.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ADDORUPDATE = "ADD";
            SETADD();
            button4.Visible = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(ADDORUPDATE!=null)
            {
                if (ADDORUPDATE.Equals("ADD"))
                {
                    if (!string.IsNullOrEmpty(textBox2.Text.ToString()) && !string.IsNullOrEmpty(textBox3.Text.ToString()) && !string.IsNullOrEmpty(textBox4.Text.ToString()) && !string.IsNullOrEmpty(textBox9.Text.ToString()) && !string.IsNullOrEmpty(textBox7.Text.ToString()) && !string.IsNullOrEmpty(comboBox1.Text.ToString()) && !string.IsNullOrEmpty(dateTimePicker1.Value.ToShortDateString()))
                    {
                        string newid;
                        int countid;
                        connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                        sqlConn = new SqlConnection(connectionString);
                        SqlCommand cmd = new SqlCommand(@"SELECT( CASE WHEN ISNULL(MAX(ID),'')='' THEN '0000000000' ELSE  MAX(ID)  END) AS ID  FROM Member WITH (NOLOCK) ", sqlConn);

                        sqlConn.Open();
                        adapter = new SqlDataAdapter(cmd);
                        dsNewCardID.Clear();
                        adapter.Fill(dsNewCardID);

                        newid = dsNewCardID.Tables[0].Rows[0][0].ToString();
                        countid = Convert.ToInt16(newid.Substring(3, 7));
                        countid = countid + 1;
                        newid = countid.ToString().PadLeft(10, '0');
                        list_Member.Clear();
                        list_Member.Add(new Member() { ID = newid.ToString(), Cname = textBox2.Text.ToString(), Mobile1 = textBox3.Text.ToString(), Telphone = textBox4.Text.ToString(), Email = textBox9.Text.ToString(), Address = textBox7.Text.ToString(), Sex = comboBox1.Text.ToString(), Birthday = dateTimePicker1.Value.ToShortDateString(), IsUpdate = "Y" });

                        MemberAdd();
                    }
                   
                }
                else if (ADDORUPDATE.Equals("UPDATE"))
                {
                    list_Member.Clear();
                    list_Member.Add(new Member() { ID = dataGridView1.CurrentRow.Cells[0].Value.ToString(), Cname = textBox2.Text.ToString(), Mobile1 = textBox3.Text.ToString(), Telphone = textBox4.Text.ToString(), Email = textBox9.Text.ToString(), Address = textBox7.Text.ToString(), Sex = comboBox1.Text.ToString(), Birthday = dateTimePicker1.Value.ToShortDateString(), IsUpdate = "Y" });

                    MemberUpdate();
                }

                ADDORUPDATE = null;
            }
            button4.Visible = false;

        }

        #endregion


    }
}
