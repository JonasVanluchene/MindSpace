using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MindSpace.Model
{
    [Table(nameof(ReflectionQuestion))]
    public class ReflectionQuestion
    {
        public int Id { get; set; }
        public required string QuestionText { get; set; }
        
    }
}
