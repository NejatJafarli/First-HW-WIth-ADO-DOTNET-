using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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




        public bool IsUpdate { get; set; } = false;
        public AddWindow()
        {
            InitializeComponent();
            DataContext = this;
            CategoryQuery();
            AuthorsQuery();
            ThemesQuery();
            PressQuery();
            IsUpdate = false;
        }

        public AddWindow(string BookName)
        {
            InitializeComponent();
            DataContext = this;
            BookNameQuery(BookName);
            IsUpdate = true;
        }

        string BookName;
        string BookComment;

        string BookCat;
        string BookAut;
        string BookThem;
        string BookPress;

        int BookId;
        int BookYear;
        int BookQuantity;
        int BookPages;


        public void BookNameQuery(string name)
        {
            SqlDataReader reader = null;
            try
            {
                CategoryQuery();
                ThemesQuery();
                AuthorsQuery();
                PressQuery();

                Conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT * FROM Books Where @BookName = Books.Name", Conn);

                cmd.Parameters.Add("@BookName", System.Data.SqlDbType.NVarChar).Value = name;
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TextName = reader["Name"].ToString();
                    BookName = TextName;

                    Comment = reader["Comment"].ToString();
                    BookComment = Comment;

                    Id = int.Parse((reader["Id"].ToString()));
                    BookId = Id;

                    Quanity = int.Parse((reader["Quantity"].ToString()));
                    BookQuantity = Quanity;

                    Pages = int.Parse((reader["Pages"].ToString()));
                    BookPages = Pages;

                    Year = int.Parse((reader["YearPress"].ToString()));
                    BookYear = Year;

                    int idCat = int.Parse(reader["Id_Category"].ToString());
                    int idAut = int.Parse(reader["Id_Author"].ToString());
                    int idThem = int.Parse(reader["Id_Themes"].ToString());
                    int idPress = int.Parse(reader["Id_Press"].ToString());


                    Conn.Close();
                    Conn.Open();

                    SqlDataAdapter cmd2 = new SqlDataAdapter($"SELECT Name FROM Categories WHERE Id=@Id", Conn);
                    cmd2.SelectCommand.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = idCat;

                    DataTable dataTable = new DataTable();
                    cmd2.Fill(dataTable);
                    CategoriesSelection = dataTable.Rows[0][0].ToString();
                    BookCat = CategoriesSelection;

                    cmd2 = new SqlDataAdapter($"SELECT CONCAT(FirstName,LastName) FROM Authors WHERE Id=@Id", Conn);
                    cmd2.SelectCommand.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = idAut;

                    dataTable = new DataTable();
                    cmd2.Fill(dataTable);
                    AuthorsSelection = dataTable.Rows[0][0].ToString();
                    BookAut = AuthorsSelection;

                    cmd2 = new SqlDataAdapter($"SELECT Name FROM Themes WHERE Id=@Id", Conn);
                    cmd2.SelectCommand.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = idThem;

                    dataTable = new DataTable();
                    cmd2.Fill(dataTable);
                    ThemesSelection = dataTable.Rows[0][0].ToString();
                    BookThem = ThemesSelection;

                    cmd2 = new SqlDataAdapter($"SELECT Name FROM Press WHERE Id=@Id", Conn);
                    cmd2.SelectCommand.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = idPress;

                    dataTable = new DataTable();
                    cmd2.Fill(dataTable);
                    PressSelection = dataTable.Rows[0][0].ToString();
                    BookPress = PressSelection;


                }
            }
            finally
            {
                reader?.Close();
                Conn?.Close();
            }
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
            if (!IsUpdate)
            {
                int PressId = 0, CategoriesId = 0, ThemesId = 0, AuthorsId = 0;

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
            else
            {
                SqlCommand cmd = null;
                try
                {
                    if (Id != BookId)
                    {
                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET Id=@id Where Id=@bookId ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@bookId", SqlDbType.Int).Value = BookId;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookName != TextName)
                    {
                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET Name=@Name Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = TextName;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookComment != Comment)
                    {
                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET Comment=@Comment Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = Comment;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookPages != Pages)
                    {
                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET Pages=@Pages Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Pages", SqlDbType.Int).Value = Pages;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookYear != Year)
                    {
                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET YearPress=@Year Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Pages", SqlDbType.Int).Value = Year;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookQuantity != Quanity)
                    {
                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET Quantity=@Quanity Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@Quanity", SqlDbType.Int).Value = Quanity;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookCat != CategoriesSelection)
                    {
                        int NewCatId=0;
                        SqlDataReader reader = null;
                        try
                        {
                            Conn.Open();
                            cmd = new SqlCommand($"SELECT Id FROM Categories WHERE Categories.Name='{CategoriesSelection}'", Conn);
                            reader = cmd.ExecuteReader();

                            if (reader.Read())
                                NewCatId = Convert.ToInt32(reader[0].ToString());
                        }
                        finally
                        {
                            reader?.Close();
                            Conn?.Close();
                        }

                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET id_Category=@NewCatId Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@NewCatId", SqlDbType.Int).Value = NewCatId;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookAut != AuthorsSelection)
                    {
                        int NewAutId = 0;
                        SqlDataReader reader = null;
                        try
                        {
                            Conn.Open();
                            cmd = new SqlCommand($"SELECT Id FROM Authors WHERE CONCAT(Authors.FirstName,Authors.LastName)='{AuthorsSelection}'", Conn);
                            reader = cmd.ExecuteReader();

                            if (reader.Read())
                                NewAutId = Convert.ToInt32(reader[0].ToString());
                        }
                        finally
                        {
                            reader?.Close();
                            Conn?.Close();
                        }

                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET id_Author=@NewAutId Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@NewAutId", SqlDbType.Int).Value = NewAutId;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookThem!=ThemesSelection)
                    {
                        int NewThemId = 0;
                        SqlDataReader reader = null;
                        try
                        {
                            Conn.Open();
                            cmd = new SqlCommand($"SELECT Id FROM Themes WHERE Themes.Name='{ThemesSelection}'", Conn);
                            reader = cmd.ExecuteReader();

                            if (reader.Read())
                                NewThemId = Convert.ToInt32(reader[0].ToString());
                        }
                        finally
                        {
                            reader?.Close();
                            Conn?.Close();
                        }

                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET id_Themes=@NewThemId Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@NewThemId", SqlDbType.Int).Value = NewThemId;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    if (BookPress != PressSelection)
                    {
                        int NewPressId = 0;
                        SqlDataReader reader = null;
                        try
                        {
                            Conn.Open();
                            cmd = new SqlCommand($"SELECT Id FROM Press WHERE Press.[Name]='{PressSelection}'", Conn);
                            reader = cmd.ExecuteReader();

                            if (reader.Read())
                                NewPressId = Convert.ToInt32(reader[0].ToString());
                        }
                        finally
                        {
                            reader?.Close();
                            Conn?.Close();
                        }

                        Conn.Open();
                        cmd = new SqlCommand("UPDATE Books SET id_Press=@NewPressId Where Id=@id ", Conn);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@NewPressId", SqlDbType.Int).Value = NewPressId;
                        cmd.ExecuteNonQuery();
                        Conn?.Close();
                    }
                    this.Close();
                }
                finally
                {
                    Conn?.Close();
                }
            }
        }
    }
}
