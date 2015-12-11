using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube.Pdf.App.Page
{
    public partial class PasswordForm : Form
    {
        public PasswordForm(string filename="")
        {
            InitializeComponent();
            PasswordLabel.Text = string.Format("\"{0}\"のオーナーパスワードを入力してください。",filename);
        }
        public string Password
        {
            get { return PasswordTextBox.Text; }
        }
    }
}
