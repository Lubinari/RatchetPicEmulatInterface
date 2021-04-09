using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace Academia
{
    public partial class Form1 : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        string strSQL;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button_open.Enabled = true;
            button_close.Enabled = false;
            button_enviar.Enabled = false;
            verticalProgressBar_statusCOM.Value = 0;
            comboBox_BaudRat.Text = "9600";

        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                strSQL = "INSERT INTO cad_cliente (ID, NOME, SENHA,STATUS) VALUES (@ID, @NOME, @SENHA,@STATUS)";

                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@ID", txtId.Text);
                comando.Parameters.AddWithValue("@NOME", txtNome.Text);
                comando.Parameters.AddWithValue("@SENHA", txtSenha.Text);
                comando.Parameters.AddWithValue("@STATUS", txtStatus.Text);

                conexao.Open();

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtId.Clear();
                txtNome.Clear();
                txtSenha.Clear();
                txtStatus.Clear();

                strSQL = "SELECT * FROM cad_cliente";

                da = new MySqlDataAdapter(strSQL, conexao);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvDados.DataSource = dt;

                conexao.Close();
                conexao = null;
                comando = null;

            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                strSQL = "UPDATE cad_cliente SET NOME = @NOME, SENHA = @SENHA, STATUS = @STATUS WHERE ID = @ID";

                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@ID", txtId.Text);
                comando.Parameters.AddWithValue("@NOME", txtNome.Text);
                comando.Parameters.AddWithValue("@SENHA", txtSenha.Text);
                comando.Parameters.AddWithValue("@STATUS", txtStatus.Text);


                conexao.Open();

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
                strSQL = "SELECT * FROM cad_cliente";

                da = new MySqlDataAdapter(strSQL, conexao);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvDados.DataSource = dt;

                conexao.Close();
                conexao = null;
                comando = null;

                txtId.Clear();
                txtNome.Clear();
                txtSenha.Clear();
                txtStatus.Clear();


            }
        }

        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                strSQL = "DELETE FROM cad_cliente WHERE ID = @ID";

                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@ID", txtId.Text);

                conexao.Open();

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtId.Clear();
                txtNome.Clear();
                txtSenha.Clear();
                txtStatus.Clear();

                strSQL = "SELECT * FROM cad_cliente";

                da = new MySqlDataAdapter(strSQL, conexao);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvDados.DataSource = dt;

                conexao.Close();
                conexao = null;
                comando = null;
            }
        }

        private void buttonConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                strSQL = "SELECT * FROM cad_cliente WHERE ID = @ID";

                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@ID", txtId.Text);

                conexao.Open();

                dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    txtNome.Text = Convert.ToString(dr["nome"]);
                    txtSenha.Text = Convert.ToString(dr["senha"]);
                    txtStatus.Text = Convert.ToString(dr["status"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
                conexao = null;
                comando = null;
            }
        }

        private void buttonExibir_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                strSQL = "SELECT * FROM cad_cliente";

                da = new MySqlDataAdapter(strSQL, conexao);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvDados.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
                conexao = null;
                comando = null;
            }
        }

        private void buttonLimpar_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtNome.Clear();
            txtSenha.Clear();
            txtStatus.Clear();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox_COMPort_DropDown(object sender, EventArgs e)
        {
            string[] portLists = SerialPort.GetPortNames();
            comboBox_COMPort.Items.Clear();
            comboBox_COMPort.Items.AddRange(portLists);
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox_COMPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox_BaudRat.Text);
                serialPort1.Open();

                button_open.Enabled = false;
                button_close.Enabled = true;
                button_enviar.Enabled = true;
                verticalProgressBar_statusCOM.Value = 100;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();

                button_open.Enabled = true;
                button_close.Enabled = false;
                button_enviar.Enabled = false;
                verticalProgressBar_statusCOM.Value = 0;

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                serialPort1.Close();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button_enviar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                strSQL = "SELECT * FROM cad_cliente";

                comando = new MySqlCommand(strSQL, conexao);
                conexao.Open();
                dr = comando.ExecuteReader();

                char[] dados = new char[6];

                serialPort1.Write("\r");
                Thread.Sleep(500);

                while (dr.Read())
                {
                    dados[0] = Convert.ToChar(dr["ID"]);
                    dados[1] = Convert.ToChar(dr["status"]);

                    for (int i = 2; i < 6; i++)
                    {
                        dados[i] = Convert.ToString(dr["senha"]).ToCharArray()[i - 2];
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        serialPort1.Write(dados, i, 1);
                        Thread.Sleep(100);
                    }
                }

                serialPort1.Write("\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
                conexao = null;
                comando = null;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string user_ID_del = serialPort1.ReadTo("\r");
            if (user_ID_del.Length > 0)
            {
                char[] ID_del = user_ID_del.ToCharArray();


                foreach (char ID in ID_del)
                {

                    try
                    {
                        conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                        strSQL = "DELETE FROM cad_cliente WHERE ID = @ID";

                        comando = new MySqlCommand(strSQL, conexao);
                        comando.Parameters.AddWithValue("@ID", Convert.ToInt16(ID));

                        conexao.Open();

                        comando.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conexao.Close();
                        conexao = null;
                        comando = null;
                    }
                }
            }


            string dados = serialPort1.ReadTo("\n");
            string user;
            while (dados.Length > 0)
            {
                user = dados.Substring(0, 6);
                dados = dados.Remove(0, 6);
                int user_ID = Convert.ToInt16(user[0]);
                int user_status = Convert.ToInt16(user[1]);
                int user_pass = Convert.ToInt16(user.Substring(2));


                try
                {
                    conexao = new MySqlConnection("Server=localhost;Database=cad_cliente;Uid=root;Pwd=1234;");

                    strSQL = "INSERT IGNORE INTO cad_cliente (ID, STATUS, NOME, SENHA) VALUES (@ID, @STATUS, @NOME, @SENHA)";

                    comando = new MySqlCommand(strSQL, conexao);
                    comando.Parameters.AddWithValue("@ID", user_ID);
                    comando.Parameters.AddWithValue("@STATUS", user_status);
                    comando.Parameters.AddWithValue("@NOME", "ADD nome");
                    comando.Parameters.AddWithValue("@SENHA", user_pass);

                    conexao.Open();

                    comando.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conexao.Close();
                    conexao = null;
                    comando = null;
                }


            }


        }
    }
}