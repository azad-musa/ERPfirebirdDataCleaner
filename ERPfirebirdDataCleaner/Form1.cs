using System;
using System.Data;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace ERPfirebirdDataCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conn = "Database=C:\\WOLVOX.FDB;" + "User=SYSDBA;" + "Password=masterkey;" + "Dialect=3;" + "Server=localhost";
            try
            {
                using (FbConnection dbcon = new FbConnection(conn))
                {
                    dbcon.Open();
                    FbCommand cmd = new FbCommand("SELECT * FROM FATURA", dbcon);
                    FbDataAdapter adapter = new FbDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Set the DataSource of the existing DataGridView control
                    dataGridView1.DataSource = dataTable;

                    dbcon.Close();
                }
            }
            catch (FbException ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string conn = "Database=C:\\WOLVOX.FDB;" + "User=SYSDBA;" + "Password=masterkey;" + "Dialect=3;" + "Server=localhost";
            try
            {
                using (FbConnection dbcon = new FbConnection(conn))
                {
                    dbcon.Open();

                    
                    FbCommand deleteFaturaHrCmd = new FbCommand(
                        @"DELETE FROM FATURAHR
                  WHERE BLFTKODU IN (SELECT BLKODU FROM FATURA WHERE TRIM(EKBILGI_1) = '' OR EKBILGI_1 IS NULL)", dbcon);
                    int rowsAffectedFaturaHr = deleteFaturaHrCmd.ExecuteNonQuery();

                    FbCommand deleteCariHrCmd = new FbCommand(
                        @"DELETE FROM CARIHR
                  WHERE EVRAK_NO IN (SELECT FATURA_NO FROM FATURA WHERE TRIM(EKBILGI_1) = '' OR EKBILGI_1 IS NULL)", dbcon);
                    int rowsAffectedCariHr = deleteCariHrCmd.ExecuteNonQuery();

                    FbCommand deleteKasaHrCmd = new FbCommand(
                        @"DELETE FROM KASAHR
                  WHERE EVRAK_NO IN (SELECT FATURA_NO FROM FATURA WHERE TRIM(EKBILGI_1) = '' OR EKBILGI_1 IS NULL)", dbcon);
                    int rowsAffectedKasaHr = deleteKasaHrCmd.ExecuteNonQuery();


                    FbCommand deleteFaturaCmd = new FbCommand(
                        "DELETE FROM FATURA WHERE TRIM(EKBILGI_1) = '' OR EKBILGI_1 IS NULL", dbcon);
                    int rowsAffectedFatura = deleteFaturaCmd.ExecuteNonQuery();

                    MessageBox.Show(
                        $"{rowsAffectedFatura} rows deleted from FATURA table.\n{rowsAffectedFaturaHr} rows deleted from FATURAHR table.",
                        "Deletion Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    dbcon.Close();
                }
            }
            catch (FbException ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
