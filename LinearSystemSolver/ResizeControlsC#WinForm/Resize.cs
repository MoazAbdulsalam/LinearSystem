using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace مشروع_جبر_خطي
{
    internal class Resize
    {

       
        private Dictionary<Control, CtrlInfo> DefaultControlSizes = new Dictionary<Control, CtrlInfo>();
        private Size FormDefaultClientSize;
        private Form TargetForm;

        public Resize(Form form)
        {
            TargetForm = form;
            FormDefaultClientSize = form.ClientSize;

            Control ctrl = form.GetNextControl(form, true);
            while (ctrl != null)
            {
                if (ctrl is ListBox listBox)
                    listBox.IntegralHeight = false;

                DefaultControlSizes.Add(ctrl, new CtrlInfo(ctrl.Bounds, ctrl.Font.Size));
                ctrl = form.GetNextControl(ctrl, true);
            }

            form.Resize += Form_Resize;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            ScaleControls();
        }

        private void ScaleControls()
        {
            if (TargetForm.WindowState != FormWindowState.Minimized)
            {
                foreach (var kvp in DefaultControlSizes)
                {
                    Control ctrl = kvp.Key;

                    double Xscl = (double)TargetForm.ClientSize.Width / FormDefaultClientSize.Width;
                    double Yscl = (double)TargetForm.ClientSize.Height / FormDefaultClientSize.Height;

                    float fntscl = (float)(kvp.Value.cFontSize * Yscl);
                    ctrl.Font = new Font(ctrl.Font.FontFamily, fntscl, ctrl.Font.Style, ctrl.Font.Unit);

                    ctrl.Width = (int)(kvp.Value.cBounds.Width * Xscl);
                    ctrl.Height = (int)(kvp.Value.cBounds.Height * Yscl);
                    ctrl.Left = (int)(kvp.Value.cBounds.X * Xscl);
                    ctrl.Top = (int)(kvp.Value.cBounds.Y * Yscl);

                    // 🔹 تعديل خاص للـ ListView Columns
                    if (ctrl is ListView listView && listView.View == View.Details && listView.Columns.Count > 0)
                    {
                        int totalWidth = listView.ClientSize.Width;
                        int colWidth = totalWidth / listView.Columns.Count;

                        foreach (ColumnHeader col in listView.Columns)
                        {
                            col.Width = colWidth;
                        }
                    }
                }
            }
        }

        private class CtrlInfo
        {
            public Rectangle cBounds;
            public float cFontSize;

            public CtrlInfo(Rectangle bounds, float fontSize)
            {
                cBounds = bounds;
                cFontSize = fontSize;
            }
        }
    }

    
}
