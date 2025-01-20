using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ProblemStruct
    {
        [Key]
        public int Id { get; set; }
        public string problemName { get; set; } = string.Empty;

        public string problemDescription { get; set; } = string.Empty;

        public List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }

    public class TestCase
    {
        public int? Id { get; set; }
        public string input { get; set; } = string.Empty;

        public string expectedOutput { get; set; } = string.Empty;
    }

    public class Code
    {
        [Key]
        public int submissionId { get; set; }
        
        public string source { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
    }

    public class Submission{

        [Key]
        public int submissionId { get; set; }

        public int problemId { get; set; }

        public int userId { get; set; }


        public string source { get; set; } = string.Empty;

        public string language { get; set; } = string.Empty;


        public bool accepted { get; set; } = false;
        public string status { get; set; } = string.Empty;

    }



    
}