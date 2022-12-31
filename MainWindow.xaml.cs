using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
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
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace WpfAndSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XMLConfigReader configReader;
        int currantPage = 0;
        string connetionStr;
        string[] strCommand;

        SqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            configReader = new XMLConfigReader("Settings.xml");
            connetionStr = configReader.ConnetionStr;
            strCommand = configReader.StrPageReadCommands;


            loadDataButton.IsEnabled = false;
            connection = new SqlConnection(connetionStr);

            connection.StateChange += (object o, StateChangeEventArgs e) =>
            {
                if (e.CurrentState == ConnectionState.Open)
                {
                    loadDataButton.IsEnabled = true;
                    infoTextBox.Text = "Connected";
                }
                else
                {
                    loadDataButton.IsEnabled = false;
                    infoTextBox.Text = "Disconnect";
                }
            };

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        private void TestConnectButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SecureString secureString = passwordTextBox.SecurePassword;
                secureString.MakeReadOnly();

                connection.Credential = new SqlCredential(
                    usernameTextBox.Text,
                    secureString);

                connection.Open();

            }
            catch (SqlException ex)
            {
                infoTextBox.Text = ex.Message;
            }

        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = SendSqlCommand(strCommand[currantPage]);
            
            dataGrid.ItemsSource = dataTable.AsDataView();

        }

        private DataTable SendSqlCommand(string strCommand)
        {
            DataTable dataTable = new DataTable();

            try
            {

                SqlCommand sqlCommand = new SqlCommand(strCommand, connection);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCommand;
                dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
                sqlAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                infoTextBox.Text = ex.Message;
            }

            return dataTable;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //statusLabel.Content = selectListBox.Items.IndexOf(e.Source);
           // if()
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.Source as RadioButton;
            statusLabel.Content = "Current page: " + radioButton.Content;
            currantPage = int.Parse((string)radioButton.Content) - 1;
        }


    }
}
