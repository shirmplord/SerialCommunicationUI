using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO.Ports;

namespace SerialCommunicationUI
{
    public partial class CommUI : Form
    {
        /*
         * VARIABLE DECLARATIONS
         */
        private const byte _from = 64;
        private byte defaultTo = 1;
        //private byte crc16 = 0b1011;    // dummy crc16 value
        public CommUI()
        {
            InitializeComponent();
        }
        private void CommUI_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxPortName.Items.AddRange(ports);
            tbDataReceived.Multiline = true;
            tbDataReceived.Dock = DockStyle.Fill;
            LoadDefaultConfiguration();
        }
        // Load the default configurations for the COM port
        // Currently using app.config, xml better? 
        private void LoadDefaultConfiguration()
        {
            try
            {
                serialPort.WriteTimeout = 3000;
                serialPort.ReadTimeout = 3000;
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings.Count == 0)
                {
                    Console.WriteLine("Default configuration not found");
                }
                else
                {
                    serialPort.PortName = appSettings["comName"];
                    serialPort.BaudRate = int.Parse(appSettings["comBaudRate"]);
                    serialPort.DataBits = int.Parse(appSettings["comDataBits"]);
                    serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), appSettings["comStopBits"]);
                    serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), appSettings["comParity"]);

                    cBoxPortName.Text = serialPort.PortName;
                    cBoxCommand.SelectedItem = "status";
                    cBoxTarget.SelectedItem = 1;
                    cBoxTarget.Text = "1";
                    cBoxTarget.Enabled = false;
                }
                // When using the serialPort object, need to include a function
                // to check for data in receive buffer
                // serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPortIn_DataReceived);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading default port settings from App.config");
            }
        }
        // Event handler
        // Open port and send the data
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort.IsOpen == false) serialPort.Open();
                else
                {
                    Commands commands = new Commands();
                    SendData(commands.GetCode(cBoxCommand.Text), defaultTo);
                }
            }
            catch (UnauthorizedAccessException) 
            {
                //Console.WriteLine("Cannot open selected COM port");
                MessageBox.Show("Cannot open selected COM port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TimeoutException)
            {
                //Console.WriteLine("Operation timed out");
                MessageBox.Show("Operation timed out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        // Send data function
        // The Command class includes codes 100 - 108 and 140, 141. 
        // Command can be added to the items list of the Command box manually
        // provided the name in the box and the key in the dictionary are the same.
        private void SendData(byte code, byte _to)
        {
            CRC16Checker cRC16 = new CRC16Checker();
            StringBuilder sbOutputData = new StringBuilder();
            if (100 <= code && code <= 108)
            {
                // Calculate the CRC16 value
                StringBuilder sb = new StringBuilder();
                sb.Append(code);
                sb.Append(_from);
                sb.Append(_to);
                //Console.WriteLine("Sending the following data: {0}", sb.ToString());
                string hex = StringToHex(sb.ToString());
                string crc16 = CRC16Checker.ComputeChecksum(HexToByte(hex)).ToString("x2");
                //Console.WriteLine("On Write: {0}", hex);
                // Create the string to write to the port
                sbOutputData.Append("[");
                sbOutputData.Append(code + ",");
                sbOutputData.Append(_from + ",");
                sbOutputData.Append(_to + ",");
                sbOutputData.Append(crc16);
                sbOutputData.Append("]");
                serialPort.Write(sbOutputData.ToString());
            }
            else if (code == 140 || code == 141)
            {
                // Calculate the CRC16 value
                StringBuilder sb = new StringBuilder();
                byte data = 0b01;
                sb.Append(code);
                sb.Append(_from);
                sb.Append(cBoxTarget.Text);
                sb.Append(data);
                string hex = StringToHex(sb.ToString());
                string crc16 = CRC16Checker.ComputeChecksum(HexToByte(hex)).ToString("x2");
                // Create the string to write to the port
                sbOutputData.Append("[");
                sbOutputData.Append(code + ",");
                sbOutputData.Append(_from + ",");
                sbOutputData.Append(cBoxTarget.Text + ",");
                sbOutputData.Append(0b01 + ",");
                sbOutputData.Append(crc16);
                sbOutputData.Append("]");
                serialPort.Write(sbOutputData.ToString());
            }
            //Simulate a response from the module
            //else serialPort.Write("[12,1,64,102,0,1,1,1,1,0,0,10]");
            //else serialPort.Write("[12,1,64,102,0,1,1,1,1,0,0,d8a6]");
            else serialPort.Write("[0;1,64,103,404,ebb0]");
        }
        // Read the response from the serial line
        private void serialPortIn_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Read string 
                string inputData = serialPort.ReadExisting();
                string[] inputElements = DataParser(inputData);
                CRC16Checker cRC16 = new CRC16Checker();
                int resCode = int.Parse(inputElements[0]);
                Console.WriteLine(resCode);

                // Calculate the CRC16 value to compare with the input
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i< inputElements.Length -1;i++)
                {
                    sb.Append(inputElements[i]);
                }
                Console.WriteLine("Receiving the following data: {0}",sb.ToString());
                string hex = StringToHex(sb.ToString());
                Console.WriteLine("On read: {0}", hex);
                string crc16 = CRC16Checker.ComputeChecksum(HexToByte(hex)).ToString("x2");
                // Checksum is wrong
                if (crc16.Equals(inputElements[inputElements.Length - 1]) == false) 
                {
                    sb.Clear();
                    sb.Append("Checksum error, expected: " + crc16 + " Received: " + inputElements[inputElements.Length - 1]);
                    MessageBox.Show(sb.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                // nack response
                else if (resCode == 0) 
                {
                    MessageBox.Show("Encounter problem, transmission error detected", "NACK", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // ack response
                else if (resCode == 1) 
                {
                    MessageBox.Show("Transmission successful", "", MessageBoxButtons.OK);
                }
                // status response
                else if (resCode == 12) 
                {
                    try
                    {
                        ShowData(inputData);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Source + " " + error.Message);
                    }
                }
                // version response
                else if (resCode == 13) 
                {
                    try 
                    {
                        string version = "Version number: ";
                        version += inputElements[4];
                        ShowData(version);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Source +" "+ error.Message);
                    }
                }
                // Simulate sending back the response for each cases
                // Since this program is working on the computer, 
                // these request should not be normally be handled.
                // This is here for debugging purposes (2 instances communicating with each other)
                else if (resCode == 102)
                {
                    try
                    {
                        ShowData(inputData);
                        serialPort.Write("[12,1,64,102,0,1,1,1,1,0,0,d8a6]");
                        //SendData(12, byte.Parse(inputElements[1]));
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Source + " " + error.Message);
                    }
                }
                else if (resCode == 103)
                {
                    try
                    {
                        ShowData(inputData);
                        //SendData(13, byte.Parse(inputElements[1]));
                        serialPort.Write("[13,1,64,103,10gb,a6e5]");
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Source + " " + error.Message);
                    }
                }
                else if (resCode == 140)
                {
                    try
                    {
                        ShowData(inputData);
                        //SendData(13, byte.Parse(inputElements[1]));
                        if (int.Parse(inputElements[2]) == 1) serialPort.Write("[0;1,64,140,1,630d]");
                        else serialPort.Write("[1;4,64,140,1,af99]");
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Source + " " + error.Message);
                    }
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Operation timed out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Delagate for safe calls
        delegate void AddTextToTextBox(string text);
        // ShowData on the form text window
        private void ShowData(string text)
        {
            tbDataReceived.Clear();
            //invokeRequired required compares the thread ID of the calling thread to the thread of the creating thread.
            //if these threads are different, it returns true
            if (this.tbDataReceived.InvokeRequired)
            {
                this.Invoke(new AddTextToTextBox(ShowData), new object[] { text });
            }
            else
            {
                this.tbDataReceived.AppendText(text);
            }
        }
        // Make the target selection available only to the set output command
        private void cBoxCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)cBoxCommand.SelectedItem != "output_ctrl")
            {
                cBoxTarget.Enabled = false;
            }
            else
            {
                cBoxTarget.Enabled = true;
            }
        }
        // Update port on changing the selection in the port dropdown menu
        private void cBoxPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                if (serialPort.IsOpen == true) serialPort.Close();
                serialPort.PortName = cBoxPortName.Text;
                serialPort.Open();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Cannot open selected COM port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Closing the port when closing the form
        private void CommUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen) serialPort.Close();
        }
        private byte[] HexToByte(string input)
        {
            byte[] result = new byte[input.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(input.Substring(2 * i, 2), 16);
            }
            return result;
        }
        private string StringToHex(string input)
        {
            byte[] temp = Encoding.Default.GetBytes(input);
            var hexString = BitConverter.ToString(temp);
            hexString = hexString.Replace("-", "");
            return hexString;
        }
        // Parse data from string with separator to array of elements
        private string[] DataParser(string input) 
        {
            char[] sep = new char[] { ',', ']', '[', ';' };
            string[] subStrings = input.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            return subStrings;
        }
    }
}
