using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        string file;
        public Window2()
        {
            InitializeComponent();
        }

        bool image_default = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Wybierz zdjęcie";
            dialog.Filter = "Wszystkie dostępne grafiki|*.jpg;*.jpeg;*.png|" +
        "Obrazy JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        "Grafika PNG (*.png)|*.png";
            if (dialog.ShowDialog() == true)
            {
                ImageBrush background = new ImageBrush();
                background.ImageSource = new BitmapImage(new Uri(dialog.FileName));
                file = dialog.FileName;
                if (background.ImageSource.Width < 650 || background.ImageSource.Height < 500)
                {
                    MessageBox.Show("Grafika odrzucona. Obrazek jest za mały. Wymagany rozmiar ma być przynajmniej 650x500 pikseli.");
                }
                else
                {
                    if (background.ImageSource.Width / background.ImageSource.Height < 1.3
                    || background.ImageSource.Width / background.ImageSource.Height > 2)
                    {
                        MessageBox.Show("Grafika odrzucona. Stosunek szerokości do wysokości obrazka nie mogą być mniejsze od 1,3 ani większe od 2.");
                    }
                    else
                    {
                        Background = background;
                        Background.Opacity = 0.5;
                        PrzyciskPodgladu.Visibility = Visibility.Visible;
                        image_default = false;
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window5 start = new Window5();
            Close();
            start.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool isHelperOpen = false;
            foreach (Window opened_window in Application.Current.Windows)
            {
                if (opened_window is Window4)
                {
                    opened_window.Activate();
                    isHelperOpen = true;
                }
            }
            if (!isHelperOpen)
            {
                Window4 helper = new Window4();
                helper.Show();
            }
        }

        bool tag_default = true;
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            tag.Background.Opacity = 0.75;
            if (tag_default)
            {                
                tag.Text = "";
                tag.Foreground = Brushes.Black;               
                tag_default = false;
            }
        }

        bool recipe_default = true;
        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            recipe.Background.Opacity = 0.75;
            if (recipe_default)
            {               
                recipe.Text = "";
                recipe.Foreground = Brushes.Black;               
                recipe_default = false;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            tag.Background.Opacity = 0.65;
            if (tag.Text == "")
            {
                tag.Text = "Składniki, sprzęt, itp.";
                tag.Foreground = Brushes.Gray;
                tag_default = true;
            }
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            recipe.Background.Opacity = 0.65;
            if (recipe.Text == "")
            {
                recipe.Text = "Przepis...";
                recipe.Foreground = Brushes.Gray;
                recipe_default = true;
            }
        }

        bool title_default = true;
        private void TextBox_GotFocus_2(object sender, RoutedEventArgs e)
        {
            title.Background.Opacity = 0.75;
            if (title_default)
            {
                title.Text = "";
                title.Foreground = Brushes.Black;
                title_default = false;
            }
        }

        private void TextBox_LostFocus_2(object sender, RoutedEventArgs e)
        {
            title.Background.Opacity = 0.65;
            if (title.Text == "")
            {
                title.Text = "Jak nazwiesz swój przepis?";
                title.Foreground = Brushes.Gray;
                title_default = true;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string titleChecker = title.Text;
            while (titleChecker.Length > 0)
            {
                if (titleChecker[0] == ' ')
                {
                    titleChecker = titleChecker.Substring(1);
                    continue;
                }
                if (titleChecker[titleChecker.Length - 1] == ' ')
                {
                    titleChecker = titleChecker.Substring(0, titleChecker.Length - 1);
                    continue;
                }
                while (titleChecker.IndexOf("  ") != -1)
                    titleChecker = titleChecker.Substring(0, titleChecker.IndexOf("  ")) + titleChecker.Substring(titleChecker.IndexOf("  ") + 2, titleChecker.Length - titleChecker.IndexOf("  ") - 2);
                break;
            }
            string contentChecker = recipe.Text;
            while (contentChecker.Length > 0)
            {
                if (contentChecker[0] == ' ')
                {
                    contentChecker = contentChecker.Substring(1);
                    continue;
                }
                break;
            }
            int letterCheckerTitle = titleChecker.Count(char.IsLetter);
            int letterCheckerTag = tag.Text.Count(char.IsLetter);
            foreach (char character in titleChecker)
            {
                if (character == ' ')
                {
                    letterCheckerTitle++;
                }
            }
            foreach (char character in tag.Text)
            {
                if (character == ' ')
                {
                    letterCheckerTag++;
                }
            }
            do
            {
                if ((recipe_default || contentChecker.Length == 0) && (title_default || titleChecker.Length == 0))
                {
                    MessageBox.Show("Wypełnij pola");
                    break;
                }
                if (recipe_default || contentChecker.Length == 0)
                {
                    MessageBox.Show("Przedstaw swój przepis");
                    break;
                }
                if (title_default || titleChecker.Length == 0)
                {
                    MessageBox.Show("Nazwij swój przepis");
                    break;
                }
                if (letterCheckerTitle != titleChecker.Length || (letterCheckerTag != tag.Text.Length && !tag_default))
                {
                    MessageBox.Show("Tytuł i tagi powinny jedynie zawierać litery i odstępy");
                    break;
                }
                if (image_default)
                {
                    MessageBox.Show("Dodaj zdjęcie");
                    break;
                }
                RecipeData added_data = new RecipeData();
                List<RecipeData> list_of_data = new List<RecipeData>();
                if (File.Exists(@"..\..\filebase\json.json"))
                {
                    list_of_data = JsonConvert.DeserializeObject<List<RecipeData>>(File.ReadAllText(@"..\..\filebase\json.json"));
                }
                titleChecker = titleChecker.First().ToString().ToUpper() + titleChecker.Substring(1);
                tag.Text = tag.Text.ToLower();
                added_data.title = titleChecker;
                string tagging = tag.Text;
                added_data.tags = tag.Text.Split(' ').ToList();
                if (tag_default)
                {
                    added_data.tags = null;
                }
                added_data.date = DateTime.Now;
                added_data.type = "xxxxx";
                if (breakfast.IsChecked.Value)
                {
                    added_data.type = "v"+ added_data.type.Substring(1, 4);
                }
                if (soup.IsChecked.Value)
                {
                    added_data.type = added_data.type.Substring(0, 1)+ "v" + added_data.type.Substring(2, 3);
                }
                if (dish.IsChecked.Value)
                {
                    added_data.type = added_data.type.Substring(0, 2) + "v" + added_data.type.Substring(3, 2);
                }
                if (dessert.IsChecked.Value)
                {
                    added_data.type = added_data.type.Substring(0, 3) + "v" + added_data.type.Substring(4, 1);
                }
                if (supper.IsChecked.Value)
                {
                    added_data.type = added_data.type.Substring(0, 4) + "v";
                }
                added_data.content = recipe.Text;
                try
                {
                    bool existing_title = false;
                    int title_index = 0;
                    foreach (RecipeData data_element in list_of_data)
                    {
                        if (data_element.title == added_data.title)
                        {
                            existing_title = true;
                            break;
                        }
                        title_index++;
                    }
                    if (existing_title)
                    {
                        var message = MessageBox.Show("Plik o takim tytule istnieje. Czy zastąpić?", "", MessageBoxButton.YesNo);
                        if (message == MessageBoxResult.Yes)
                        {
                            if (File.Exists(@"..\..\filebase\img\" + added_data.title + ".img"))
                            {
                                File.Delete(@"..\..\filebase\img\" + added_data.title + ".img");
                            }
                            File.Copy(file, @"..\..\filebase\img\" + added_data.title + ".img");
                            list_of_data.RemoveAt(title_index);
                            list_of_data.Add(added_data);
                            File.WriteAllText(@"..\..\filebase\json.json", JsonConvert.SerializeObject(list_of_data));
                            Window5 start_window = new Window5();
                            start_window.DodanoPrzepis.Visibility = Visibility.Visible;
                            start_window.DodanoPrzepis.Text = "Zmodyfikowano";
                            start_window.Show();
                            Close();
                        }
                    }
                    else
                    {
                        File.Copy(file, @"..\..\filebase\img\" + added_data.title + ".img");
                        list_of_data.Add(added_data);
                        File.WriteAllText(@"..\..\filebase\json.json", JsonConvert.SerializeObject(list_of_data));
                        Window5 start_window = new Window5();
                        start_window.DodanoPrzepis.Visibility = Visibility.Visible;
                        start_window.Show();
                        Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Wystąpił błąd podczas zapisywania danych.");
                }
        } while (false);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Hidden;
            Background.Opacity = 1;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Background.Opacity == 1)
            {
                grid.Visibility = Visibility.Visible;
                Background.Opacity = 0.5;
            }
        }

        #region CheckBoxColors
        private void breakfast_Checked(object sender, RoutedEventArgs e)
        {
            breakfast.Foreground = new SolidColorBrush(Color.FromArgb(255, 54, 149, 10));
        }

        private void soup_Checked(object sender, RoutedEventArgs e)
        {
            soup.Foreground = new SolidColorBrush(Color.FromArgb(255, 54, 149, 10));
        }

        private void supper_Checked(object sender, RoutedEventArgs e)
        {
            supper.Foreground = new SolidColorBrush(Color.FromArgb(255, 54, 149, 10));
        }

        private void dish_Checked(object sender, RoutedEventArgs e)
        {
            dish.Foreground = new SolidColorBrush(Color.FromArgb(255, 54, 149, 10));
        }

        private void dessert_Checked(object sender, RoutedEventArgs e)
        {
            dessert.Foreground = new SolidColorBrush(Color.FromArgb(255, 54, 149, 10));
        }

        private void breakfast_Unchecked(object sender, RoutedEventArgs e)
        {
            breakfast.Foreground = new SolidColorBrush(Color.FromArgb(255, 211, 134, 0));
        }

        private void soup_Unchecked(object sender, RoutedEventArgs e)
        {
            soup.Foreground = new SolidColorBrush(Color.FromArgb(255, 211, 134, 0));
        }

        private void supper_Unchecked(object sender, RoutedEventArgs e)
        {
            supper.Foreground = new SolidColorBrush(Color.FromArgb(255, 211, 134, 0));
        }

        private void dish_Unchecked(object sender, RoutedEventArgs e)
        {
            dish.Foreground = new SolidColorBrush(Color.FromArgb(255, 211, 134, 0));
        }

        private void dessert_Unchecked(object sender, RoutedEventArgs e)
        {
            dessert.Foreground = new SolidColorBrush(Color.FromArgb(255, 211, 134, 0));
        }
        #endregion
    }
}
