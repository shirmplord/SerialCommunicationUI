
using System;

namespace SerialCommunicationUI
{
    partial class CommUI
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
            this.components = new System.ComponentModel.Container();
            this.Serial = new System.Windows.Forms.GroupBox();
            this.tbDataReceived = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cBoxTarget = new System.Windows.Forms.ComboBox();
            this.lCommand = new System.Windows.Forms.Label();
            this.lPort = new System.Windows.Forms.Label();
            this.cBoxPortName = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.cBoxCommand = new System.Windows.Forms.ComboBox();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.Serial.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Serial
            // 
            this.Serial.Controls.Add(this.tbDataReceived);
            this.Serial.Location = new System.Drawing.Point(263, 12);
            this.Serial.Name = "Serial";
            this.Serial.Size = new System.Drawing.Size(115, 172);
            this.Serial.TabIndex = 0;
            this.Serial.TabStop = false;
            this.Serial.Text = "Data received";
            // 
            // tbDataReceived
            // 
            this.tbDataReceived.Location = new System.Drawing.Point(6, 35);
            this.tbDataReceived.Name = "tbDataReceived";
            this.tbDataReceived.Size = new System.Drawing.Size(100, 20);
            this.tbDataReceived.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cBoxTarget);
            this.groupBox2.Controls.Add(this.lCommand);
            this.groupBox2.Controls.Add(this.lPort);
            this.groupBox2.Controls.Add(this.cBoxPortName);
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Controls.Add(this.cBoxCommand);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 172);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Target";
            // 
            // cBoxTarget
            // 
            this.cBoxTarget.FormattingEnabled = true;
            this.cBoxTarget.Items.AddRange(new object[] {
            "1",
            "4"});
            this.cBoxTarget.Location = new System.Drawing.Point(5, 116);
            this.cBoxTarget.Name = "cBoxTarget";
            this.cBoxTarget.Size = new System.Drawing.Size(129, 21);
            this.cBoxTarget.TabIndex = 7;
            // 
            // lCommand
            // 
            this.lCommand.AutoSize = true;
            this.lCommand.Location = new System.Drawing.Point(6, 57);
            this.lCommand.Name = "lCommand";
            this.lCommand.Size = new System.Drawing.Size(54, 13);
            this.lCommand.TabIndex = 6;
            this.lCommand.Text = "Command";
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Location = new System.Drawing.Point(6, 17);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(26, 13);
            this.lPort.TabIndex = 5;
            this.lPort.Text = "Port";
            this.lPort.Click += new System.EventHandler(this.label1_Click);
            // 
            // cBoxPortName
            // 
            this.cBoxPortName.FormattingEnabled = true;
            this.cBoxPortName.Location = new System.Drawing.Point(5, 33);
            this.cBoxPortName.Name = "cBoxPortName";
            this.cBoxPortName.Size = new System.Drawing.Size(129, 21);
            this.cBoxPortName.TabIndex = 4;
            this.cBoxPortName.SelectedIndexChanged += new System.EventHandler(this.cBoxPortName_SelectedIndexChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(160, 33);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // cBoxCommand
            // 
            this.cBoxCommand.FormattingEnabled = true;
            this.cBoxCommand.Items.AddRange(new object[] {
            "alive",
            "reset",
            "status",
            "version",
            "inputs",
            "param",
            "times",
            "aux_param",
            "output_ctrl",
            "led_ctrl",
            "simulate"});
            this.cBoxCommand.Location = new System.Drawing.Point(5, 73);
            this.cBoxCommand.Name = "cBoxCommand";
            this.cBoxCommand.Size = new System.Drawing.Size(129, 21);
            this.cBoxCommand.TabIndex = 2;
            this.cBoxCommand.SelectedIndexChanged += new System.EventHandler(this.cBoxCommand_SelectedIndexChanged);
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 115200;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortIn_DataReceived);
            // 
            // CommUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 194);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Serial);
            this.Name = "CommUI";
            this.Text = "Send data to module";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommUI_FormClosing);
            this.Load += new System.EventHandler(this.CommUI_Load);
            this.Serial.ResumeLayout(false);
            this.Serial.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        #endregion

        private System.Windows.Forms.GroupBox Serial;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cBoxCommand;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.ComboBox cBoxPortName;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.Label lCommand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBoxTarget;
        private System.Windows.Forms.TextBox tbDataReceived;
    }
}

