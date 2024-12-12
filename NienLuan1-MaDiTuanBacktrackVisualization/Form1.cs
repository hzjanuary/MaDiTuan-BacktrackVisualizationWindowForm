using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NienLuan1_MaDiTuanBacktrackVisualization
{
    public partial class MainForm : Form
    {
        const int kichthuoc = 8;
        int[,] KhungCo = new int[kichthuoc + 1, kichthuoc + 1];
        int[] x = { 2, 1, -1, -2, -2, -1, 1, 2 };
        int[] y = { 1, 2, 2, 1, -1, -2, -2, -1 };
        bool thang = false;
        public MainForm()
        {
            InitializeComponent();
            HienThiBanCo();
        }
        private void HienThiBanCo()
        {
            tblChessBoard.Controls.Clear();
            tblChessBoard.RowCount = kichthuoc;
            tblChessBoard.ColumnCount = kichthuoc;

            for (int i = 0; i < kichthuoc; i++)
            {
                for (int j = 0; j < kichthuoc; j++)
                {
                    Label cell = new Label
                    {
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = (i + j) % 2 == 0 ? Color.White : Color.Black,
                        ForeColor = Color.Black,
                        Font = new Font("Arial", 12, FontStyle.Bold)
                    };
                    tblChessBoard.Controls.Add(cell, j, i);
                }
            }
        }
        private async Task Di(int x_m, int y_m, int n)
        {
            int x_t, y_t;
            for (int i = 0; i < 8; i++)
            {
                x_t = x_m + x[i];
                y_t = y_m + y[i];
                if (0 < x_t && x_t <= kichthuoc && 0 < y_t && y_t <= kichthuoc && KhungCo[x_t, y_t] == 0)
                {
                    KhungCo[x_t, y_t] = n;
                    MarkPosition(x_t - 1, y_t - 1, n);  // Đánh dấu ô bằng màu

                    await Task.Delay(200);

                    if (n == kichthuoc * kichthuoc)
                    {
                        thang = true;
                        MessageBox.Show("Thành công!");
                        return;
                    }

                    if (!thang)
                    {
                        await Di(x_t, y_t, n + 1);
                    }

                    if (thang) return;

                    KhungCo[x_t, y_t] = 0;
                    ClearPosition(x_t - 1, y_t - 1);  // Xóa màu khi quay lui
                    await Task.Delay(100);
                }
            }
        }
        private void MarkPosition(int row, int col, int step)
        {
            var cell = tblChessBoard.GetControlFromPosition(col, row) as Label;
            if (cell != null)
            {
                cell.Text = step.ToString();
                cell.BackColor = Color.Purple;  // Màu cho các bước di chuyển
            }
        }
        private void ClearPosition(int row, int col)
        {
            var cell = tblChessBoard.GetControlFromPosition(col, row) as Label;
            if (cell != null)
            {
                cell.Text = "";
                cell.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            int x_chon = int.Parse(txtX.Text);
            int y_chon = int.Parse(txtY.Text);

            if (x_chon < 1 || x_chon > kichthuoc || y_chon < 1 || y_chon > kichthuoc)
            {
                MessageBox.Show("Tọa độ không hợp lệ!");
                return;
            }

            for (int i = 0; i <= kichthuoc; i++)
            {
                for (int j = 0; j <= kichthuoc; j++)
                {
                    KhungCo[i, j] = 0;
                }
            }

            KhungCo[x_chon, y_chon] = 1;
            MarkPosition(y_chon - 1, x_chon - 1, 1);

            thang = false;
            await Di(x_chon, y_chon, 2);

            if (!thang)
            {
                MessageBox.Show("Không có lời giải.");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
