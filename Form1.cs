using System.Drawing.Imaging;

namespace ComapareImageForManualView
{
    public partial class Form1 : Form
    {
        enum enImageFormat { jpg, bmp }

        string sImageFolderPath_1 = "";
        string sImageFolderPath_2 = "";

        public Form1()
        {
            InitializeComponent();

            btnChoseImageFolderPath_1.Click += ButtonImageFolder_Click;
            btnChoseImageFolderPath_2.Click += ButtonImageFolder_Click;
            nudOrderNumber.ValueChanged += NumericUpDown1_ValueChanged;
            Combobox_InitImageFormat(cmbImageFormat_1);
            Combobox_InitImageFormat(cmbImageFormat_2);
        }

        #region 路徑選擇相關函式
        /// <summary>
        /// 選擇的資料夾路徑的按鍵點擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonImageFolder_Click(object sender, EventArgs e)
        {

            Button button = (Button)sender;
            if (button.Name.Contains("_1"))
                choseFolderPath(tbImageFolderPath_1, ref sImageFolderPath_1);
            else if (button.Name.Contains("_2"))
                choseFolderPath(tbImageFolderPath_2, ref sImageFolderPath_2);
        }

        /// <summary>
        /// 將選擇的資料夾路徑存取與顯示於TextBox
        /// </summary>
        /// <param name="tbFolderPath">顯示資料夾路徑的TextBox</param>
        /// <param name="sFolderPath">存取資料夾路徑的字串</param>
        void choseFolderPath(TextBox tbFolderPath, ref string sFolderPath)
        {
            SelectFolder.FolderBrowserDialog fbd = new SelectFolder.FolderBrowserDialog();

            try
            {
                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    sFolderPath = fbd.DirectoryPath;
                    tbFolderPath.Text = fbd.DirectoryPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 初始化ComboBox，填入項目為圖片格式
        /// </summary>
        /// <param name="cmb"></param>
        void Combobox_InitImageFormat(ComboBox cmb)
        {
            cmb.Items.Clear();

            foreach (enImageFormat enTerm in Enum.GetValues(typeof(enImageFormat)))
            {
                cmb.Items.Add(enTerm);
            }
            cmb.SelectedIndex = 0;
        }

        #region 圖片顯示相關函式
        /// <summary>
        /// 透過數值改變，觸發圖片的顯示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown nud = (NumericUpDown)sender;
                int intNumber = Convert.ToInt32(nud.Value);

                string sImageFormat_1 = cmbImageFormat_1.Text;
                string sImageFormat_2 = cmbImageFormat_2.Text;

                //ShowPicture_Method1(intNumber, sImageFolderPath_1, sImageFolderPath_2, pbImage_1, pbImage_2);

                ShowPicture_Method2(intNumber, sImageFolderPath_1, sImageFolderPath_2, pbImage_1, pbImage_2, sImageFormat_1, sImageFormat_2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 顯示圖片
        /// </summary>
        /// <param name="intNumber">順序數字</param>
        /// <param name="sImageFolderPath_1">圖檔資料夾路徑1</param>
        /// <param name="sImageFolderPath_2">圖檔資料夾路徑2</param>
        /// <param name="pbImage_1">顯示圖片的PictureBox1</param>
        /// <param name="pbImage_2">顯示圖片的PictureBox2</param>
        void ShowPicture_Method1(int intNumber, string sImageFolderPath_1, string sImageFolderPath_2, PictureBox pbImage_1, PictureBox pbImage_2)
        {
            string sPathImage_1 = sImageFolderPath_1 + "\\" + (intNumber).ToString("0000") + ".jpg";
            string sPathImage_2 = sImageFolderPath_2 + "\\" + (intNumber).ToString("0000") + ".jpg";

            Image bmp_1, bmp_2;
            pbImage_1.Image = null;
            pbImage_2.Image = null;

            bmp_1 = Image.FromFile(sPathImage_1);
            bmp_2 = Image.FromFile(sPathImage_2);
            ZoomImage(pbImage_1, bmp_1);
            ZoomImage(pbImage_2, bmp_2);
        }

        void ShowPicture_Method2(int intNumber, string sImageFolderPath_1, string sImageFolderPath_2, 
                            PictureBox pbImage_1, PictureBox pbImage_2, string sImageFormat_1, string sImageFormat_2)
        {

            string sPathImage_1 = JudgeImageExist(intNumber, sImageFolderPath_1, sImageFormat_1, "");
            string sPathImage_2 = JudgeImageExist(intNumber, sImageFolderPath_2, sImageFormat_2, "");

            Image bmp_1, bmp_2;
            pbImage_1.Image = null;
            pbImage_2.Image = null;
            if (sPathImage_1 == "" || sPathImage_2 == "")
                return;

            bmp_1 = Image.FromFile(sPathImage_1);
            bmp_2 = Image.FromFile(sPathImage_2);
            ZoomImage(pbImage_1, bmp_1);
            ZoomImage(pbImage_2, bmp_2);
        }


        string JudgeImageExist(int intNumber, string sImageFolderPath, string sImageFormat, string sKeyword)
        {
            string sResult = "";
            string sOrder = intNumber.ToString("0000");
            string[] arrayImagePath = Directory.GetFiles(sImageFolderPath);

            if(arrayImagePath.Length == 0)
                return sResult;

            for (int i = 0; i < arrayImagePath.Length; i++)
            {
                string sFileName = Path.GetFileNameWithoutExtension(arrayImagePath[i]);
                string sFileExtension = Path.GetExtension(arrayImagePath[i]);

                if (sFileName == sOrder + sKeyword && sFileExtension == "." + sImageFormat)
                    return arrayImagePath[i];
            }

            return sResult;
        }

        /// <summary>
        /// 調整圖片大小填入PictureBox
        /// </summary>
        /// <param name="pb">顯示圖片的PictureBox</param>
        /// <param name="bmp">圖</param>
        void ZoomImage(PictureBox pb, Image bmp)
        {
            Image bmpClone;

            bmpClone = (Image)bmp.Clone(); //複製一個重複的影像

            // 定義存圖的框架多大的點陣
            Bitmap bmpZoom = new Bitmap(pb.Width, pb.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            // 將bmpZoom建立為新圖形
            Graphics g = Graphics.FromImage(bmpZoom);

            // 定義存圖的 位置 與 長寬大小
            Rectangle rect = new Rectangle(0, 0, pb.Width, pb.Height);

            g.DrawImage(bmpClone, rect);
            pb.Image = bmpZoom.Clone(new Rectangle(0, 0, bmpZoom.Width, bmpZoom.Height), bmpZoom.PixelFormat);
            bmpZoom.Dispose();
            bmpClone.Dispose();
        }

        #endregion
    }
}