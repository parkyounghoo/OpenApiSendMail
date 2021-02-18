using System;
using System.Windows.Forms;

namespace KONEPS_KPC_MailBatch
{
    public partial class Form1 : Form
    {
        public string formText
        {
            get { return this.lbResult.Text; }
            set { this.lbResult.Items.Add(value);}
        }
        public Form1()
        {
            InitializeComponent();
        }

        apiRequestTimer timer;
        private void btnStart_Click(object sender, EventArgs e)
        {
            lbResult.Items.Add("▶ 메일 발송 시작 " + DateTime.Now);
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            timer = new apiRequestTimer();
            timer.OnStart(this);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lbResult.Items.Add("▶ 메일 발송 중지 " + DateTime.Now);
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            timer.OnClose();
        }
    }
}
