using System;
using System.IO;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        #region Private members

        /// <summary>
        /// Is file saved
        /// </summary>
        private bool mIsSaved = true;

        /// <summary>
        /// The name of the opened file
        /// </summary>
        private string mCurrentFile = string.Empty;

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If the rich textbox has text...
            if (richTextBox1.Text != string.Empty)
            {
                // If the user agrees to losing content...
                if (((DialogResult.OK == MessageBox.Show("The content will be lost.", "Continue?", MessageBoxButtons.OKCancel))))
                {
                    // set current file to none
                    mCurrentFile = string.Empty;

                    // clear the text of rich textbox
                    richTextBox1.Text = string.Empty;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // If the user selected a file...
                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    // get the name of the selected file
                    mCurrentFile = openFileDialog.FileName;

                    // If the file is of rich text format...
                    if (Path.GetExtension(mCurrentFile) == ".rtf")
                        // load the file's content into the rich textbox as rich text format
                        richTextBox1.LoadFile(mCurrentFile);
                    // If the file of plain text type...
                    else
                        // load the file's content into the rich textbox as plain text
                        richTextBox1.LoadFile(mCurrentFile, RichTextBoxStreamType.PlainText);

                    // Add the file name to the window title
                    this.Text = Path.GetFileName(mCurrentFile) + " - Text Editor";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mIsSaved = true;

            if (mCurrentFile == "")
                saveAsToolStripMenuItem_Click(sender, e);
            else
                richTextBox1.SaveFile(mCurrentFile, RichTextBoxStreamType.PlainText);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentFile == "")
                saveFileDialog.FileName = "Untitled";

            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                if (Path.GetExtension(saveFileDialog.FileName) == ".txt" || Path.GetExtension(saveFileDialog.FileName) == ".cs")
                    richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                else
                    richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);

                mCurrentFile = saveFileDialog.FileName;

                this.Text = Path.GetFileName(mCurrentFile) + " - Text Editor";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            mIsSaved = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mIsSaved == false)
            {
                if (MessageBox.Show("Changes were not saved. Exit?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
    }
}
