using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WAVPlayer
{
    public partial class frmWAVPlayer : Form
    {
        SoundPlayer player;
        public frmWAVPlayer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 當使用者按下「瀏覽」按鈕時，開啟檔案對話框讓使用者選擇 WAV 檔案，並將選擇的檔案路徑顯示在 txtFilePath 文字框中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.ofdWAVFile.Filter = "WAV Files(*.wav)|*.wav";
            if (this.ofdWAVFile.ShowDialog() == DialogResult.OK)
            {
                //this.txtPath.Text = this.ofdWAVFile.FileName;
                txtPath.Text = ofdWAVFile.FileName;

                FileInfo info = new FileInfo(txtPath.Text);

                lblFileName.Text = "檔名：" + info.Name;

                lblFileSize.Text = "大小：" + info.Length / 1024 + " KB";
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            /*try
            {
                player = new SoundPlayer();  //建立撥放器物件
                player.SoundLocation = txtPath.Text;         //指定音效所在路徑檔名
                player.Load();                                               //載入音效檔資料
                player.Play();                                                //撥放音效
                //player1.PlaySync();                                    //同步撥放音效，直到撥放完成才繼續執行後續程式碼
                //MessageBox.Show("音效撥放完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
            //沒有選檔案
            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("請先選擇 WAV 檔案！");
                return;
            }

            //檔案不存在
            if (!File.Exists(txtPath.Text))
            {
                MessageBox.Show("檔案不存在！");
                return;
            }
            try
            {
                player = new SoundPlayer(txtPath.Text);

                player.Load();

                player.Play();

                lblStatus.Text = "狀態：播放中";
            }
            catch (Exception ex)
            {
                MessageBox.Show("無法撥放音效檔，請確認檔案路徑是否正確！\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            //沒有選檔案
            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("請先選擇 WAV 檔案！");
                return;
            }

            //檔案不存在
            if (!File.Exists(txtPath.Text))
            {
                MessageBox.Show("檔案不存在！");
                return;
            }
            //使用完整檔名建立物件
            player = new SoundPlayer(txtPath.Text);
            player.PlayLooping();                                 //重複播放
            lblStatus.Text = "狀態：循環播放";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //player.Stop();                                        //停止撥放
            //fsWAV.Close();
            if (player != null)
            {
                player.Stop();

                lblStatus.Text = "狀態：已停止";
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.Close();
        }

        private void frmWAVPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("確定要關閉應用程式嗎？", "關閉確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;  //取消關閉
            }
        }

        private void frmWAVPlayer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void frmWAVPlayer_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            txtPath.Text = files[0];

            FileInfo info = new FileInfo(txtPath.Text);

            lblFileName.Text = "檔名：" + info.Name;

            lblFileSize.Text = "大小：" + info.Length / 1024 + " KB";
        }
    }
}
