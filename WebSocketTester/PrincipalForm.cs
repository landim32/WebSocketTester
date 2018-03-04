using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace WebSocketTester
{
    public partial class PrincipalForm : Form
    {
        private WebSocket ws = null;

        public PrincipalForm()
        {
            InitializeComponent();
            //disableSendBtn();
        }

        private void createWebSocket() {
            ws = new WebSocket(UrlTextBox.Text);
            ws.WaitTime = TimeSpan.FromSeconds(10);
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.OnError += Ws_OnError;
            ws.OnMessage += Ws_OnMessage;
            ws.ConnectAsync();
            logTextBox.AppendText("Try connect to " + UrlTextBox.Text + "...\r\n");
        }

        private void enableConnectBtn() {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    GoBtn.Text = "DISCONNECT";
                    logTextBox.AppendText("Connect!\r\n");
                });
            }
            else {
                GoBtn.Text = "DISCONNECT";
                logTextBox.AppendText("Connect!\r\n");
            }
        }

        private void disableConnectBtn() {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    GoBtn.Text = "CONNECT";
                    logTextBox.AppendText("Disconnected!\r\n");
                });
            }
            else
            {
                GoBtn.Text = "CONNECT";
                logTextBox.AppendText("Disconnected!\r\n");
            }
        }

        private void enableSendBtn() {
            SendButton.Text = "SEND";
            SendButton.Enabled = true;
        }

        private void disableSendBtn() {
            SendButton.Text = "SENDING...";
            SendButton.Enabled = false;
        }

        private void GoBtn_Click(object sender, EventArgs e)
        {
            if (ws != null)
            {
                if (ws.IsAlive)
                {
                    ws.Close();
                    ws = null;
                    //disableConnectBtn();
                }
                else
                {
                    createWebSocket();
                }
            }
            else {
                createWebSocket();
            }
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            enableConnectBtn();

        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            disableConnectBtn();
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate {
                    logTextBox.AppendText("--received----------------\r\n");
                    logTextBox.AppendText(e.Data + "\r\n");
                });
            }
           
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate {
                    logTextBox.AppendText("Error: " + e.Message + "\r\n");
                    SendButton.Text = "Send";
                    SendButton.Enabled = true;
                });
            }
          
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (ws != null && ws.IsAlive)
            {
                disableSendBtn();
                var texto = envioTextBox.Text;
                envioTextBox.Text = "";
                ws.SendAsync(texto, (bool enviado) =>
                {
                    if (InvokeRequired)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            logTextBox.AppendText("--sended----------------\r\n");
                            logTextBox.AppendText(texto + "\r\n");
                            enableSendBtn();
                        });
                    }
                });
            }
            else {
                logTextBox.AppendText("Not connected!\r\n");
                //enableSendBtn();
            }
        }
    }
}
