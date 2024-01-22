
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async Task SaveFile()
        {
            string conStr = @"Data Source = STHQ012E-09; Database=FilesDB; TrustServerCertificate=true; Integrated Security = false; User Id = admin; Password = admin;";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.CommandText = @"INSERT INTO Images VALUES(@FileName, @ImageBin)";
                command.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, 50);

                string filename = filePath.Text;

                string fileCoreName = "page.png";
                byte[] ImageBytes;
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    ImageBytes = new byte[fs.Length];
                    fs.Read(ImageBytes, 0, ImageBytes.Length);
                    command.Parameters.Add("@ImageBin", System.Data.SqlDbType.Image, Convert.ToInt32(fs.Length));
                }
                command.Parameters["@FileName"].Value = fileCoreName;
                command.Parameters["@ImageBin"].Value = ImageBytes;
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();
                MessageBox.Show("Successfull operation");
            }

        }

        private  async Task ReadFileFromDBAsync()
        {
            string conStr = @"Data Source = STHQ012E-09; Database=FilesDB; TrustServerCertificate=true; Integrated Security = false; User Id = admin; Password = admin;";

            List<Image> imageList = new List<Image>();
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                string sqlQuery = "SELECT * FROM Images";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string fileName = reader.GetString(1);
                        byte[] ImageBytes = (byte[])reader.GetValue(2);

                        Image img = new Image(id, fileName, ImageBytes);
                        imageList.Add(img);
                        MessageBox.Show("Successfull operation");

                    }
                }
            }

            if (imageList.Count > 0)
            {
                using (FileStream fileStream = new FileStream(imageList[0].FileName, FileMode.OpenOrCreate))
                {
                    fileStream.Write(imageList[0].ImageBytes, 0, imageList[0].ImageBytes.Length);
                    
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SaveFile();
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            filePath.Text = string.Empty;
        }

        private async void Button1_Click (object sender, RoutedEventArgs e)
        {
            await ReadFileFromDBAsync();
            
        }
        
    }
}
