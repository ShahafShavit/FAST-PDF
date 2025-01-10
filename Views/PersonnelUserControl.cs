using System.ComponentModel;


public partial class PersonnelUserControl : UserControl
{
    private Stack<(int rowIndex, int columnIndex, object previousValue)> undoStack = new Stack<(int, int, object)>();
    private object oldCellValue;
    private bool suppressUndoStack = false;

    public PersonnelUserControl()
    {
        InitializeComponent();

    }
    public void SetDataSource<T>(BindingList<T> List)
    {
        dataGridView1.AutoGenerateColumns = true;
        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
        dataGridView1.DataSource = List;
        dataGridView1.KeyDown += (s, e) =>
        {
            // Handle Delete or Backspace keys
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    if (!cell.ReadOnly) // Ensure the cell is editable
                    {
                        undoStack.Push((cell.RowIndex, cell.ColumnIndex, cell.Value)); // Store previous value
                        Console.WriteLine("Added to stack: " + undoStack.Peek());
                        cell.Value = null; // Clear the cell's value
                    }
                }

                e.Handled = true; // Prevent default behavior
            }

            // Handle Ctrl+V for pasting clipboard content
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (dataGridView1.CurrentCell != null)
                {
                    undoStack.Push((dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.Value));
                    string clipboardText = Clipboard.GetText();
                    dataGridView1.CurrentCell.Value = clipboardText; // Overwrite current cell with clipboard text
                    e.Handled = true;
                }
            }

            // Handle Ctrl+Z for undo
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (undoStack.Count > 0)
                {
                    var (rowIndex, columnIndex, previousValue) = undoStack.Pop();

                    suppressUndoStack = true; // Disable tracking temporarily
                    dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = previousValue; // Restore old value
                    suppressUndoStack = false; // Re-enable tracking
                }

                e.Handled = true; // Prevent default behavior
            }
        };

        dataGridView1.CellBeginEdit += (s, e) =>
        {
            oldCellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        };
        dataGridView1.CellValueChanged += (s, e) =>
        {
            if (suppressUndoStack) return; // Skip adding to undo stack

            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell != null)
            {
                undoStack.Push((e.RowIndex, e.ColumnIndex, oldCellValue));
                Console.WriteLine($"Pushed to undo stack: Row {e.RowIndex}, Column {e.ColumnIndex}, Value {oldCellValue}");
            }
        };




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


    private void SwapItems<T>(BindingList<T> list, int index1, int index2)
    {
        var temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }



}

