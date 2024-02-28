using Shared.Cliente;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WpFGrpcClienteCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewmodel.GetInstance();
            this.DataContext = MainWindowViewmodel.GetInstance();
            listmensagem_sistema.SetBinding(ListBox.ItemsSourceProperty,new Binding("Mensagem_sistema_coleçao") { Source = MainWindowViewmodel.GetInstance() });
            textobloco.SetBinding(TextBlock.TextProperty, new Binding("Mensagem_recebida") { Source = MainWindowViewmodel.GetInstance() });
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task task = Canal.GetInstance().chamarserviço();
        }
    }
}