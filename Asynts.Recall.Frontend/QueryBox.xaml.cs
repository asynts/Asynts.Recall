using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

using Asynts.Recall.Frontend.Models;

namespace Asynts.Recall.Frontend
{
    /// <summary>
    /// Interaction logic for QueryBox.xaml
    /// </summary>
    public partial class QueryBox : UserControl
    {
        private readonly QueryBoxModel _queryBoxModel;

        public QueryBox()
        {
            InitializeComponent();

            _queryBoxModel = new QueryBoxModel();
            DataContext = _queryBoxModel;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _queryBoxModel.SubmitQueryCommand.Execute(null);
            }
        }
    }
}
