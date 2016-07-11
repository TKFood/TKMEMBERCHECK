using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TKMEMBERCHECK
{
    public partial class FrmMemberCheck : Form
    {
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComm = new SqlCommand();
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbSqlQuery = new StringBuilder();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder();
        SqlTransaction tran;
        SqlCommand cmd = new SqlCommand();
        DataSet dsMember = new DataSet();
        int result;

        public FrmMemberCheck()
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
        }

        List<Member> list_Member = new List<Member>();

        #region FUNTION

        #endregion

        #region SERACH
        public void Search()
        {
            textBox8.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox19.Clear();
            textBox17.Clear();
            textBox9.Clear();
            textBox7.Clear();

            try
            {

                if (!string.IsNullOrEmpty(textBox1.Text.ToString()) || !string.IsNullOrEmpty(textBox5.Text.ToString()) || !string.IsNullOrEmpty(comboBox2.Text.ToString()))
                {
                    sqlConn = new SqlConnection("server=192.168.1.102;database=TKFOODDB;uid=sa;pwd=chi");
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
                    if (!string.IsNullOrEmpty(comboBox2.Text.ToString()))
                    {
                        if(comboBox2.Text.ToString().Equals("未核準"))
                        {
                            if (string.IsNullOrEmpty(sbSqlQuery.ToString()))
                            {
                                sbSqlQuery.Append("  ISNULL(NewCardID,'')='' ");
                            }
                            else
                            {
                                sbSqlQuery.Append(" AND  ISNULL(NewCardID,'')='' ");
                            }
                        }
                        else if (comboBox2.Text.ToString().Equals("已核準"))
                        {
                            if (string.IsNullOrEmpty(sbSqlQuery.ToString()))
                            {
                                sbSqlQuery.Append("  ISNULL(NewCardID,'')<>'' ");
                            }
                            else
                            {
                                sbSqlQuery.Append(" AND  ISNULL(NewCardID,'')<>'' ");
                            }
                        }
                        else if (comboBox2.Text.ToString().Equals("全部"))
                        {

                        }
                    }

                        sbSql.AppendFormat("SELECT TOP 10000 [ID],[Cname] AS '姓名',[Mobile1]  AS '手機',[Telphone]  AS '電話',[Email],[Address]  AS '住址',[Sex]  AS '性別',[Birthday]  AS '生日',[OldCardID]  AS '舊卡號',[OldLevel]  AS '舊會員等級' ,[NewCardID]  AS '新卡號' ,[NewLevel]  AS '新會員等級' FROM [TKFOODDB].[dbo].[Member] WHERE {0} ", sbSqlQuery.ToString());

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
                sqlConn = new SqlConnection("server=192.168.1.102;database=TKFOODDB;uid=sa;pwd=chi");

                sqlConn.Close();
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();

                sbSql.Clear();
                //sbSql.Append("UPDATE Member SET Cname='009999',Mobile1='009999',Telphone='',Email='',Address='',Sex='',Birthday='' WHERE ID='009999'");

                sbSql.AppendFormat("  UPDATE Member SET Cname='{1}',Mobile1='{2}',Telphone='{3}',Email='{4}',Address='{5}',Sex='{6}',Birthday='{7}',NewCardID='{8}',NewLevel='{9}' WHERE ID='{0}' ", list_Member[0].ID.ToString(), list_Member[0].Cname.ToString(), list_Member[0].Mobile1.ToString(), list_Member[0].Telphone.ToString(), list_Member[0].Email.ToString(), list_Member[0].Address.ToString(), list_Member[0].Sex.ToString(), list_Member[0].Birthday.ToString(), list_Member[0].NewCardID.ToString(), list_Member[0].NewLevel.ToString());
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

            }

        }

        #endregion

        #region GRIDVIEW

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dsMember.Tables["TEMPdsMember"].Rows.Count >= 1)
            {
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox9.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                textBox8.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                textBox19.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                textBox17.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                comboBox3.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();

                if (!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[6].Value.ToString()) && dataGridView1.CurrentRow.Cells[6].Value.ToString().Equals("女"))
                {
                    comboBox1.Text = "女";
                }
                else if (!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[6].Value.ToString()) && dataGridView1.CurrentRow.Cells[6].Value.ToString().Equals("男"))
                {
                    comboBox1.Text = "男";
                }
                if (!string.IsNullOrEmpty(dataGridView1.CurrentRow.Cells[7].Value.ToString()))
                {
                    dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[7].Value.ToString());
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

        #endregion

        #region BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            list_Member.Clear();
            list_Member.Add(new Member() { ID = dataGridView1.CurrentRow.Cells[0].Value.ToString(), Cname = textBox2.Text.ToString(), Mobile1 = textBox3.Text.ToString(), Telphone = textBox4.Text.ToString(), Email = textBox9.Text.ToString(), Address = textBox7.Text.ToString(), Sex = comboBox1.Text.ToString(), Birthday = dateTimePicker1.Value.ToShortDateString() , NewCardID = textBox17 .Text.ToString(),NewLevel=comboBox3.Text.ToString()});

            MemberUpdate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox17.Text = "111";
            comboBox3.Text = textBox19.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MemberUpdate();
        }
        #endregion


    }
}
