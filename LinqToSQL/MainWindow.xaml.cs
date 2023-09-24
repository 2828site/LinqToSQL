using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Configuration;
using LinqToSQL.testdbDataSetTableAdapters;

namespace LinqToSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.testdbConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            //InsertUniverstities();
            //InsertStudent();
            //InsertLectures();
            //InsertStudentLectureAssociations();
            //GetUniversityOfTonie();
            //GetLecturesFromTonie();
            //GetAllStudentsFromYale();
            //GetUniversitiesWithTransgenders();
            //GetAllLecturesFromBeijingTech();
            //UpdateTonie();
            DeleteJame();
        }

        public void InsertUniverstities()
        {

            dataContext.ExecuteCommand("delete from University");

            University yale = new University();
            yale.Name = "Yale";
            dataContext.Universities.InsertOnSubmit(yale);

            University beijingTech = new University();
            beijingTech.Name = "Beijing Tech";
            dataContext.Universities.InsertOnSubmit(beijingTech);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent()
        {
            University yale = dataContext.Universities.First(um => um.Name.Equals("Yale"));
            /* from university in dataContext.University where university == "yale" select university */
            University beijingTech = dataContext.Universities.First(um => um.Name.Equals("Beijing Tech"));

            List<Student> students = new List<Student>();

            students.Add(new Student { Name="Carla", Gender="female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Tonie", Gender = "male", University = yale });
            students.Add(new Student { Name = "Leyle", Gender = "female", University = beijingTech });
            students.Add(new Student { Name = "Jame", Gender = "trans-gender", University = beijingTech });

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Tonie = dataContext.Students.First(st => st.Name.Equals("Tonie"));
            Student Leyle = dataContext.Students.First(st => st.Name.Equals("Leyle"));
            Student Jame = dataContext.Students.First(st => st.Name.Equals("Jame"));

            Lecture Math = dataContext.Lectures.First(lc => lc.Name.Equals("Math"));
            Lecture History = dataContext.Lectures.First(lc => lc.Name.Equals("History"));
            
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Carla, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { StudentId = Tonie.Id, LectureId = Math.Id });

            StudentLecture slTonie = new StudentLecture();
            slTonie.Student = Tonie;
            slTonie.LectureId = History.Id;
            dataContext.StudentLectures.InsertOnSubmit(slTonie);

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Leyle, Lecture = History });

            dataContext.SubmitChanges();
            //MessageBox.Show(Tonie.Name);
            MainDataGrid.ItemsSource = dataContext.StudentLectures;
        }

        public void GetUniversityOfTonie()
        {
            Student Tonie = dataContext.Students.First(st => st.Name.Equals("Tonie"));
            University TonieUniversity = Tonie.University;

            List<University> universities = new List<University>();
            universities.Add(TonieUniversity);

            MainDataGrid.ItemsSource = universities;
        }

        public void GetLecturesFromTonie()
        {
            Student Tonie = dataContext.Students.First(st => st.Name.Equals("Tonie"));
            var tonieLectures = from sl in Tonie.StudentLectures select sl.Lecture;
            
            MainDataGrid.ItemsSource = tonieLectures;
        }

        public void GetAllStudentsFromYale()
        {
            var students = from student in dataContext.Students
                           where student.University.Name == "Yale"
                           select student;

            MainDataGrid.ItemsSource = students;
        }

        public void GetUniversitiesWithTransgenders()
        {
            var students = from student in dataContext.Students
                           join university in dataContext.Universities
                           on student.University equals university
                           where student.Gender == "trans-gender"
                           select university;

            MainDataGrid.ItemsSource = students;
        }

        public void GetAllLecturesFromBeijingTech()
        {
            var lecturesFromBeijingTech = from sl in dataContext.StudentLectures
                                           join student in dataContext.Students on sl.StudentId equals student.Id
                                           where student.University.Name == "Beijing Tech"
                                           select sl.Lecture;

            MainDataGrid.ItemsSource = lecturesFromBeijingTech;
        }

        public void UpdateTonie()
        {
            Student Tonie = dataContext.Students.FirstOrDefault(st => st.Name == "Tonie");

            Tonie.Name = "Antonio";

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteJame()
        {
            Student Jame = dataContext.Students.FirstOrDefault(st => st.Name == "Jame");
            dataContext.Students.DeleteOnSubmit(Jame);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }
    }
}
