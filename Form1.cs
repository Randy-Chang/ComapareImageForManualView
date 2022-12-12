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

        #region ���|��ܬ����禡
        /// <summary>
        /// ��ܪ���Ƨ����|�������I���ƥ�
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
        /// �N��ܪ���Ƨ����|�s���P��ܩ�TextBox
        /// </summary>
        /// <param name="tbFolderPath">��ܸ�Ƨ����|��TextBox</param>
        /// <param name="sFolderPath">�s����Ƨ����|���r��</param>
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
        /// ��l��ComboBox�A��J���ج��Ϥ��榡
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

        #region �Ϥ���ܬ����禡
        /// <summary>
        /// �z�L�ƭȧ��ܡAĲ�o�Ϥ������
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
        /// ��ܹϤ�
        /// </summary>
        /// <param name="intNumber">���ǼƦr</param>
        /// <param name="sImageFolderPath_1">���ɸ�Ƨ����|1</param>
        /// <param name="sImageFolderPath_2">���ɸ�Ƨ����|2</param>
        /// <param name="pbImage_1">��ܹϤ���PictureBox1</param>
        /// <param name="pbImage_2">��ܹϤ���PictureBox2</param>
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
        /// �վ�Ϥ��j�p��JPictureBox
        /// </summary>
        /// <param name="pb">��ܹϤ���PictureBox</param>
        /// <param name="bmp">��</param>
        void ZoomImage(PictureBox pb, Image bmp)
        {
            Image bmpClone;

            bmpClone = (Image)bmp.Clone(); //�ƻs�@�ӭ��ƪ��v��

            // �w�q�s�Ϫ��ج[�h�j���I�}
            Bitmap bmpZoom = new Bitmap(pb.Width, pb.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            // �NbmpZoom�إ߬��s�ϧ�
            Graphics g = Graphics.FromImage(bmpZoom);

            // �w�q�s�Ϫ� ��m �P ���e�j�p
            Rectangle rect = new Rectangle(0, 0, pb.Width, pb.Height);

            g.DrawImage(bmpClone, rect);
            pb.Image = bmpZoom.Clone(new Rectangle(0, 0, bmpZoom.Width, bmpZoom.Height), bmpZoom.PixelFormat);
            bmpZoom.Dispose();
            bmpClone.Dispose();
        }

        #endregion
    }
}