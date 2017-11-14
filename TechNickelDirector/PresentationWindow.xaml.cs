using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TechNickelDirector
{
    /// <summary>
    /// Interaction logic for PresentationWindow.xaml
    /// </summary>
    public partial class PresentationWindow : Window
    {
        public PresentationWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // show the message box here and collect the result
            // if you want to stop it, set e.Cancel = true
            e.Cancel = true;
        }

    }
}
