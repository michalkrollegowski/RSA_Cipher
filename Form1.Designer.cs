﻿namespace RSA_Cipher
{
    partial class window_application
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.encryption_field = new System.Windows.Forms.TextBox();
            this.encryption_label = new System.Windows.Forms.Label();
            this.rsa_label = new System.Windows.Forms.Label();
            this.decryption_field = new System.Windows.Forms.TextBox();
            this.decryption_label = new System.Windows.Forms.Label();
            this.encryption_button = new System.Windows.Forms.Button();
            this.decrypting_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // encryption_field
            // 
            this.encryption_field.Location = new System.Drawing.Point(85, 117);
            this.encryption_field.Margin = new System.Windows.Forms.Padding(4);
            this.encryption_field.Multiline = true;
            this.encryption_field.Name = "encryption_field";
            this.encryption_field.Size = new System.Drawing.Size(933, 190);
            this.encryption_field.TabIndex = 0;
            // 
            // encryption_label
            // 
            this.encryption_label.AutoSize = true;
            this.encryption_label.Location = new System.Drawing.Point(155, 97);
            this.encryption_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.encryption_label.Name = "encryption_label";
            this.encryption_label.Size = new System.Drawing.Size(106, 16);
            this.encryption_label.TabIndex = 1;
            this.encryption_label.Text = "Encrytpion Field:";
            // 
            // rsa_label
            // 
            this.rsa_label.AutoSize = true;
            this.rsa_label.Cursor = System.Windows.Forms.Cursors.Default;
            this.rsa_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rsa_label.Location = new System.Drawing.Point(112, 23);
            this.rsa_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rsa_label.Name = "rsa_label";
            this.rsa_label.Size = new System.Drawing.Size(198, 39);
            this.rsa_label.TabIndex = 2;
            this.rsa_label.Text = "RSA Cipher";
            this.rsa_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // decryption_field
            // 
            this.decryption_field.Location = new System.Drawing.Point(85, 361);
            this.decryption_field.Margin = new System.Windows.Forms.Padding(4);
            this.decryption_field.Multiline = true;
            this.decryption_field.Name = "decryption_field";
            this.decryption_field.Size = new System.Drawing.Size(933, 200);
            this.decryption_field.TabIndex = 3;
            // 
            // decryption_label
            // 
            this.decryption_label.AutoSize = true;
            this.decryption_label.Location = new System.Drawing.Point(155, 341);
            this.decryption_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.decryption_label.Name = "decryption_label";
            this.decryption_label.Size = new System.Drawing.Size(108, 16);
            this.decryption_label.TabIndex = 4;
            this.decryption_label.Text = "Decryption Field:";
            // 
            // encryption_button
            // 
            this.encryption_button.Location = new System.Drawing.Point(920, 315);
            this.encryption_button.Margin = new System.Windows.Forms.Padding(4);
            this.encryption_button.Name = "encryption_button";
            this.encryption_button.Size = new System.Drawing.Size(100, 28);
            this.encryption_button.TabIndex = 5;
            this.encryption_button.Text = "Encrypt";
            this.encryption_button.UseVisualStyleBackColor = true;
            this.encryption_button.Click += new System.EventHandler(this.encryption_button_Click);
            // 
            // decrypting_button
            // 
            this.decrypting_button.Location = new System.Drawing.Point(920, 569);
            this.decrypting_button.Margin = new System.Windows.Forms.Padding(4);
            this.decrypting_button.Name = "decrypting_button";
            this.decrypting_button.Size = new System.Drawing.Size(100, 28);
            this.decrypting_button.TabIndex = 6;
            this.decrypting_button.Text = "Decrypt";
            this.decrypting_button.UseVisualStyleBackColor = true;
            this.decrypting_button.Click += new System.EventHandler(this.decryption_button_Click);
            // 
            // window_application
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 700);
            this.Controls.Add(this.decrypting_button);
            this.Controls.Add(this.encryption_button);
            this.Controls.Add(this.decryption_label);
            this.Controls.Add(this.decryption_field);
            this.Controls.Add(this.rsa_label);
            this.Controls.Add(this.encryption_label);
            this.Controls.Add(this.encryption_field);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "window_application";
            this.Text = "RSA Cipher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox encryption_field;
        private System.Windows.Forms.Label encryption_label;
        private System.Windows.Forms.Label rsa_label;
        private System.Windows.Forms.TextBox decryption_field;
        private System.Windows.Forms.Label decryption_label;
        private System.Windows.Forms.Button encryption_button;
        private System.Windows.Forms.Button decrypting_button;
    }
}

