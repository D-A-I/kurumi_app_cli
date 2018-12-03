using System;
using System.Collections.Generic;

namespace AppModels.kurumi
{
    public partial class Tasks
    {
        public int TaskId { get; set; }
        public int GroupId { get; set; }
        public int Status { get; set; }
        public string Content { get; set; }
        public string Pic { get; set; }
        public int? Period { get; set; }
    }
}
