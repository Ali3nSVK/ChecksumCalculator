using System.Windows.Controls;

namespace ChecksumCalculator.Data
{
    public enum HashType
    {
        CRC32,
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public class HashUiElement
    {
        public HashType type;
        public Label label;
        public Button button;
        public TextBox text;
        public ProgressBar prog;

        public void SetColumns()
        {
            label?.SetValue(Grid.ColumnProperty, 0);
            text?.SetValue(Grid.ColumnProperty, 1);
            prog?.SetValue(Grid.ColumnProperty, 1); 
            button?.SetValue(Grid.ColumnProperty, 2);
        }

        public void SetRow(int row)
        {
            label?.SetValue(Grid.RowProperty, row);
            text?.SetValue(Grid.RowProperty, row);
            prog?.SetValue(Grid.RowProperty, row);
            button?.SetValue(Grid.RowProperty, row);
        }
    }
}
