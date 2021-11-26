using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdoDotNet_HW_Books_And_Authors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SqlConnection Conn { get; set; } = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringToDB"].ConnectionString);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TableCB.SelectedIndex = 1;
            CategoriesAndAuthors.SelectedIndex = 1;
        }

        private void TableCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableCB.SelectedIndex == 0)
            {
                AuthorsQuery();
                SearchText.IsEnabled = false;
            }
            else if (TableCB.SelectedIndex == 1)
            {
                CategoryQuery();
                SearchText.IsEnabled = false;
            }
            else if (TableCB.SelectedIndex == 2)
            {
                FillDataGrid();
                CategoriesAndAuthors.Items.Clear();
                SearchText.IsEnabled = true;
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
            if (CategoriesAndAuthors.SelectedItem is null) return;
            if (TableCB.SelectedIndex == 0)
            {
                FillDataGridForAuthors();
            }
            else if (TableCB.SelectedIndex == 1)
            {
                FillDataGridForCategory();
            }
            else
                return;
         
        }
        private void FillDataGrid()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(Conn.ConnectionString))
            {
                var cmdString = $@"SELECT * FROM Books ";

                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                DG.ItemsSource = dataTable.DefaultView;
            }
        }
        private void FillDataGridForAuthors()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(Conn.ConnectionString))
            {
                var cmdString = $@"SELECT CONCAT(FirstName, LastName) AS 'Author', Books.Name AS 'Book' FROM Authors JOIN Books ON Books.Id_Author = Authors.Id WHERE CONCAT(FirstName, LastName) = '{CategoriesAndAuthors.SelectedItem}'";

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

        private void DG_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (DG.SelectedItem is null) return;
                DataRowView row = (DataRowView)DG.SelectedItem;
                row.Delete();

                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE Books WHERE Name='{row["Book"]}'", Conn);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    Conn?.Close();
                }

            }
        }

        private void DG_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddWindow addWindow = new AddWindow(((DataRowView)DG.SelectedItem).Row.Table.Columns[1].ColumnName);
            addWindow.ShowDialog();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText.Text != "")
            {
                SqlConnection sqlConnection;
                using (sqlConnection = new SqlConnection(Conn.ConnectionString))
                {

                    ComboBoxItem cbi = (ComboBoxItem)TableCB.SelectedItem;


                    var cmdString = $"SELECT * FROM Books WHERE Name LIKE '%{SearchText.Text}%'";

                    SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    DataTable dataTable = new DataTable(cbi.Content.ToString());

                    sqlDataAdapter.Fill(dataTable);

                    DG.ItemsSource = dataTable.DefaultView;
                }
            }
            else if (SearchText.Text == "")
            {
                FillDataGrid();
            }
        }
    }
}
