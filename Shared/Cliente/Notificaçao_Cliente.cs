using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Shared.Cliente
{
    public class Notificaçao_Cliente : INotifyPropertyChanged
    {
        #region implementaçao do singleton
        private Notificaçao_Cliente() { }
        private static Notificaçao_Cliente _notificaçao;
        private static readonly object _lock = new object();
        public static Notificaçao_Cliente GetInstance()
        {
            if (_notificaçao == null)
            {
                lock (_lock)
                {
                    if (_notificaçao == null)
                    {
                        _notificaçao = new Notificaçao_Cliente();
                    }
                }
            }
            return _notificaçao;
        }
        #endregion

        #region notificaçao de propriedades do singleton
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region propriedades implementadas e notificaçao dessas propriedades
        private string mensagem_sistema = "";
        public string Mensagem_sistema
        {
            get { return mensagem_sistema; }
            set
            {
                if (mensagem_sistema != value)
                {
                    mensagem_sistema = value;
                    OnMensagem_sistemaChanged();
                }
            }
        }
        public event PropertyChangedEventHandler Mensagem_sistemaChanged;
        protected void OnMensagem_sistemaChanged([CallerMemberName] string propertyName = "")
        {
            Mensagem_sistemaChanged?.Invoke(this, new PropertyChangedEventArgs(Mensagem_sistema));
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
                    OnMensagem_recebidaChanged();
                }
            }
        }
        public event PropertyChangedEventHandler Mensagem_recebidaChanged;
        protected void OnMensagem_recebidaChanged([CallerMemberName] string propertyName = "")
        {
            Mensagem_recebidaChanged?.Invoke(this, new PropertyChangedEventArgs(Mensagem_recebida));
        }

        #endregion

    }
}