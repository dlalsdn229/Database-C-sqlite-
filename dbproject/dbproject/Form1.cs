using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace dbproject
{
    public partial class Form1 : Form
    {
        String admin_id = "admin";
        String admin_pw = "1234";
        int login_flag = 0;//1:관리자 2:학생  3: 교수
        int table_flag = 0; //1.학생 2.교수 3.교과목 4.학생_신청 5.학과

        OleDbConnection conn;
        string connectionString;
        String db_id = "S20154090";
        String db_pw = "1234";

        String key_name = "";
        string dep_name = "";

        public Form1()
        {
            InitializeComponent();
            connectionString = "Provider=MSDAORA;Password=" + db_pw + ";User ID=" + db_id;//oracle 서버 연결
        }

        //로그인이 필요합니다.
        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        //login btn
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView6.Rows.Clear();
            dataGridView7.Rows.Clear();
            //updatedb(dataGridView6, "select * from std where stdno = " + id.Text, 8);
            //updatedb(dataGridView7, "select * from prof where prof_num = " + id.Text, 3);

            //connectionString = "Provider=MSDAORA;Password=" + db_pw + ";User ID=" + db_id;//oracle 서버 연결
            //관리자 로그인
            if (login_flag == 1 && id.Text.Equals(admin_id) && pw.Text.Equals(admin_pw))
            {
                label1.Text = "관리자님 어서오세요.";
                login_flag = 1;
                //연결 스트링에 대한 정보 
                //Oracle - MSDAORA
                //conn = new OleDbConnection(connectionString);
                //conn.Open(); //데이터베이스 연결
                //모든 정보 열람가능
                updatedb(dataGridView1, "select * from std", 8);
                updatedb(dataGridView2, "select * from prof", 3);
                updatedb(dataGridView3, "select * from cls", 7);
                updatedb(dataGridView4, "select * from dep", 2);
                updatedb(dataGridView5, "select * from std_res", 4);

                //관리자가 사용가능한 버튼 visible
                loginbtn.Visible = false;
                logoutbtn.Visible = true;
                //관리자 패널
                panel1.Visible = true;
                panel3.Visible = true;


            }
            //학생
            
            else if (login_flag == 2 && pw.Text.Equals("1234") && dataGridView6.Rows[0].Cells[0].FormattedValue.ToString().Equals(id.Text))
            {
                updatedb(dataGridView6, "select * from std where stdno = " + id.Text, 8);
                updatedb(dataGridView7, "select * from prof where prof_num = " + id.Text, 3);


                label1.Text = dataGridView6.Rows[0].Cells[1].FormattedValue.ToString() + " 님 어서오세요.";
                dep_name = "'" + dataGridView6.Rows[0].Cells[4].FormattedValue.ToString() + "'";
                textBox1.Text = id.Text;
                //패널 visible 설정
                loginbtn.Visible = false;
                logoutbtn.Visible = true;
                panel2.Visible = true;

                updatedb(dataGridView1, "select * from std where stdno = " + textBox1.Text, 8);
                updatedb(dataGridView2, "select * from prof where dep_name=" + dep_name, 3);
                updatedb(dataGridView3, "select * from cls where dep_name=" + dep_name, 7);
                updatedb(dataGridView5, "select * from std_res where stdno = " + textBox1.Text, 4);

                

                
            }   
            //교수
            else if (login_flag == 3 && pw.Text.Equals("1234") && dataGridView7.Rows[0].Cells[0].FormattedValue.ToString().Equals(id.Text))
            {
                updatedb(dataGridView6, "select * from std where stdno = " + id.Text, 8);
                updatedb(dataGridView7, "select * from prof where prof_num = " + id.Text, 3);

                label1.Text = dataGridView7.Rows[0].Cells[1].FormattedValue.ToString() + " 교수님 어서오세요.";
                dep_name ="'"+ dataGridView7.Rows[0].Cells[2].FormattedValue.ToString()+"'";


                //패널 visible 설정
                loginbtn.Visible = false;
                logoutbtn.Visible = true;
                panel4.Visible = true;                
                
                updatedb(dataGridView1, "select * from std where dep_name="+dep_name, 8);
                updatedb(dataGridView2, "select * from prof where dep_name="+dep_name, 3);
                updatedb(dataGridView3, "select * from cls where dep_name="+dep_name, 7);
                updatedb(dataGridView5, "select * from std_res r,std s where r.stdno=s.stdno and s.dep_name="+dep_name, 4);

            }
           

        }       
       
        private void updatedb(DataGridView dgv,String command,int col)
        {
            conn = new OleDbConnection(connectionString);
           
            try
            {
                conn.Open(); //데이터베이스 연결
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = command; //member 테이블
                cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
                cmd.Connection = conn;

                OleDbDataReader read = cmd.ExecuteReader(); //select * from emp 결과
                //dataGridView1.ColumnCount = 9;
                dgv.ColumnCount = col;
                //필드명 받아오는 반복문
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    dgv.Columns[i].Name = read.GetName(i);
                }

                //행 단위로 반복
                while (read.Read())
                {
                    object[] obj = new object[dgv.ColumnCount]; // 필드수만큼 오브젝트 배열

                    for (int i = 0; i < dgv.ColumnCount; i++) // 필드 수만큼 반복
                    {
                        obj[i] = new object();
                        obj[i] = read.GetValue(i); // 오브젝트배열에 데이터 저장
                    }

                    dgv.Rows.Add(obj); //데이터그리드뷰에 오브젝트 배열 추가
                }

                read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
            }
        }

        //id
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pw_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                login_flag = 1;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                login_flag = 2;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                login_flag = 3;
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //삭제
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox10.Text != "")
            {
                if (table_flag == 1)//학생
                {
                     dataGridView1.Rows.Clear();
                     manudb("delete from std where " + key_name + "=" + textBox10.Text);
                     updatedb(dataGridView1, "select * from std", 8);                  

                }
                else if (table_flag == 2)//교수
                {
                    dataGridView2.Rows.Clear();
                    manudb("delete from prof where " + key_name + "=" + textBox10.Text);
                    updatedb(dataGridView2, "select * from prof", 3);
                }
                else if (table_flag == 3)//교과목
                {
                    dataGridView3.Rows.Clear();                    
                    manudb("delete from cls where " + key_name + "=" + textBox10.Text);
                    updatedb(dataGridView3, "select * from cls", 7);

                }
                else if (table_flag == 4)//학생신청
                {
                    dataGridView5.Rows.Clear();
                    manudb("delete from std_res where " + key_name + "=" + textBox10.Text);
                    updatedb(dataGridView5, "select * from std_res", 4);
                }
                else if (table_flag == 5)//학과
                {
                    dataGridView4.Rows.Clear();
                    manudb("delete from dep where " + key_name + "=" + "'" + textBox10.Text+ "'" );
                    updatedb(dataGridView4, "select * from dep", 2);
                }
            }
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            id.Text = "";
            pw.Text = "";
            label1.Text = "로그인이 필요합니다.";
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            dataGridView3.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
            dataGridView4.Columns.Clear();
            dataGridView4.Rows.Clear();
            dataGridView4.Refresh();
            dataGridView5.Columns.Clear();
            dataGridView5.Rows.Clear();
            dataGridView5.Refresh();
            logoutbtn.Visible = false;
            loginbtn.Visible = true;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Text.Equals("학생"))
            {
                table_flag = 1;
                profpanel.Visible = false;
                clspanel.Visible = false;
                std_respanel.Visible = false;
                depanel.Visible = false;
                stdpanel.Visible = true;
            }
            else if (comboBox2.Text.Equals("교수"))
            {
                table_flag = 2;
                profpanel.Visible = true;
                clspanel.Visible = false;
                std_respanel.Visible = false;
                depanel.Visible = false;
                stdpanel.Visible = false;
            }
            else if (comboBox2.Text.Equals("교과목"))
            {
                table_flag = 3;
                profpanel.Visible = false;
                clspanel.Visible = true;
                std_respanel.Visible = false;
                depanel.Visible = false;
                stdpanel.Visible = false;
            }
            else if (comboBox2.Text.Equals("학생_신청"))
            {
                table_flag = 4;
                profpanel.Visible = false;
                clspanel.Visible = false;
                std_respanel.Visible = true;
                depanel.Visible = false;
                stdpanel.Visible = false;
            }
            else if (comboBox2.Text.Equals("학과"))
            {
                table_flag = 5;
                profpanel.Visible = false;
                clspanel.Visible = false;
                std_respanel.Visible = false;
                depanel.Visible = true;
                stdpanel.Visible = false;
            }
        }

        //검색
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        //교과목 개설
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();

            manudb("INSERT INTO cls VALUES(" + textBox3.Text + ",'" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "',"
                    + textBox7.Text + "," + textBox8.Text + ",'" + textBox9.Text + "')");
            updatedb(dataGridView3, "select * from cls", 7);
            
            //초기화
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
        }
           
        //키값설정
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox3.SelectedIndex == 0)
            {
                key_name = "stdno";
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                key_name = "DEP_NAME";
            }
            else if (comboBox3.SelectedIndex == 2)
            {
                key_name = "prof_num";
            }
            else if (comboBox3.SelectedIndex == 3)
            {
                key_name = "clsno";
            }
          
        }
        private void manudb(String command)
        {
            conn = new OleDbConnection(connectionString);
            try
            {
                conn.Open(); //데이터베이스 연결
                OleDbCommand cmd = new OleDbCommand();

                cmd.CommandText = command;

                cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
                cmd.Connection = conn;

                int col = cmd.ExecuteNonQuery(); //쿼리문을 실행하고 영향받는 행의 수를 반환.
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
            }
            /**
            finally
            {
                if (conn != null)
                {
                    conn.Close(); //데이터베이스 연결 해제
                }
            }**/
        }
        //입력
        private void insert_Click(object sender, EventArgs e)
        {
            //conn = new OleDbConnection(connectionString);
            if (table_flag == 1)//학생
            {
                dataGridView1.Rows.Clear();
                manudb("INSERT INTO std VALUES(" + textBox11.Text + ",'" + textBox12.Text + "'," + textBox13.Text + ",'"
                        + textBox14.Text + "','" + textBox15.Text + "','" + textBox16.Text + "','" + textBox17.Text + "','" + textBox18.Text + "')");
                updatedb(dataGridView1, "select * from std", 8);
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox16.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
            }
            else if(table_flag == 2)//교수
            {
                dataGridView2.Rows.Clear();
                manudb("INSERT INTO prof VALUES(" + textBox21.Text + ",'" + textBox22.Text + "','" + textBox23.Text + "')");
                updatedb(dataGridView2, "select * from prof", 3);
                textBox21.Text = "";
                textBox22.Text = "";
                textBox23.Text = "";

            }
            else if (table_flag == 3)//교과목
            {
                dataGridView3.Rows.Clear();

                manudb("INSERT INTO cls VALUES(" + textBox3.Text + ",'" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "',"
                        + textBox7.Text + "," + textBox8.Text + ",'" + textBox9.Text + "')");
                updatedb(dataGridView3, "select * from cls", 7);
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";

            }
            else if (table_flag == 4)//학생신청
            {
                dataGridView5.Rows.Clear();
                manudb("INSERT INTO std_res VALUES(" + textBox24.Text + "," + textBox25.Text + ",'" + textBox26.Text + "','"
                        + textBox27.Text + "')");
                updatedb(dataGridView5, "select * from std_res",4);
                textBox24.Text = "";
                textBox25.Text = "";
                textBox26.Text = "";
                textBox27.Text = "";
            }
            else if (table_flag == 5)//학과
            {
                dataGridView4.Rows.Clear();
                manudb("INSERT INTO dep VALUES('" + textBox19.Text + "','" + textBox20.Text + "')");
                updatedb(dataGridView4, "select * from dep",2);
                textBox19.Text = "";
                textBox20.Text = "";
            }


        }

        //수정
        private void update_Click(object sender, EventArgs e)
        {
            string attr="";
            string change = "";
            
            if (table_flag == 1)//학생
            {
                if (textBox11.Text != "") { attr = "stdno"; change = textBox11.Text;
                }
                else if(textBox12.Text != "") { attr = "std_name"; change = "'" + textBox12.Text + "'";                                        
                }
                else if(textBox13.Text != "") { attr = "grade"; change = textBox13.Text;
                }
                else if(textBox14.Text != "") { attr = "phone"; change = textBox14.Text;
                }
                else if(textBox15.Text != "") { attr = "dep_name"; change = "'" + textBox15.Text + "'";
                }
                else if(textBox16.Text != "") { attr = "prof_num"; change = textBox16.Text;
                }
                else if(textBox17.Text != "") { attr = "coun_contents"; change = "'" + textBox17.Text + "'";
                }
                else if(textBox18.Text != "") { attr = "coun_date"; change = "'" + textBox18.Text + "'";
                }

                dataGridView1.Rows.Clear();                
                manudb("update std set " + attr + "=" + change + " where " + key_name + "=" + textBox10.Text);                                             
                updatedb(dataGridView1, "select * from std", 8);

            }
            else if (table_flag == 2)//교수
            {
                if (textBox21.Text != "") { attr = "prof_num"; change = textBox21.Text; }
                else if (textBox22.Text != "") { attr = "prof_name"; change = "'" + textBox22.Text + "'";  }
                else if (textBox23.Text != "") { attr = "dep_name"; change = "'" + textBox23.Text + "'"; }                

                dataGridView2.Rows.Clear();
                manudb("update prof set " + attr + "=" + change + " where " + key_name + "=" + textBox10.Text);
                updatedb(dataGridView2, "select * from prof", 3);
                textBox21.Text = "";
                textBox22.Text = "";
                textBox23.Text = "";

            }
            else if (table_flag == 3)//교과목
            {
                if (textBox3.Text != "") { attr = "clsno"; change = textBox3.Text; }
                else if (textBox4.Text != "") { attr = "dep_name"; change = "'" + textBox4.Text + "'"; }
                else if (textBox5.Text != "") { attr = "cls_name"; change = "'" + textBox5.Text + "'"; }
                else if (textBox6.Text != "") { attr = "cls_div"; change = "'" + textBox6.Text + "'"; }
                else if (textBox7.Text != "") { attr = "cls_grade"; change = textBox7.Text; }
                else if (textBox8.Text != "") { attr = "prof_num"; change = textBox8.Text; }
                else if (textBox9.Text != "") { attr = "open"; change = "'" + textBox9.Text + "'"; }

                dataGridView3.Rows.Clear();
                manudb("update cls set " + attr + "=" + change + " where " + key_name + "=" + textBox10.Text);
                updatedb(dataGridView3, "select * from cls", 7);
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";

            }
            else if (table_flag == 4)//학생신청
            {
                if (textBox24.Text != "") { attr = "stdno"; change = textBox24.Text; }
                else if (textBox25.Text != "") { attr = "clsno"; change = textBox25.Text; }
                else if (textBox26.Text != "") { attr = "state"; change = "'" + textBox26.Text + "'"; }
                else if (textBox27.Text != "") { attr = "score"; change = "'" + textBox27.Text + "'"; }
                
                dataGridView5.Rows.Clear();
                manudb("update std_res set " + attr + "=" + change + " where " + key_name + "=" + textBox10.Text);
                updatedb(dataGridView5, "select * from std_res", 4);
                textBox24.Text = "";
                textBox25.Text = "";
                textBox26.Text = "";
                textBox27.Text = "";               
            }
            else if (table_flag == 5)//학과
            {
                if (textBox19.Text != "") { attr = "dep_name"; change = "'" + textBox24.Text + "'"; }
                else if (textBox20.Text != "") { attr = "dep_office"; change = "'" + textBox25.Text + "'";}                

                dataGridView4.Rows.Clear();
                manudb("update dep set " + attr + "=" + change + " where " + key_name + "=" + textBox10.Text);
                updatedb(dataGridView4, "select * from dep", 2);
                textBox19.Text = "";
                textBox20.Text = "";
            }
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {

        }

        //수강신청
        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
            manudb("INSERT INTO std_res VALUES(" + textBox1.Text + "," + textBox2.Text + ",'" + "승인중.." + "','" + "NULL"+ "')");
            updatedb(dataGridView5,"select * from std_res where stdno =" + id.Text ,4);
        }

        //성적조회
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
            if (textBox2.Text != "")//선택조회
            {
                updatedb(dataGridView5, "select * from std_res where stdno =" + textBox1.Text + " and clsno = "+ textBox2.Text, 4);
            }
            else
            {
                updatedb(dataGridView5, "select * from std_res where stdno =" + id.Text, 4);
            }
            
            
        }

        //승인하기
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
            manudb("update std_res set state='승인완료' where stdno = " + textBox28.Text + " and clsno = " +textBox29.Text);
            updatedb(dataGridView5, "select * from std_res", 4);
        }

        //상담조회
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            updatedb(dataGridView1, "select * from std where stdno =" + id.Text, 8);
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {

        }

        //수강명부 조회
        private void button6_Click_1(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
            if (textBox30.Text != "")
            {
                updatedb(dataGridView5, "select * from std_res where clsno =" + textBox30.Text, 4);
            }
            else
            {
                updatedb(dataGridView5, "select * from std_res", 4);
            }
        }

        //성적입력
        private void button7_Click_1(object sender, EventArgs e)
        {
            string score = "'" + textBox33.Text+ "'";
            dataGridView5.Rows.Clear();
            manudb("update std_res set score=" + score + "where stdno = " + textBox31.Text + " and clsno = " + textBox32.Text);
            //updatedb(dataGridView5, "select * from std_res", 4);
            updatedb(dataGridView5, "select * from std_res r,std s where r.stdno=s.stdno and s.dep_name=" + dep_name, 4);
        }

        //상담입력
        private void button8_Click(object sender, EventArgs e)
        {
            string contents = "'" + textBox34.Text + "'";
            string date = "'" + textBox35.Text + "'";
            dataGridView1.Rows.Clear();
            manudb("update std set coun_contents=" + contents + "where stdno = " + textBox31.Text);
            manudb("update std set coun_date=" + date + "where stdno = " + textBox31.Text);
            updatedb(dataGridView1, "select * from std", 8);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private string stringdb(String command)
        {
            conn = new OleDbConnection(connectionString);
            string s="";
            try
            {
                conn.Open(); //데이터베이스 연결
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = command; //member 테이블
                cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
                cmd.Connection = conn;

                
                OleDbDataReader read = cmd.ExecuteReader(); //select * from emp 결과

                
                //button9.Text = s;
                read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
            }
            return s;
        }


       
    }
}
