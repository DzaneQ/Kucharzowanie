using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {

        public Window5()
        {
            InitializeComponent();
            LoadList(0, "");
            #region junk
            //MySqlConnection connection = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=kucharz;UID=root;PASSWORD:;");
            //MySqlCommand title_list = new MySqlCommand("select tytul from przepisy", connection);
            //connection.Open();
            //DataTable data_list = new DataTable();
            //data_list.Load(title_list.ExecuteReader());
            //connection.Close();
            //list.DataContext = data_list;
            #endregion junk
        }

        private void LoadList(int setting, string search)
        {
            recipeList.Items.Clear();            
            List<RecipeData> list_of_recipes = new List<RecipeData>();
            if (File.Exists(@"..\..\filebase\json.json"))
            {
                list_of_recipes = JsonConvert.DeserializeObject<List<RecipeData>>(File.ReadAllText(@"..\..\filebase\json.json"));
            }
            int element_index = 0;
            if (setting != 0 || (search != null && !TextBox_default))
            {
                List<RecipeData> filter_list = new List<RecipeData>();
                //filter_list = list_of_recipes;
                foreach (RecipeData element in list_of_recipes)
                {
                    if (setting == 0 || element.type[setting - 1] == 'v')
                    {
                        bool found = true;
                        if (search != null && !TextBox_default)
                        {
                            string[] word = search.ToLower().Split(' ');
                            for (int i = 0; i < word.Length; i++)
                            {
                                if (!element.title.ToLower().Contains(word[i]))
                                {
                                    found = false;
                                    if (element.tags != null)
                                    {
                                        if (element.tags.Contains(word[i]))
                                        {
                                            found = true;
                                        }
                                    }

                                }
                            }
                        }
                        if (search == null || TextBox_default) found = true;
                        if (found)
                        {
                            filter_list.Add(element);
                        }
                    }
                }
                //foreach (RecipeData element in list_of_recipes)
                //{
                //    if (setting != 0 && element.type[setting - 1] == 'x')
                //    {
                //        filter_list.RemoveAt(element_index);
                //        continue;
                //    }
                //    if (search != null && !TextBox_default)
                //    {
                //        string[] word = search.ToLower().Split(' ');
                //        for (int i = 0; i < word.Length; i++)
                //        {
                //            if (!element.title.ToLower().Contains(word[i]))
                //            {
                //                if (element.tags != null)
                //                {
                //                    if (element.tags.Contains(word[i]))
                //                    {
                //                        element_index++;
                //                        continue;
                //                    }
                //                }
                //                filter_list.RemoveAt(element_index);
                //                continue;
                //            }
                //        }
                //    }
                //    if (search == null || TextBox_default)
                //    {
                //        element_index++;
                //        continue;
                //    }
                //}

                foreach (RecipeData element in filter_list)
                {
                    recipeList.Items.Add(element.title);
                    element_index++;
                }
            }
            else
            {
                foreach (RecipeData element in list_of_recipes)
                {
                    recipeList.Items.Add(element.title);
                    element_index++;
                }
            }
            if (element_index == 0)
            {
                recipeList.Items.Add(noRecipes);
            }
            if (element_index > 1)
            {
                recipeList.Items.SortDescriptions.Add(
                            new System.ComponentModel.SortDescription("",
                            System.ComponentModel.ListSortDirection.Ascending));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 window = new Window2();
            Close();
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool isHelperOpen = false;
            foreach (Window opened_window in Application.Current.Windows)
            {
                if (opened_window is Window3)
                {
                    opened_window.Activate();
                    isHelperOpen = true;
                }
            }
            if (!isHelperOpen)
            {
                Window3 helper = new Window3();
                helper.Show();
            }
        }

        private void searcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (recipeList != null) LoadList(meal.SelectedIndex, searcher.Text);
        }

        private void searcher_KeyDown(object sender, KeyEventArgs e)
        {
            if (recipeList != null) LoadList(meal.SelectedIndex, searcher.Text);
        }

        bool TextBox_default = true;
        private void searcher_GotFocus(object sender, RoutedEventArgs e)
        {
            searcher.Background.Opacity = 0.75;
            if (TextBox_default)
            {
                searcher.Text = "";
                searcher.Foreground = Brushes.Black;
                TextBox_default = false;
            }
        }

        private void searcher_LostFocus(object sender, RoutedEventArgs e)
        {
            searcher.Background.Opacity = 0.65;
            if (searcher.Text == "")
            {
                searcher.Text = "Wyszukaj przepis lub składnik...";
                searcher.Foreground = Brushes.Gray;
                TextBox_default = true;
            }
        }

        private void meal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (recipeList != null) LoadList(meal.SelectedIndex, searcher.Text);
        }

        private void recipeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (recipeList.SelectedItem != noRecipes)
                {
                    string fileJson = @"..\..\filebase\json.json";
                    string fileImg = @"..\..\filebase\img\" + recipeList.SelectedItem.ToString() + ".img";
                    List<RecipeData> list_of_recipes = JsonConvert.DeserializeObject<List<RecipeData>>(File.ReadAllText(fileJson));
                    RecipeData data = null;
                    foreach (RecipeData element in list_of_recipes)
                    {
                        if (element.title == recipeList.SelectedItem.ToString())
                        {
                            data = element;
                            break;
                        }
                    }
                    if (data == null)
                    {
                        throw new Exception();
                    }
                    if (!File.Exists(fileImg))
                    {
                        throw new FileNotFoundException();
                    }
                    Window1 recipeWindow = new Window1();
                    recipeWindow.title.Text = data.title;
                    recipeWindow.recipe.Text = data.content;
                    recipeWindow.date.Text = "Dodano: " + data.date.ToShortDateString();
                    ImageBrush photo = new ImageBrush();
                    photo.ImageSource = new BitmapImage(new Uri(fileImg, UriKind.Relative));
                    photo.Opacity = 0.5;
                    recipeWindow.Background = photo;
                    recipeWindow.Show();
                }
            }
            catch (FileNotFoundException)
            {
                string fileJson = @"..\..\filebase\json.json";
                List<RecipeData> list_of_recipes = JsonConvert.DeserializeObject<List<RecipeData>>(File.ReadAllText(fileJson));
                RecipeData data = null;
                int data_index = 0;
                foreach (RecipeData element in list_of_recipes)
                {
                    if (element.title == recipeList.SelectedItem.ToString())
                    {
                        data = element;
                        break;
                    }
                    data_index++;
                }
                list_of_recipes.RemoveAt(data_index);
                File.WriteAllText(@"..\..\filebase\json.json", JsonConvert.SerializeObject(list_of_recipes));
                if (recipeList != null) LoadList(meal.SelectedIndex, searcher.Text);
                MessageBox.Show("Nie znaleziono pliku.");
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd wyświetlania przepisu.");
            }
        }
    }
}
