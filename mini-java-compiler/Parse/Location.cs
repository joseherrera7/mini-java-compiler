namespace mini_java_compiler.Parse
{

    public class Location
    {
        private int position;
        private int lineNr;
        private int columnNr;

        public Location(int position, int lineNr, int columnNr)
        {
            Init(position, lineNr, columnNr);
        }

        public Location(Location location)
        {
            Init(location.position, location.lineNr, location.columnNr);
        }

        private void Init(int position, int lineNr, int columnNr)
        {
            this.position = position;
            this.lineNr = lineNr;
            this.columnNr = columnNr;
        }

        public Location Clone()
        {
            return new Location(this);
        }
        public override string ToString()
        {
            return "(posición: " + (position + 0) + ", línea: " + (lineNr + 1) + ", columna: " + (columnNr + 1) + ")";
        }

        public void NextLine()
        {
            position++;
            lineNr++;
            columnNr = 0;
        }

        public void NextColumn()
        {
            position++;
            columnNr++;
        }

        public int Position
        {
            get { return position; }
        }

        public int LineNr
        {
            get { return lineNr; }
        }

        public int ColumnNr
        {
            get { return columnNr; }
        }
    }

}
