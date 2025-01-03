using System.ComponentModel;
using System.Data;


public partial class PersonnelForm : Form
{
    public PersonnelForm(Personnel personnel, ClientsList clients, int activeWindow)
    {
        InitializeComponent();
        InitializeDataGridView(personnel, clients);
        tabControl1.SelectTab(activeWindow);

        this.FormClosing += (s, e) => { Config.UpdatePersonnel(personnel); Config.UpdateClients(clients); };
    }
    private void InitializeDataGridView(Personnel personnel, ClientsList clients)
    {
        var tabPages = tabControl1.TabPages.Cast<TabPage>();
        object[] objects = { personnel.PersonList, clients.Clients };
        var zipped = objects.Zip(tabPages, (data, tabs) => (Data: data, Tabs: tabs));

        foreach (var pairs in zipped)
        {
            PersonnelUserControl puc = new PersonnelUserControl();

            if (pairs.Data is BindingList<Person> personList)
            {
                puc.SetDataSource(personList);
            }
            else if (pairs.Data is BindingList<Client> clientList)
            {
                puc.SetDataSource(clientList);
            }

            pairs.Tabs.Controls.Clear();
            pairs.Tabs.Controls.Add(puc);
            puc.Dock = DockStyle.Fill;

            //puc.Anchor = AnchorStyles.None;
        }
        /*
        tabPage1.Text = "אנשי מקצוע";
        tabPage3.Text = "לקוחות";
        personnelGridView.AutoGenerateColumns = true;
        clientsGridView.AutoGenerateColumns = true;
        personnelGridView.AllowUserToAddRows = true;
        personnelGridView.ReadOnly = false;
        personnelGridView.DataSource = personnel.PersonList;
        clientsGridView.DataSource = clients.Clients;
        */
    }

    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
    {

    }
}

