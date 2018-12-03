namespace AppModels.kurumi {
    public class TaskOfGroup {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int TaskId { get; set; }
        public int Status { get; set; }
        public string Content { get; set; }
        public string Pic { get; set; }
        public int? Period { get; set; }
    }
}