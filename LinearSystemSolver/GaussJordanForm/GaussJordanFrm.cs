using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace مشروع_جبر_خطي
{
    public partial class GaussJordanFrm : Form
    {
        public GaussJordanFrm()
        {
            InitializeComponent();
        }
        
        Form Equations = null;
        Form Matrix = null;
        Form Inverse = null;

        private void enterEquationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Equations == null || Equations.IsDisposed)
            {
                Equations = new EquationsForm();
                Equations.MdiParent = this;
                Equations.Show();
            }
            else
            {
                Equations.Focus();
            }
        }

        private void enterMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Matrix == null || Matrix.IsDisposed)
            {
                Matrix = new MatrixForm();
                Matrix.MdiParent = this;
                Matrix.Show();
            }
            else
            {
                Matrix.Focus();
            }
        }

        private void matrixInverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Inverse == null || Inverse.IsDisposed)
            {
                Inverse = new InverseForm();
                Inverse.MdiParent = this;
                Inverse.Show();
            }
            else
            {
                Inverse.Focus();
            }
        }
    }
}
