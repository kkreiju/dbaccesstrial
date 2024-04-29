using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void SubjectEntryButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            SubjectEntry subjectEntry = new SubjectEntry();
            subjectEntry.ShowDialog();
        }

        private void SubjectScheduleEntryButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            SubjectScheduleEntry subjectScheduleEntry = new SubjectScheduleEntry();
            subjectScheduleEntry.ShowDialog();
        }
    }
}
