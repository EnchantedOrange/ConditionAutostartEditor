namespace ConditionedAutostartEditor.Properties
{
    partial class FormKek
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonKek = new Button();
            SuspendLayout();
            // 
            // buttonKek
            // 
            buttonKek.Location = new Point(12, 12);
            buttonKek.Name = "buttonKek";
            buttonKek.Size = new Size(75, 75);
            buttonKek.TabIndex = 0;
            buttonKek.Text = "kek";
            buttonKek.UseVisualStyleBackColor = true;
            buttonKek.Click += buttonKek_Click;
            // 
            // FormKek
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonKek);
            Name = "FormKek";
            Text = "FormKek";
            ResumeLayout(false);
        }

        #endregion

        private Button buttonKek;
    }
}