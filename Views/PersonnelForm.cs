using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;


public partial class PersonnelForm : Form
{
    public PersonnelForm(Personnel personnel, ClientsList clients, int activeWindow)
    {
        InitializeComponent();
        InitializeDataGridView(personnel, clients);
        tabControl1.SelectTab(activeWindow);
        this.FormClosing += (s,e) => { Config.UpdatePersonnel(personnel); Config.UpdateClients(clients); };
    }
    private void InitializeDataGridView(Personnel personnel, ClientsList clients)
    {
        
        tabPage1.Text = "אנשי מקצוע";
        tabPage3.Text = "לקוחות";
        personnelGridView.AutoGenerateColumns = true;
        clientsGridView.AutoGenerateColumns = true;
        personnelGridView.AllowUserToAddRows = true;
        personnelGridView.ReadOnly = false;
        personnelGridView.DataSource = personnel.PersonList;
        clientsGridView.DataSource = clients.Clients;

    }


}

