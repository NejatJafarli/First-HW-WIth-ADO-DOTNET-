using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace AdoDotNet_HW_Books_And_Authors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SqlConnection Conn { get; set; }=new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringToDB"].ConnectionString);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TableCB.SelectedIndex= 1;
            CategoriesAndAuthors.SelectedIndex= 1;
        }

        private void TableCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableCB.SelectedIndex == 0)
            {
                AuthorsQuery();
            }
            else if (TableCB.SelectedIndex == 1)
            {
                CategoryQuery();
            }
        }
        public void CategoryQuery()
        {

            CategoriesAndAuthors.Items.Clear();
            SqlDataReader reader = null;

            try
            {
                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT DISTINCT Categories.Name AS 'Category' FROM Categories JOIN Books ON Books.Id_Category = Categories.Id", Conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CategoriesAndAuthors.Items.Add(reader[0].ToString());
                }
                CategoriesAndAuthors.SelectedIndex = 1;
            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }
        }
        public void AuthorsQuery()
        {

            CategoriesAndAuthors.Items.Clear();
            SqlDataReader reader = null;

            try
            {
                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT CONCAT(FirstName,LastName) AS 'Author' FROM Authors", Conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CategoriesAndAuthors.Items.Add(reader[0].ToString());
                }
                CategoriesAndAuthors.SelectedIndex = 1;

            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }

        }

        private void CategoriesAndAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // SELECT CONCAT(FirstName, LastName) AS 'Author', Books.Name AS 'BookName' FROM Authors JOIN Books ON Books.Id_Author = Authors.Id WHERE CONCAT(FirstName, LastName) = '{CatAut.SelectedItem}'
            if (CategoriesAndAuthors.SelectedItem is null) return;
            if (TableCB.SelectedIndex == 0)
            {
                FillDataGridForAuthors();
            }
            else if (TableCB.SelectedIndex == 1)
            {
                FillDataGridForCategory();
            }
        }

        private void FillDataGridForAuthors()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(Conn.ConnectionString))
            {
                var cmdString = $@"SELECT CONCAT(FirstName, LastName) AS 'Author', Books.Name AS 'BookName' FROM Authors JOIN Books ON Books.Id_Author = Authors.Id WHERE CONCAT(FirstName, LastName) = '{CategoriesAndAuthors.SelectedItem}'";

                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(CategoriesAndAuthors.SelectedItem.ToString());

                sqlDataAdapter.Fill(dataTable);

                DG.ItemsSource = dataTable.DefaultView;
            }
        }
        public void FillDataGridForCategory()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(Conn.ConnectionString))
            {
                var cmd = $@"SELECT Categories.Name AS 'Category', Books.Name AS 'Book' FROM Categories JOIN Books ON Books.Id_Category = Categories.Id WHERE Categories.Name = '{CategoriesAndAuthors.SelectedItem}'";
                SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(CategoriesAndAuthors.SelectedItem.ToString());
                sqlDataAdapter.Fill(dataTable);
                DG.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();

        }
    }
}
