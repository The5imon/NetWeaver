using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;

namespace NetWeaverGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Room> rooms;
        private List<Client> clients;

        
        public MainWindow(EventInterface face, DBInterface dBInterface)
        {
            InitializeComponent();

            TextBlock funktionen = new TextBlock();
            funktionen.Text = "Funktionen";
            funktionen.FontSize = 20;
            Button scripts = new Button();
            scripts.Content = "Scripts";
            scripts.FontSize = 20;
            scripts.Background = Brushes.White;
            scripts.Tag = "Script";
            scripts.Click += button_click;
            TextBlock raume = new TextBlock();
            raume.Text = "Räume";
            raume.FontSize = 20;
            
            Navbar.Children.Add(funktionen);
            Navbar.Children.Add(scripts);
            Navbar.Children.Add(raume);
            /*foreach (Room room in rooms)
            {
                Button button = new Button();
                button.Content = room.Roomname.ToUpper();
                button.FontSize = 20;
                button.Tag = room;
                button.Click += button_click;
                button.Background = Brushes.White;
                Navbar.Children.Add(button);
            }*/
        }

        void button_click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button?.Tag.GetType().Name)
            {
                case "Client":
                    MessageBox.Show(((Client)button.Tag).HostName+"\n"
                                                                 +((Client)button.Tag).RoomNumber+"\n"
                                                                 +((Client)button.Tag).IPAddress+"\n"
                                                                 +((Client)button.Tag).MAC+"\n"
                                                                 +((Client)button.Tag).IsOnline);
                    break;
                case "Room":
                    ShowRoom((Room)button.Tag);
                    break;
                case "String":
                    switch (button.Tag)
                    {
                        case "Script":
                            ShowScript();
                            break;
                        case "SelectFile":
                            GetFile(button);
                            break;
                        case "StartSpread":
                            SpreadFiles();
                            break;
                    }
                    break;
                default:
                    throw new Exception("Button has an invalid Name! \nSeek a developer of the Project!");
            }
        }

        private void SpreadFiles()
        {
            
        }

        private void GetFile(Button button)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Scripts|*.txt;*.bat;*.ps1";
            openFileDialog.ShowDialog();
            string path = openFileDialog.FileName;
            if (path.Length != 0)
            {
                button.Content = path;
//                TextBox textBox = (TextBox) Workground.Children[1];
//                textBox.Clear();
//                using (var reader =
//                    new StreamReader(
//                        path))
//                {
//                    while (!reader.EndOfStream)
//                    {
//                        textBox.Text += reader.ReadLine() + "\n";
//                    }
//                }
            }
        }

        void ShowRoom(Room room)
        {
            if (Workground.Children.Count != 0)
            {
                Workground.Children.Clear();
            }

            Menu.Text = room.Roomname.ToUpper();
            foreach (Client client in clients)
            {
                if (client.RoomNumber == room.RoomNumber)
                {
                    Button button = new Button();
                    button.HorizontalContentAlignment = HorizontalAlignment.Left;
                    
                    Image image = new Image();
                    if (client.IsOnline)
                    {
                        image.Source = new BitmapImage(new Uri("C:\\Users\\Max\\source\\repos\\WpfApp3\\WpfApp3\\Data\\PC_on.png"));
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("C:\\Users\\Max\\source\\repos\\WpfApp3\\WpfApp3\\Data\\PC_off.png"));
                    }
                    image.Width = 20;
                    
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = client.HostName;
                    
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(textBlock);

                    button.Content = stackPanel;
                    button.FontSize = 16;
                    button.Tag = client;
                    button.Click += button_click;
                    
                    
                    button.Background = Brushes.White;
                    Workground.Children.Add(button);
                }
            }
        }

        void ShowScript()
        {
            if (Workground.Children.Count != 0)
            {
                Workground.Children.Clear();
            }

            Menu.Text = "Script";
            
            Button selectFileButton = new Button();
            selectFileButton.Content="Datei auswählen";
            selectFileButton.Tag = "SelectFile";
            selectFileButton.Click += button_click;
            
//            TextBox textBox = new TextBox();
//            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
//            textBox.IsReadOnly = true;
//            textBox.Height = ActualHeight;
            
            Button start = new Button();
            start.Content = "Start";

            Workground.Children.Add(selectFileButton);
//            Workground.Children.Add(textBox);
            Workground.Children.Add(start);
        }
        
        
    }
}
