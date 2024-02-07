namespace _4932Assignment1
{
    // Constants used to detect user intention with lines
    public static class Action
    {
        public const int Move = 4;
        public const int ResizeStart = 5;
        public const int ResizeEnd = 6;
        public const int Delete = 7;
        public const int CreateLine = 8;
    }

    // Constants for working with imagebase form type
    public static class FormType
    {
        public const int SOURCE = 1;
        public const int DESTINATION = 2;
        public const int FINAL = 3;
        public const string SOURCE_STR = "Source";
        public const string DESTINATION_STR = "Destination";
        public const string FINAL_STR = "Final";
    }
}
