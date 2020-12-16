namespace BioMad_backend.Areas.Admin.Models
{
    public enum TableActionButtonType
    {
        Edit,
        Delete
    }

    public class TableActionButton
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public object Arguments { get; set; }
    }
}