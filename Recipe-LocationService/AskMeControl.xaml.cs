using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Microsoft.Phone.Recipes.LocationService
{
    public delegate void OnButtonClickEventHandler(object sender, ButtonClickEventArgs e);

    public partial class AskMeControl : UserControl
    {
        public event OnButtonClickEventHandler OnButtonClick;

        public AskMeControl()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RaiseEvent(true);
        }

        private void DeclineButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RaiseEvent(false);
        }

        private void RaiseEvent(bool userApproves)
        {
            if (OnButtonClick != null)
                OnButtonClick(this, new ButtonClickEventArgs(userApproves));
        }
    }

    public class ButtonClickEventArgs : EventArgs
    {
        // Fields...
        private bool _UserApproves = false;

        public ButtonClickEventArgs(bool userApproves)
        {
            _UserApproves = userApproves;
        }

        public bool UserApproves
        {
            get { return _UserApproves; }
            set
            {
                _UserApproves = value;
            }
        }
    }
}