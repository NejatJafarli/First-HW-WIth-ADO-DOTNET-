using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
using System.Windows.Shapes;

namespace AdoDotNet_HW_Books_And_Authors
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {

        public SqlConnection Conn { get; set; } = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringToDB"].ConnectionString);
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(AddWindow));



        public string TextName
        {
            get { return (string)GetValue(TextNameProperty); }
            set { SetValue(TextNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextNameProperty =
            DependencyProperty.Register("TextName", typeof(string), typeof(AddWindow));



        public int Pages
        {
            get { return (int)GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PagesProperty =
            DependencyProperty.Register("Pages", typeof(int), typeof(AddWindow));



        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Year.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(AddWindow));




        public string Comment
        {
            get { return (string)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Comment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentProperty =
            DependencyProperty.Register("Comment", typeof(string), typeof(AddWindow));



        public int Quanity
        {
            get { return (int)GetValue(QuanityProperty); }
            set { SetValue(QuanityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Quanity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuanityProperty =
            DependencyProperty.Register("Quanity", typeof(int), typeof(AddWindow));




        public ObservableCollection<string> CBthemes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CBcategories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CBAuthors { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CBpress { get; set; } = new ObservableCollection<string>();



        public string ThemesSelection
        {
            get { return (string)GetValue(ThemesSelectionProperty); }
            set { SetValue(ThemesSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThemesSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThemesSelectionProperty =
            DependencyProperty.Register("ThemesSelection", typeof(string), typeof(AddWindow));


        public string CategoriesSelection
        {
            get { return (string)GetValue(CategoriesSelectionProperty); }
            set { SetValue(CategoriesSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoriesSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesSelectionProperty =
            DependencyProperty.Register("CategoriesSelection", typeof(string), typeof(AddWindow));



        public string AuthorsSelection
        {
            get { return (string)GetValue(AuthorsSelectionProperty); }
            set { SetValue(AuthorsSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AuthorsSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorsSelectionProperty =
            DependencyProperty.Register("AuthorsSelection", typeof(string), typeof(AddWindow));



        public string PressSelection
        {
            get { return (string)GetValue(PressSelectionProperty); }
            set { SetValue(PressSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressSelectionProperty =
            DependencyProperty.Register("PressSelection", typeof(string), typeof(AddWindow));





        public AddWindow()
        {
            InitializeComponent();
            DataContext = this;
            CategoryQuery();
            AuthorsQuery();
            ThemesQuery();
            PressQuery();
        }

        public void CategoryQuery()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT DISTINCT Categories.Name  FROM Categories", Conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CBcategories.Add(reader[0].ToString());
                }
            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }
        }
        public void AuthorsQuery()
        {

            SqlDataReader reader = null;

            try
            {
                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT CONCAT(FirstName,LastName) FROM Authors", Conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CBAuthors.Add(reader[0].ToString());
                }
            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }

        }
        public void ThemesQuery()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT DISTINCT Name FROM Themes", Conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CBthemes.Add(reader[0].ToString());
                }
            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }
        }
        public void PressQuery()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT DISTINCT Name FROM Press", Conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CBpress.Add(reader[0].ToString());
                }
            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            int PressId=0, CategoriesId=0, ThemesId=0, AuthorsId=0;

            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                Conn.Open();

                cmd = new SqlCommand($"SELECT Id FROM Press WHERE Press.[Name]='{PressSelection}'", Conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                    PressId = Convert.ToInt32(reader[0].ToString());

                reader?.Close();



                cmd = new SqlCommand($"SELECT Id FROM Themes WHERE Themes.Name='{ThemesSelection}'", Conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                    ThemesId = Convert.ToInt32(reader[0].ToString());


                reader?.Close();

                cmd = new SqlCommand($"SELECT Id FROM Categories WHERE Categories.Name='{CategoriesSelection}'", Conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                    CategoriesId = Convert.ToInt32(reader[0].ToString());

                reader?.Close();

                cmd = new SqlCommand($"SELECT Id FROM Authors WHERE CONCAT(Authors.FirstName,Authors.LastName)='{AuthorsSelection}'", Conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                    AuthorsId = Convert.ToInt32(reader[0].ToString());

                reader?.Close();

                string insertString = $"INSERT INTO Books (Id,[Name],Pages,YearPress,Id_Themes,Id_Category,Id_Author,Id_Press,Comment,Quantity) VALUES ({Id},'{TextName}',{Pages},{Year},{ThemesId},{CategoriesId},{AuthorsId},{PressId},'{Comment}',{Quanity})";

                cmd = new SqlCommand(insertString, Conn);
                cmd.ExecuteNonQuery();

            }
            finally
            {
                Conn?.Close();
                reader?.Close();
            }

            this.Close();
        }
    }
}
