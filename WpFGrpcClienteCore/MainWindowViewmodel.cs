using Shared.Cliente;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WpFGrpcClienteCore
{
    public class MainWindowViewmodel : INotifyPropertyChanged
    {

        private MainWindowViewmodel() 
        {
            Notificaçao_Cliente.GetInstance().Mensagem_sistemaChanged += MainWindowViewmodel_Texto_notificaçaoChanged;
            Notificaçao_Cliente.GetInstance().Mensagem_recebidaChanged += MainWindowViewmodel_Mensagem_recebidaChanged;
           
        }

        private void MainWindowViewmodel_Mensagem_recebidaChanged(object? sender, PropertyChangedEventArgs e)
        {
          
            Mensagem_recebida = Notificaçao_Cliente.GetInstance().Mensagem_recebida;
            
        }

        private void MainWindowViewmodel_Texto_notificaçaoChanged(object? sender, PropertyChangedEventArgs e)
        {
            Mensagem_sistema = Notificaçao_Cliente.GetInstance().Mensagem_sistema;
        }

        private static MainWindowViewmodel _notificaçao;
        private static readonly object _lock = new object();
        public static MainWindowViewmodel GetInstance()
        {

            if (_notificaçao == null)
            {

                lock (_lock)
                {

                    if (_notificaçao == null)
                    {
                        _notificaçao = new MainWindowViewmodel();

                    }
                }
            }
            return _notificaçao;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private string mensagem_sistema = "";
        public string Mensagem_sistema
        {
            get { return mensagem_sistema; }
            set
            {
                if(mensagem_sistema != value)
                {
                mensagem_sistema = value;
                
                    MainWindowViewmodel.GetInstance().Mensagem_sistema_coleçao.Insert(0, value);
                    OnPropertyChanged();  
                }             
            }
        }

        private string mensagem_recebida = "";
        public string Mensagem_recebida
        {
            get { return mensagem_recebida; }
            set
            {
                if (mensagem_recebida != value)
                {
                    mensagem_recebida = value;

                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> mensagem_sistema_coleçao=new ObservableCollection<string>();
        public ObservableCollection<string> Mensagem_sistema_coleçao
        {
            get { return mensagem_sistema_coleçao; }
            set
            {
                mensagem_sistema_coleçao = value;
                OnPropertyChanged();
            }


        }
    }
}