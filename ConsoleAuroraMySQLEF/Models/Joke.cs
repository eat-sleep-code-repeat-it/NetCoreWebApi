﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleAuroraMySQLEF.Models
{
    public class Joke
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
