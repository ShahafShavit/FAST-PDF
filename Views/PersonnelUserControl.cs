using System.ComponentModel;


public partial class PersonnelUserControl : UserControl
{
    public PersonnelUserControl()
    {
        InitializeComponent();
    }
    public void SetDataSource<T>(BindingList<T> List)
    {
        dataGridView1.AutoGenerateColumns = true;
        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
        dataGridView1.DataSource = List;
    }

    private void upRowBtn_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count == 0)
        {
            //MessageBox.Show("Please select a cell to move up.");
            return;
        }

        var rowIndex = dataGridView1.SelectedCells[0].RowIndex;
        var columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
        if (rowIndex == dataGridView1.Rows.Count - 1) { return; }
        if (rowIndex == 0)
        {
            //MessageBox.Show("The selected row is already at the top.");
            return;
        }

        if (dataGridView1.DataSource is BindingList<Person> personList)
        {
            SwapItems(personList, rowIndex, rowIndex - 1);
        }
        else if (dataGridView1.DataSource is BindingList<Client> clientList)
        {
            SwapItems(clientList, rowIndex, rowIndex - 1);
        }

        dataGridView1.ClearSelection();

        dataGridView1.Rows[rowIndex - 1].Cells[columnIndex].Selected = true;
    }

    private void downRowBtn_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count == 0)
        {
            //MessageBox.Show("Please select a cell to move down.");
            return;
        }

        var rowIndex = dataGridView1.SelectedCells[0].RowIndex;
        var columnIndex = dataGridView1.SelectedCells[0].ColumnIndex;
        if (rowIndex == dataGridView1.Rows.Count - 1) { return; }
        if (rowIndex == dataGridView1.Rows.Count - 2) // to account for the extra generation row
        {
            MessageBox.Show("The selected row is already at the bottom.");
            return;
        }

        if (dataGridView1.DataSource is BindingList<Person> personList)
        {
            SwapItems(personList, rowIndex, rowIndex + 1);
        }
        else if (dataGridView1.DataSource is BindingList<Client> clientList)
        {
            SwapItems(clientList, rowIndex, rowIndex + 1);
        }

        dataGridView1.ClearSelection();
        dataGridView1.Rows[rowIndex + 1].Cells[columnIndex].Selected = true;
    }

    private void deleteRowBtn_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count == 0)
        {
            //MessageBox.Show("Please select a cell to delete a row.");
            return;
        }

        var rowIndex = dataGridView1.SelectedCells[0].RowIndex;

        if (dataGridView1.DataSource is BindingList<Person> personList)
        {
            personList.RemoveAt(rowIndex);
        }
        else if (dataGridView1.DataSource is BindingList<Client> clientList)
        {
            clientList.RemoveAt(rowIndex);
        }
    }

    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
    {

    }

    private void SwapItems<T>(BindingList<T> list, int index1, int index2)
    {
        var temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }



}

